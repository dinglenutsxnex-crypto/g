using System;
using Godot;

[Serializable]
public struct Vector3Serializer
{
    public float x;
    public float y;
    public float z;

    public Vector3 V3
    {
        get { return new Vector3(x, y, z); }
    }

    public Vector3Serializer(Vector3 v3)
    {
        x = v3.X;
        y = v3.Y;
        z = v3.Z;
    }

    public static implicit operator Vector3(Vector3Serializer v)
    {
        return v.V3;
    }
}
