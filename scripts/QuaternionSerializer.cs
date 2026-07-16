using System;
using Godot;

[Serializable]
public struct QuaternionSerializer
{
    public float x;
    public float y;
    public float z;
    public float w;

    public Vector3 eulerAngles
    {
        get { return Q.GetEuler(); }
    }

    public Quaternion Q
    {
        get { return new Quaternion(x, y, z, w); }
    }

    public QuaternionSerializer(Quaternion q)
    {
        x = q.X;
        y = q.Y;
        z = q.Z;
        w = q.W;
    }

    public static implicit operator Quaternion(QuaternionSerializer q)
    {
        return q.Q;
    }
}
