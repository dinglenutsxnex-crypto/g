using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using SF3.GameModels;

public partial class SetupRagdoll : Node
{
	private class RagdollPartData
	{
		private CollisionShape3D _capsuleCollider;
		private CollisionShape3D _boxCollider;
		private CollisionShape3D _sphereCollider;
		private RigidBody3D _rigidBody;
		private Generic6DOFJoint3D _joint;
		private Vector3 _localPosition;
		private Vector3 _localAngles;
		private float _weight;

		public CollisionShape3D capsuleCollider { get { return _capsuleCollider; } }
		public CollisionShape3D boxCollider { get { return _boxCollider; } }
		public CollisionShape3D sphereCollider { get { return _sphereCollider; } }
		public RigidBody3D rigidBody { get { return _rigidBody; } }
		public Generic6DOFJoint3D joint { get { return _joint; } }
		public Vector3 localPosition { get { return _localPosition; } }
		public Vector3 localAngles { get { return _localAngles; } }
		public float weight { get { return _weight; } }

		public RagdollPartData(Node3D boneTransform, IBonesHolder model)
		{
			_rigidBody = boneTransform.GetNode<RigidBody3D>();
			_joint = boneTransform.GetNode<Generic6DOFJoint3D>();
			CollisionShape3D component = boneTransform.GetNode<CollisionShape3D>();
			if (component != null)
			{
				if (component.Shape is CapsuleShape3D)
				{
					_capsuleCollider = component;
				}
				else if (component.Shape is BoxShape3D)
				{
					_boxCollider = component;
				}
				else if (component.Shape is SphereShape3D)
				{
					_sphereCollider = component;
				}
			}
			_localPosition = boneTransform.Position;
			_localAngles = boneTransform.RotationDegrees;
			_weight = model.GetBone(boneTransform.Name).weight;
		}

		private void ActivateCollider(CollisionShape3D collider, bool activate)
		{
			if (collider != null)
			{
				collider.Disabled = !activate;
			}
		}

		public void Activate()
		{
			ActivateCollider(_capsuleCollider, true);
			ActivateCollider(_boxCollider, true);
			ActivateCollider(_sphereCollider, true);
			if (_rigidBody != null)
			{
				_rigidBody.FreezeMode = RigidBody3D.FreezeModeEnum.Static;
				_rigidBody.GravityScale = 1f;
			}
		}

		public void Disable()
		{
			ActivateCollider(_capsuleCollider, false);
			ActivateCollider(_boxCollider, false);
			ActivateCollider(_sphereCollider, false);
			if (_rigidBody != null)
			{
				_rigidBody.FreezeMode = RigidBody3D.FreezeModeEnum.Static;
				_rigidBody.GravityScale = 0f;
			}
		}
	}

	private class BoneData
	{
		private Node3D _boneObject;
		private Vector3 _localPosition;
		private Vector3 _localAngles;

		public Node3D boneObject { get { return _boneObject; } }

		public BoneData(Node3D boneObject)
		{
			_boneObject = boneObject;
			_localPosition = _boneObject.Position;
			_localAngles = _boneObject.RotationDegrees;
		}

		public void ResetToStartPose()
		{
			_boneObject.Position = _localPosition;
			_boneObject.RotationDegrees = _localAngles;
		}
	}

	[Export]
	private Node3D _ragdollSourcePrefab;

	[Export]
	private Node3D _parentArmature;

	private Dictionary<string, BoneData> _modelBones;
	private Dictionary<string, RagdollPartData> _ragdollData;
	private IBonesHolder _bonesHolder;

	public void Initialize(IBonesHolder bonesHolder)
	{
		_bonesHolder = bonesHolder;
		CollectData();
		SetRagdollPose();
		ConfigureRagdoll();
	}

	private void CollectData()
	{
		_modelBones = new Dictionary<string, BoneData>();
		_ragdollData = new Dictionary<string, RagdollPartData>();
		for (int i = 0; i < _parentArmature.GetChildCount(); i++)
		{
			ParseModelHierarchy(_parentArmature.GetChild(i));
		}
		for (int j = 0; j < _ragdollSourcePrefab.GetChildCount(); j++)
		{
			ParseRagdollHierarchy(_ragdollSourcePrefab.GetChild(j));
		}
		for (int k = 0; k < _modelBones.Count; k++)
		{
			KeyValuePair<string, BoneData> keyValuePair = _modelBones.ElementAt(k);
			if (!_ragdollData.ContainsKey(keyValuePair.Key))
			{
				_modelBones.Remove(keyValuePair.Key);
				k--;
			}
		}
	}

	private void ParseModelHierarchy(Node3D currentRootTransform)
	{
		if (_modelBones.ContainsKey(currentRootTransform.Name))
		{
			throw new Exception(string.Format("Parent skeleton already has bone with name [{0}] ", currentRootTransform.Name));
		}
		_modelBones.Add(currentRootTransform.Name, new BoneData(currentRootTransform));
		if (currentRootTransform.GetChildCount() > 0)
		{
			for (int i = 0; i < currentRootTransform.GetChildCount(); i++)
			{
				ParseModelHierarchy(currentRootTransform.GetChild(i));
			}
		}
	}

