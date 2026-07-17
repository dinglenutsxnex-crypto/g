using System.Collections.Generic;
using Godot;

namespace SF3.GameModels
{
	public partial class SkeletonRagdoll : Node3D
	{
		private const int NONE_MIRRORING_DUPLICATE = -1;

		public bool activeOnStart;
		public string[] ignoredTransforms;

		private HashSet<string> _ignoredTransformsSet;
		private Joint3D[] _joints;
		private CollisionObject3D[] _colliders;
		private RigidBody3D[] _rigidBodies;
		private bool _hasIgnoredColliders;
		private Vector3[] _lastImpulse;
		private Vector3[] _lastTorque;
		private Vector3 _forceAwake;
		private Vector3 _torqueAwake;
		private bool _isSleeping;
		private int _sleepPriority;
		private bool isFrozen;

		public bool isActive { get; private set; }
		public int mirroringDuplicateIndex { get; private set; }

		public void Init(string mirroringDuplicateIndex)
		{
			isActive = false;
			_isSleeping = false;
			_sleepPriority = -1;
			_ignoredTransformsSet = new HashSet<string>(ignoredTransforms ?? System.Array.Empty<string>());
			ignoredTransforms = null;
			CollectJoints();
			CollectColliders();
			CollectRigidBodies();
			SetActive(activeOnStart);
			this.mirroringDuplicateIndex = !string.IsNullOrEmpty(mirroringDuplicateIndex) ? int.Parse(mirroringDuplicateIndex) : -1;
		}

		private void CollectJoints()
		{
			var all = new List<Joint3D>();
			GetComponentsInChildren(all);
			_joints = ApplyFilterIgnored(all).ToArray();
		}

		private void CollectColliders()
		{
			var all = new List<CollisionObject3D>();
			GetComponentsInChildren(all);
			_colliders = ApplyFilterIgnored(all).ToArray();
		}

		private void CollectRigidBodies()
		{
			var all = new List<RigidBody3D>();
			GetComponentsInChildren(all);
			_rigidBodies = ApplyFilterIgnored(all).ToArray();
		}

		private List<T> ApplyFilterIgnored<T>(List<T> components) where T : Node
		{
			if (_ignoredTransformsSet.Count > 0)
			{
				for (int i = 0; i < components.Count; i++)
				{
					if (_ignoredTransformsSet.Contains(components[i].Name))
					{
						components.RemoveAt(i);
						i--;
					}
				}
			}
			return components;
		}

		public void AddForce(Vector3 impulse, string capsuleName)
		{
			if (capsuleName.Length == 0 || impulse == Vector3.Zero) return;
			for (int i = 0; i < _rigidBodies.Length; i++)
			{
				if (_rigidBodies[i].Name.Equals(capsuleName))
				{
					foreach (var rb in _rigidBodies)
						rb.LinearVelocity = Vector3.Zero;
					_rigidBodies[i].ApplyImpulse(impulse);
					return;
				}
			}
			GD.Print($"Can't find rigidbody with capsule name [{capsuleName}]");
		}

		public void AddForceAwake(Vector3 impulse) => _forceAwake = impulse;
		public void AddTorqueAwake(Vector3 torque) => _torqueAwake = torque;

		public void AddForce(Vector3 impulse, int index = 0)
		{
			if (!isActive) SetActive(true);
			_rigidBodies[index].ApplyImpulse(impulse);
		}

		public void AddTorque(Vector3 torque, int index = 0)
		{
			if (!isActive) SetActive(true);
			_rigidBodies[index].ApplyTorqueImpulse(torque);
		}

		public void EnableColliders(bool enable)
		{
			if (!isFrozen)
			{
				foreach (var c in _colliders)
				{
					if (c is Area3D area) area.Monitoring = enable;
					else if (c is PhysicsBody3D pb) pb.CollisionLayer = enable ? 1u : 0u;
				}
			}
		}

		public void SetIsTriggerActive(bool active)
		{
			foreach (var c in _colliders)
			{
				if (c is Area3D area) area.Monitoring = active;
			}
		}

		public void SetActive(bool isActiveVal)
		{
			isActive = isActiveVal;
			EnableColliders(isActive);
			foreach (var j in _joints)
			{
				// GenericJoint3D doesn't have enableCollision, skip
			}
			foreach (var rb in _rigidBodies)
				rb.Freeze = !isActive;
		}

		public void FreezeObject(bool freeze)
		{
			isFrozen = freeze;
			foreach (var rb in _rigidBodies)
				rb.Freeze = freeze;
		}

		public void SetRagdollSleepState(bool isSleep, int priority)
		{
			if (_isSleeping == isSleep || _sleepPriority > priority) return;
			_isSleeping = isSleep;
			if (_isSleeping)
			{
				_lastImpulse = new Vector3[_rigidBodies.Length];
				_lastTorque = new Vector3[_rigidBodies.Length];
				for (int i = 0; i < _rigidBodies.Length; i++)
				{
					_lastImpulse[i] = _rigidBodies[i].LinearVelocity;
					_lastTorque[i] = _rigidBodies[i].AngularVelocity;
					_rigidBodies[i].Sleeping = true;
				}
			}
			else
			{
				for (int i = 0; i < _rigidBodies.Length; i++)
				{
					_rigidBodies[i].Sleeping = false;
					_rigidBodies[i].LinearVelocity = _lastImpulse[i];
					_lastImpulse[i] = Vector3.Zero;
					_rigidBodies[i].AngularVelocity = _lastTorque[i];
					_lastTorque[i] = Vector3.Zero;
				}
				AddForce(_forceAwake);
				_forceAwake = Vector3.Zero;
				AddTorque(_torqueAwake);
				_torqueAwake = Vector3.Zero;
			}
			_sleepPriority = isSleep ? priority : -1;
		}

		public void IgnoreCollisionWithRagdoll(SkeletonRagdoll skelRagdoll)
		{
			if (_hasIgnoredColliders || skelRagdoll._hasIgnoredColliders ||
				mirroringDuplicateIndex == skelRagdoll.mirroringDuplicateIndex ||
				mirroringDuplicateIndex == -1 || skelRagdoll.mirroringDuplicateIndex == -1)
				return;

			foreach (var c in _colliders)
			{
				foreach (var c2 in skelRagdoll._colliders)
				{
					PhysicsServer3D.BodyAddCollisionException(
						c.GetRigidBody()?.GetRid() ?? new Rid(),
						c2.GetRigidBody()?.GetRid() ?? new Rid());
				}
			}
			_hasIgnoredColliders = true;
		}
	}
}
