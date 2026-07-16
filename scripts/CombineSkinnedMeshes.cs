using System.Collections.Generic;
using Godot;

public partial class CombineSkinnedMeshes : Node3D
{
	public bool combineAllMeshesNow;

	public void Init()
	{
		MeshInstance3D[] componentsInChildren = GetChildren().ConvertTo<MeshInstance3D[]>();
		Transform3D worldToLocalMatrix = GlobalTransform.AffineInverse();
		Dictionary<string, int> boneNameToIndex = new Dictionary<string, int>();
		int num = 0;
		int num2 = 0;
		List<SkinMeshCombineUtility.MeshInstance> list = new List<SkinMeshCombineUtility.MeshInstance>();
		List<Material> list2 = new List<Material>();
		int num3 = 0;
		List<Node3D> list3 = new List<Node3D>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].Skin != null)
			{
				num3 += 4;
			}
			if (componentsInChildren[i].GetChildCount() > 0)
			{
				foreach (Node child in componentsInChildren[i].GetChildren())
				{
					if (child is Node3D boneNode && !list3.Contains(boneNode))
					{
						list3.Add(boneNode);
					}
				}
			}
		}
		Node3D[] array2 = new Node3D[list3.Count];
		Transform3D[] array3 = new Transform3D[list3.Count];
		Node3D[] array4 = new Node3D[array2.Length];
		for (int k = 0; k < componentsInChildren.Length; k++)
		{
			MeshInstance3D meshInstance = componentsInChildren[k];
			if (meshInstance == null || meshInstance.Mesh == null)
			{
				continue;
			}
			SkinMeshCombineUtility.MeshInstance item2 = default(SkinMeshCombineUtility.MeshInstance);
			item2.mesh = meshInstance.Mesh;
			for (int l = 0; l < meshInstance.Mesh.GetSurfaceCount(); l++)
			{
				Material mat = meshInstance.Mesh.SurfaceGetMaterial(l);
				if (mat != null)
					list2.Add(mat);
			}
			if (!meshInstance.Visible)
			{
				continue;
			}
			item2.transform = worldToLocalMatrix * meshInstance.GlobalTransform;
			for (int n = 0; n < list3.Count; n++)
			{
				bool flag = false;
				for (int num4 = 0; num4 < array2.Length; num4++)
				{
					if (array4[num4] != null && list3[n] == array4[num4])
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					continue;
				}
				for (int num5 = 0; num5 < array2.Length; num5++)
				{
					if (array4[num5] == null)
					{
						array4[num5] = list3[n];
						break;
					}
				}
				array2[num] = list3[n];
				array3[num] = Transform3D.Identity;
				boneNameToIndex.Add(list3[n].Name, num);
				num++;
			}
			meshInstance.Visible = false;
		}
		SkinMeshCombineUtility.MeshInstance[] combines = list.ToArray();
		if (GetNodeOrNull<MeshInstance3D>(".") == null)
		{
			MeshInstance3D combined = new MeshInstance3D();
			combined.Name = "CombinedMesh";
			AddChild(combined);
		}
		MeshInstance3D component = GetNode<MeshInstance3D>("CombinedMesh");
		component.Mesh = SkinMeshCombineUtility.Combine(combines);
		component.Visible = true;
	}

	public override void _Process(double delta)
	{
		if (combineAllMeshesNow)
		{
			combineAllMeshesNow = false;
			Init();
		}
	}
}
