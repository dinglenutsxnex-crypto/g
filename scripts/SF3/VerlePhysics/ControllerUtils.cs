using Godot;

namespace SF3.VerlePhysics
{
	internal static class ControllerUtils
	{
		public static void ExtractTransform(Transform3D t, out Vector3 position, out Quaternion rotation, out Vector3 scale)
		{
			position = t.Origin;
			rotation = t.Basis.GetQuaternion();
			scale = t.Basis.Scale;
		}

		public static Transform3D GenerateMatrixFrom3Nodes(Node main, Node help1, Node help2)
		{
			Vector3 col0 = (help1.position - main.position).Normalized();
			Vector3 col1 = col0.Cross(help2.position - help1.position).Normalized();
			Vector3 col2 = col0.Cross(col1).Normalized();
			Basis basis = new Basis(col0, col1, col2);
			return new Transform3D(basis, main.position);
		}
	}
}
