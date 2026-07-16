using Godot;
using SF3.GameModels;

namespace SF3.VerlePhysics
{
	public class BoneController
	{
		protected Node[] _nodes;

		protected Bone _bone;

		protected Transform3D _bindingTransform;

		public Node[] nodes
		{
			get
			{
				return _nodes;
			}
		}

		public virtual string BoneName
		{
			get
			{
				return _bone.boneName;
			}
		}

		public Transform3D BindingTransform
		{
			get
			{
				return _bindingTransform;
			}
		}

		protected Transform3D NodeMatrix
		{
			get
			{
				return ControllerUtils.GenerateMatrixFrom3Nodes(_nodes[0], _nodes[1], _nodes[2]);
			}
		}

		public BoneController(Node node1, Node node2, Node node3)
			: this(node1, node1, node3, null)
		{
		}

		public BoneController(Node node1, Node node2, Node node3, Bone bone)
		{
			_nodes = new Node[3];
			_nodes[0] = node1;
			_nodes[1] = node2;
			_nodes[2] = node3;
			_bone = bone;
			_bindingTransform = NodeMatrix.AffineInverse() * bone.transform.GlobalTransform;
		}

		public void SetBonePosition()
		{
			if (!_bone.animatedThisFrame)
			{
				Transform3D nodeMatrix = NodeMatrix;
				nodeMatrix = nodeMatrix * _bindingTransform;
				Vector3 position;
				Quaternion rotation;
				Vector3 scale;
				ControllerUtils.ExtractTransform(nodeMatrix, out position, out rotation, out scale);
				_bone.SetRotation(rotation);
				_bone.SetPosition(position);
			}
		}
	}
}
