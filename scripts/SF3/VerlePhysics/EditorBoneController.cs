using System;
using Godot;

namespace SF3.VerlePhysics
{
	[Serializable]
	internal class EditorBoneController : BoneController
	{
		private Node3D elementTransform;

		public override string BoneName
		{
			get
			{
				return elementTransform.Name;
			}
		}

		public EditorBoneController(Node node1, Node node2, Node node3, Node3D transform)
			: base(node1, node2, node3)
		{
			elementTransform = transform;
			RecalculateBindingMatrix();
		}

		public void RecalculateBindingMatrix()
		{
			Transform3D nodeMatrix = base.NodeMatrix;
			_bindingTransform = elementTransform.GlobalTransform * nodeMatrix.AffineInverse();
		}
	}
}
