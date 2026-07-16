// ⚠️ STUB: needs full port — original used MeshRenderer, Quaternion.FromToRotation, editor capsule transform math
using System;
using Godot;

namespace SF3.CollisionEditor
{
	[Tool]
	public partial class EditorCapsule : Node3D
	{
		public enum ECapsuleType
		{
			NONE, NORMAL, COLLISIBLE, ATTACK
		}

		public EditorVertex startNode;
		public EditorVertex endNode;
		private MeshInstance3D meshRenderer;
		public ECapsuleType capsuleType;
		public float radius = 5f;
		public float marginStart;
		public float marginEnd;
		public ECapsuleType savedType;
		public string leftMirrorCapsule = string.Empty;
		public bool repulsive;

		public bool visible
		{
			get { return Visible; }
		}

		public override void _Ready()
		{
		}

		public override void _Process(double delta)
		{
		}

		public void InitCapsule(EditorVertex startNodeVal, EditorVertex endNodeVal)
		{
		}

		public void SetRadius(float value)
		{
			radius = value;
		}

		public void AddUpdateCallback(Action<EditorCapsule> cb)
		{
		}
	}
}
