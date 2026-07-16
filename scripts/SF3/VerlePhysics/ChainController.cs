using System.Collections.Generic;
using Godot;
using SF3.GameModels;

namespace SF3.VerlePhysics
{
	public class ChainController
	{
		public class ChainLink
		{
			public Node myNode;

			public Node nextNode;

			public Bone myBone;

			public Node3D myTransform;

			public Transform3D bindingTransform;

			public Quaternion calculatedRotation;

			public Vector3 calculatedPosition;

			public ChainLink(Node node1, Node node2, Bone bone)
			{
				myNode = node1;
				nextNode = node2;
				myBone = bone;
				myTransform = bone.transform;
			}
		}

		public Node3D baseRotationBone;

		private List<ChainLink> links = new List<ChainLink>();

		private bool initializedCorrectly;

		public List<ChainLink> Links
		{
			get
			{
				return links;
			}
		}

		public string MainBoneName
		{
			get
			{
				return baseRotationBone.Name;
			}
		}

		public ChainController(Node3D basicBone, List<Node> nodes, List<Bone> bones)
		{
			baseRotationBone = basicBone;
			int count = nodes.Count;
			int count2 = bones.Count;
			if (count2 == count - 1)
			{
				for (int i = 0; i < bones.Count; i++)
				{
					ChainLink item = new ChainLink(nodes[i], nodes[i + 1], bones[i]);
					links.Add(item);
				}
				RecalculateBindingMatrices();
				initializedCorrectly = true;
			}
		}

		protected void RecalculateBindingMatrices()
		{
			CalculateLinksFromNodes();
			for (int i = 0; i < links.Count; i++)
			{
				Transform3D m = new Transform3D(new Basis(links[i].calculatedRotation), links[i].calculatedPosition);
				links[i].bindingTransform = m.AffineInverse() * links[i].myTransform.GlobalTransform;
			}
		}

		private static Quaternion FromToRotation(Vector3 from, Vector3 to)
		{
			from = from.Normalized();
			to = to.Normalized();
			float dot = from.Dot(to);
			if (dot > 0.99999f)
				return Quaternion.Identity;
			if (dot < -0.99999f)
			{
				Vector3 axis = Vector3.Right.Cross(from);
				if (axis.LengthSquared() < 0.0001f)
					axis = Vector3.Up.Cross(from);
				axis = axis.Normalized();
				return new Quaternion(axis, Mathf.Pi);
			}
			Vector3 cross = from.Cross(to).Normalized();
			float w = Mathf.Sqrt((1f + dot) * 2f);
			float s = 1f / w;
			return new Quaternion(cross.X * s, cross.Y * s, cross.Z * s, w * 0.5f).Normalized();
		}

		private void CalculateLinksFromNodes()
		{
			Vector3 fromDirection = baseRotationBone.Transform.Basis.X;
			Quaternion quaternion = baseRotationBone.Quaternion;
			for (int i = 0; i < links.Count; i++)
			{
				ChainLink chainLink = links[i];
				Vector3 vector = chainLink.nextNode.position - chainLink.myNode.position;
				Quaternion quaternion2 = FromToRotation(fromDirection, vector);
				Quaternion quaternion3 = quaternion2 * quaternion;
				chainLink.calculatedPosition = chainLink.myNode.position;
				chainLink.calculatedRotation = quaternion3;
				fromDirection = vector;
				quaternion = quaternion3;
			}
		}

		public void SetBonesPositions()
		{
			if (!initializedCorrectly)
			{
				return;
			}
			for (int i = 0; i < links.Count; i++)
			{
				if (links[i].myBone.animatedThisFrame)
				{
					return;
				}
			}
			CalculateLinksFromNodes();
			for (int j = 0; j < links.Count; j++)
			{
				Transform3D transform3d = new Transform3D(new Basis(links[j].calculatedRotation), links[j].calculatedPosition);
				Transform3D m = transform3d * links[j].bindingTransform;
				Vector3 position;
				Quaternion rotation;
				Vector3 scale;
				ControllerUtils.ExtractTransform(m, out position, out rotation, out scale);
				links[j].myBone.SetRotation(rotation);
				links[j].myBone.SetPosition(position);
			}
		}
	}
}
