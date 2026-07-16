using System;
using System.Text;
using Godot;

namespace SF3.Moves
{
    [Serializable]
    public class AnimatedTransform
    {
        public static readonly AnimatedTransform IDENTITY;

        public bool animateThisFrame;

        public Quaternion rotation { get; private set; }
        public Vector3 position { get; private set; }

        static AnimatedTransform()
        {
            IDENTITY = new AnimatedTransform(Vector3.Zero, Quaternion.Identity);
        }

        public AnimatedTransform(Vector3 newPosition, Quaternion newRotation)
        {
            position = newPosition;
            rotation = newRotation;
            animateThisFrame = false;
        }

        public AnimatedTransform()
            : this(Vector3.Zero, Quaternion.Identity)
        {
        }

        public void SetRotation(Quaternion newRotation)
        {
            rotation = newRotation;
        }

        public void SetPosition(Vector3 newPosition)
        {
            position = newPosition;
        }

        public void AddPosition(Vector3 addToPos)
        {
            position += addToPos;
        }

        public void AddRotation(Quaternion addRotation)
        {
            rotation *= addRotation;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Position: ");
            sb.Append(position);
            sb.Append("; Rotation: ");
            sb.Append(rotation.GetEuler());
            return sb.ToString();
        }

        public static void CopyBoneTransform(AnimatedTransform from, AnimatedTransform to)
        {
            to.SetPosition(from.position);
            to.SetRotation(from.rotation);
        }
    }
}
