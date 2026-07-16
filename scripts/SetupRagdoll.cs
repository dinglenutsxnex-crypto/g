using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using SF3.GameModels;

public class SetupRagdoll : Node
{
	private class RagdollPartData
	{
		private CapsuleCollider _capsuleCollider;
		private BoxCollider _boxCollider;
		private SphereCollider _sphereCollider;
		private Rigidbody _rigidBody;
		private CharacterJoint _joint;
		private Vector3 _localPosition;
		private Vector3 _localAngles;
		private float _weight;

		public CapsuleCollider capsuleCollider { get { return _capsuleCollider; } }
		public BoxCollider boxCollider { get { return _boxCollider; } }
		public SphereCollider sphereCollider { get { return _sphereCollider; } }
		public Rigidbody rigidBody { get { return _rigidBody; } }
		public CharacterJoint joint { get { return _joint; } }
		public Vector3 localPosition { get { return _localPosition; } }
		public Vector3 localAngles { get { return _localAngles; } }
		public float weight { get { return _weight; } }

		public RagdollPartData(Node3D boneTransform, IBonesHolder model)
		{
			_rigidBody = boneTransform.GetNode<Rigidbody>();
			_joint = boneTransform.GetNode<CharacterJoint>();
			Collider component = boneTransform.GetNode<Collider>();
			if (component is CapsuleCollider)
			{
				_capsuleCollider = (CapsuleCollider)component;
			}
			else if (component is BoxCollider)
			{
				_boxCollider = (BoxCollider)component;
			}
			else if (component is SphereCollider)
			{
				_sphereCollider = (SphereCollider)component;
			}
			_localPosition = boneTransform.Position;
			_localAngles = boneTransform.RotationDegrees;
			_weight = model.GetBone(boneTransform.Name).weight;
		}

		private void ActivateCollider(Collider collider, bool activate)
		{
			if (collider != null)
			{
				collider.Enabled = activate;
			}
		}

		public void Activate()
		{
			ActivateCollider(_capsuleCollider, true);
			ActivateCollider(_boxCollider, true);
			ActivateCollider(_sphereCollider, true);
			if (_rigidBody != null)
			{
				_rigidBody.FreezeMode = Rigidbody.FreezeModeEnum.Static;
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
				_rigidBody.FreezeMode = Rigidbody.FreezeModeEnum.Static;
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
			CharacterJoint joint = _ragdollData[key].joint;
			if (joint != null)
			{
				CharacterJoint characterJoint = _modelBones[key].boneObject.GetNode<CharacterJoint>();
				if (characterJoint == null)
				{
					characterJoint = new CharacterJoint();
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
				Rigidbody rigidbody = _modelBones[key].boneObject.GetNode<Rigidbody>();
				if (rigidbody == null)
				{
					rigidbody = new Rigidbody();
					_modelBones[key].boneObject.AddChild(rigidbody);
				}
				rigidbody.Mass = _ragdollData[key].weight;
			}
			if (_ragdollData[key].sphereCollider != null)
			{
				SphereCollider sphereCollider = _modelBones[key].boneObject.GetNode<SphereCollider>();
				if (sphereCollider == null)
				{
					sphereCollider = new SphereCollider();
					_modelBones[key].boneObject.AddChild(sphereCollider);
				}
				sphereCollider.Radius = _ragdollData[key].sphereCollider.Radius;
				sphereCollider.Position = _ragdollData[key].sphereCollider.Position;
			}
			else if (_ragdollData[key].boxCollider != null)
			{
				BoxCollider boxCollider = _modelBones[key].boneObject.GetNode<BoxCollider>();
				if (boxCollider == null)
				{
					boxCollider = new BoxCollider();
					_modelBones[key].boneObject.AddChild(boxCollider);
				}
				boxCollider.Size = _ragdollData[key].boxCollider.Size;
				boxCollider.Position = _ragdollData[key].boxCollider.Position;
			}
			else if (_ragdollData[key].capsuleCollider != null)
			{
				CapsuleCollider capsuleCollider = _modelBones[key].boneObject.GetNode<CapsuleCollider>();
				if (capsuleCollider == null)
				{
					capsuleCollider = new CapsuleCollider();
					_modelBones[key].boneObject.AddChild(capsuleCollider);
				}
				capsuleCollider.Height = _ragdollData[key].capsuleCollider.Height;
				capsuleCollider.Radius = _ragdollData[key].capsuleCollider.Radius;
				capsuleCollider.Position = _ragdollData[key].capsuleCollider.Position;
			}
		}
	}
}
