using Godot;
using System;

public static class SkinMeshCombineUtility
{
    public struct MeshInstance
    {
        public Mesh mesh;
        public int subMeshIndex;
        public Transform3D transform;
    }

    public static ArrayMesh Combine(MeshInstance[] combines)
    {
        int totalVerts = 0;
        int totalTris = 0;
        for (int i = 0; i < combines.Length; i++)
        {
            var ci = combines[i];
            if (ci.mesh == null) continue;
            for (int s = 0; s < ci.mesh.GetSurfaceCount(); s++)
            {
                if (s != ci.subMeshIndex) continue;
                var arr = ci.mesh.SurfaceGetArrays(s);
                totalVerts += ((Godot.PackedVector3Array)arr[(int)Mesh.ArrayType.Vertex]).Count;
                totalTris += ((Godot.PackedInt32Array)arr[(int)Mesh.ArrayType.Index]).Count / 3;
            }
        }

        Godot.PackedVector3Array dstVerts = new Godot.PackedVector3Array();
        dstVerts.Resize(totalVerts);
        Godot.PackedVector3Array dstNormals = new Godot.PackedVector3Array();
        dstNormals.Resize(totalVerts);
        Godot.PackedFloat32Array dstTangents = new Godot.PackedFloat32Array();
        dstTangents.Resize(totalVerts * 4);
        Godot.PackedVector2Array dstUvs = new Godot.PackedVector2Array();
        dstUvs.Resize(totalVerts);
        Godot.PackedVector2Array dstUvs2 = new Godot.PackedVector2Array();
        dstUvs2.Resize(totalVerts);

        int offset = 0;
        var result = new ArrayMesh();

        for (int si = 0; si < combines.Length; si++)
        {
            var ci = combines[si];
            if (ci.mesh == null) continue;

            for (int s = 0; s < ci.mesh.GetSurfaceCount(); s++)
            {
                if (s != ci.subMeshIndex) continue;

                var arr = ci.mesh.SurfaceGetArrays(s);
                var srcVerts = (Godot.PackedVector3Array)arr[(int)Mesh.ArrayType.Vertex];
                var srcNormals = (Godot.PackedVector3Array)arr[(int)Mesh.ArrayType.Normal];
                var srcTangents = (Godot.PackedFloat32Array)arr[(int)Mesh.ArrayType.Tangent];
                var srcUvs = (Godot.PackedVector2Array)arr[(int)Mesh.ArrayType.TexUV];
                var srcUvs2 = (Godot.PackedVector2Array)arr[(int)Mesh.ArrayType.TexUV2];
                var srcIndices = (Godot.PackedInt32Array)arr[(int)Mesh.ArrayType.Index];

                var t = ci.transform;
                var invTransBasis = t.Basis.Inverse().Transposed();

                for (int i = 0; i < srcVerts.Count; i++)
                {
                    dstVerts[offset + i] = t * srcVerts[i];
                    dstNormals[offset + i] = (invTransBasis * srcNormals[i]).Normalized();
                    dstUvs[offset + i] = srcUvs[i];
                    if (srcUvs2.Count > 0)
                        dstUvs2[offset + i] = srcUvs2[i];

                    if (srcTangents.Count > 0)
                    {
                        Vector3 tanDir = new Vector3(
                            srcTangents[i * 4],
                            srcTangents[i * 4 + 1],
                            srcTangents[i * 4 + 2]
                        );
                        tanDir = (invTransBasis * tanDir).Normalized();
                        dstTangents[offset * 4 + i * 4] = tanDir.X;
                        dstTangents[offset * 4 + i * 4 + 1] = tanDir.Y;
                        dstTangents[offset * 4 + i * 4 + 2] = tanDir.Z;
                        dstTangents[offset * 4 + i * 4 + 3] = srcTangents[i * 4 + 3];
                    }
                }

                Godot.PackedInt32Array dstIndices = new Godot.PackedInt32Array();
                dstIndices.Resize(srcIndices.Count);
                for (int i = 0; i < srcIndices.Count; i++)
                    dstIndices[i] = srcIndices[i] + offset;

                var dstArr = new Godot.Collections.Array();
                dstArr.Resize((int)Mesh.ArrayType.Max);

                Godot.PackedVector3Array surfVerts = new Godot.PackedVector3Array();
                Godot.PackedVector3Array surfNormals = new Godot.PackedVector3Array();
                Godot.PackedFloat32Array surfTangents = new Godot.PackedFloat32Array();
                Godot.PackedVector2Array surfUvs = new Godot.PackedVector2Array();
                Godot.PackedVector2Array surfUvs2 = new Godot.PackedVector2Array();

                for (int i = 0; i < srcVerts.Count; i++)
                {
                    surfVerts.Add(dstVerts[offset + i]);
                    surfNormals.Add(dstNormals[offset + i]);
                    surfUvs.Add(dstUvs[offset + i]);
                    if (srcUvs2.Count > 0)
                        surfUvs2.Add(dstUvs2[offset + i]);
                    if (srcTangents.Count > 0)
                    {
                        surfTangents.Add(dstTangents[offset * 4 + i * 4]);
                        surfTangents.Add(dstTangents[offset * 4 + i * 4 + 1]);
                        surfTangents.Add(dstTangents[offset * 4 + i * 4 + 2]);
                        surfTangents.Add(dstTangents[offset * 4 + i * 4 + 3]);
                    }
                }

                dstArr[(int)Mesh.ArrayType.Vertex] = surfVerts;
                dstArr[(int)Mesh.ArrayType.Normal] = surfNormals;
                dstArr[(int)Mesh.ArrayType.TexUV] = surfUvs;
                if (srcUvs2.Count > 0)
                    dstArr[(int)Mesh.ArrayType.TexUV2] = surfUvs2;
                if (srcTangents.Count > 0)
                    dstArr[(int)Mesh.ArrayType.Tangent] = surfTangents;
                dstArr[(int)Mesh.ArrayType.Index] = dstIndices;

                result.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, dstArr);
                offset += srcVerts.Count;
            }
        }

        return result;
    }

    private static void Copy(int vertexcount, Vector3[] src, Vector3[] dst, ref int offset, Transform3D transform)
    {
        for (int i = 0; i < src.Length; i++)
            dst[i + offset] = transform * src[i];
        offset += vertexcount;
    }

    private static void CopyNormal(int vertexcount, Vector3[] src, Vector3[] dst, ref int offset, Basis basis)
    {
        for (int i = 0; i < src.Length; i++)
            dst[i + offset] = (basis * src[i]).Normalized();
        offset += vertexcount;
    }

    private static void Copy(int vertexcount, Vector2[] src, Vector2[] dst, ref int offset)
    {
        for (int i = 0; i < src.Length; i++)
            dst[i + offset] = src[i];
        offset += vertexcount;
    }

    private static void CopyTangents(int vertexcount, Vector4[] src, Vector4[] dst, ref int offset, Basis basis)
    {
        for (int i = 0; i < src.Length; i++)
        {
            Vector4 v = src[i];
            Vector3 dir = new Vector3(v.X, v.Y, v.Z);
            dir = (basis * dir).Normalized();
            dst[i + offset] = new Vector4(dir.X, dir.Y, dir.Z, v.W);
        }
        offset += vertexcount;
    }
}