	private void ParseRagdollHierarchy(Node3D currentRootTransform)
	{
		if (_modelBones.ContainsKey(currentRootTransform.Name))
		{
			RagdollPartData value = new RagdollPartData(currentRootTransform, _bonesHolder);
			_ragdollData.Add(currentRootTransform.Name, value);
		}
		if (currentRootTransform.GetChildCount() > 0)
		{
			for (int i = 0; i < currentRootTransform.GetChildCount(); i++)
			{
				ParseRagdollHierarchy(currentRootTransform.GetChild(i));
			}
		}
	}

	public void SetRagdollPose()
	{
		foreach (string key in _ragdollData.Keys)
		{
			_modelBones[key].boneObject.Position = _ragdollData[key].localPosition;
			_modelBones[key].boneObject.RotationDegrees = _ragdollData[key].localAngles;
		}
	}

	public void SetTPose()
	{
		foreach (BoneData value in _modelBones.Values)
		{
			value.ResetToStartPose();
		}
	}

	private void ConfigureRagdoll()
	{
		foreach (string key in _ragdollData.Keys)
		{
			Generic6DOFJoint3D joint = _ragdollData[key].joint;
			if (joint != null)
			{
				Generic6DOFJoint3D characterJoint = _modelBones[key].boneObject.GetNode<Generic6DOFJoint3D>();
				if (characterJoint == null)
				{
					characterJoint = new Generic6DOFJoint3D();
					_modelBones[key].boneObject.AddChild(characterJoint);
				}
				characterJoint.ExcludeNodes = joint.ExcludeNodes;
				if (_modelBones.ContainsKey(joint.GetNodePath().ToString()))
				{
					characterJoint.SetNodePath(joint.GetNodePath());
				}
			}
			if (_ragdollData[key].rigidBody != null)
			{
				RigidBody3D rigidbody = _modelBones[key].boneObject.GetNode<RigidBody3D>();
				if (rigidbody == null)
				{
					rigidbody = new RigidBody3D();
					_modelBones[key].boneObject.AddChild(rigidbody);
				}
				rigidbody.Mass = _ragdollData[key].weight;
			}
			if (_ragdollData[key].sphereCollider != null)
			{
				CollisionShape3D sphereCollider = _modelBones[key].boneObject.GetNode<CollisionShape3D>();
				if (sphereCollider == null)
				{
					sphereCollider = new CollisionShape3D();
					sphereCollider.Shape = new SphereShape3D();
					_modelBones[key].boneObject.AddChild(sphereCollider);
				}
				SphereShape3D srcSphere = _ragdollData[key].sphereCollider.Shape as SphereShape3D;
				SphereShape3D dstSphere = sphereCollider.Shape as SphereShape3D;
				if (srcSphere != null && dstSphere != null)
				{
					dstSphere.Radius = srcSphere.Radius;
				}
				sphereCollider.Position = _ragdollData[key].sphereCollider.Position;
			}
			else if (_ragdollData[key].boxCollider != null)
			{
				CollisionShape3D boxCollider = _modelBones[key].boneObject.GetNode<CollisionShape3D>();
				if (boxCollider == null)
				{
					boxCollider = new CollisionShape3D();
					boxCollider.Shape = new BoxShape3D();
					_modelBones[key].boneObject.AddChild(boxCollider);
				}
				BoxShape3D srcBox = _ragdollData[key].boxCollider.Shape as BoxShape3D;
				BoxShape3D dstBox = boxCollider.Shape as BoxShape3D;
				if (srcBox != null && dstBox != null)
				{
					dstBox.Size = srcBox.Size;
				}
				boxCollider.Position = _ragdollData[key].boxCollider.Position;
			}
			else if (_ragdollData[key].capsuleCollider != null)
			{
				CollisionShape3D capsuleCollider = _modelBones[key].boneObject.GetNode<CollisionShape3D>();
				if (capsuleCollider == null)
				{
					capsuleCollider = new CollisionShape3D();
					capsuleCollider.Shape = new CapsuleShape3D();
					_modelBones[key].boneObject.AddChild(capsuleCollider);
				}
				CapsuleShape3D srcCapsule = _ragdollData[key].capsuleCollider.Shape as CapsuleShape3D;
				CapsuleShape3D dstCapsule = capsuleCollider.Shape as CapsuleShape3D;
				if (srcCapsule != null && dstCapsule != null)
				{
					dstCapsule.Height = srcCapsule.Height;
					dstCapsule.Radius = srcCapsule.Radius;
				}
				capsuleCollider.Position = _ragdollData[key].capsuleCollider.Position;
			}
		}
	}
}
