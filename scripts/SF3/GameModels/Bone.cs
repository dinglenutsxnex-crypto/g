using System;
using System.Collections.Generic;
using Godot;

namespace SF3.GameModels
{
    public class Bone
    {
        private bool _isCollising;

        private Vector3 _lastPosition;

        private Node3D _transform;

        public bool animatedThisFrame { get; private set; }

        public Node3D transform
        {
            get
            {
                return _transform;
            }
        }

        public Vector3 lossyScale
        {
            get
            {
                return _transform.Scale;
            }
        }

        public Vector3 localScale
        {
            get
            {
                return _transform.Scale;
            }
        }

        public Vector3 localPosition
        {
            get
            {
                return _transform.Position;
            }
        }

        public Vector3 position
        {
            get
            {
                return _transform.GlobalPosition;
            }
        }

        public Quaternion rotation
        {
            get
            {
                return _transform.Quaternion;
            }
        }

        public Quaternion localRotation
        {
            get
            {
                return _transform.Quaternion;
            }
        }

        public Vector3 previousPosition { get; private set; }

        public Vector3 previousLocalPosition { get; private set; }

        public Vector3 startPosition { get; private set; }

        public Vector3 startRotation { get; private set; }

        public float weight { get; private set; }

        public int boneID { get; private set; }

        public string boneName
        {
            get
            {
                return _transform.Name;
            }
        }

        public Bone mirrorBone { get; private set; }

        public Bone parentBone { get; private set; }

        public List<Bone> childBones { get; private set; }

        public bool pseudoPhysics { get; private set; }

        public RigidBody3D rigidBody { get; private set; }

        public event Action onPreviousPositionUpdate;

        public Bone(Node3D transform, int newBoneID = -1, Bone newParentBone = null)
        {
            _transform = transform;
            childBones = new List<Bone>();
            startRotation = transform.Rotation;
            startPosition = transform.Position;
            boneID = newBoneID;
            parentBone = newParentBone;
            animatedThisFrame = false;
            weight = 0f;
            pseudoPhysics = false;
            UpdatePreviousPosition();
        }

        public void SetPseudoPhysics(bool value)
        {
            rigidBody = _transform as RigidBody3D;
            if (rigidBody == null)
            {
                rigidBody = _transform.GetNodeOrNull<RigidBody3D>(".");
            }
            pseudoPhysics = value;
        }

        public void SetWeight(float newWeight)
        {
            weight = newWeight;
        }

        public void SetBoneID(int newID)
        {
            boneID = newID;
        }

        public void Rotate(Vector3 angles)
        {
            Vector3 rotationAngles = _transform.Rotation;
            rotationAngles += angles;
            _transform.Rotation = angles;
        }

        public void SetPosition(Vector3 position, bool isPrevious = true)
        {
            if (isPrevious)
            {
                UpdatePreviousPosition();
            }
            _transform.GlobalPosition = position;
        }

        public void SetRotation(Quaternion value)
        {
            _transform.Quaternion = value;
        }

        public void SetLocalPosition(Vector3 position)
        {
            _transform.Position = position;
            animatedThisFrame = true;
        }

        public void SetLocalRotation(Vector3 vector)
        {
            _transform.Rotation = vector;
        }

        public void SetLocalRotation(Quaternion rotation)
        {
            _transform.Quaternion = rotation;
        }

        public void UpdatePreviousPosition()
        {
            if (this.onPreviousPositionUpdate != null)
            {
                this.onPreviousPositionUpdate();
            }
            previousPosition = _transform.GlobalPosition;
            previousLocalPosition = _transform.Position;
        }

        public void SetPreviousPosition(Vector3 position)
        {
            previousPosition = position;
        }

        public void ShiftPosition(Vector3 vector)
        {
            SetPosition(position + vector);
        }

        public void SetShiftRotation(Vector3 vector)
        {
        }

        public void ResetAnimated()
        {
            animatedThisFrame = false;
        }

        public void ResetPosition()
        {
            SetLocalPosition(startPosition);
            SetLocalRotation(startRotation);
        }
    }
}
