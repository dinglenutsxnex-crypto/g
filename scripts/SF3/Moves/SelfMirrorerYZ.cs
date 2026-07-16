using Godot;

namespace SF3.Moves
{
	public class SelfMirrorerYZ : AnimationMirrorer
	{
		protected override void CalculateAndSet(MirrorIndexes index)
		{
			Vector3 boneStartRotation = GetBoneStartRotation(index.leftIndex);
			Vector3 animatedTransformRotation = GetAnimatedTransformRotation(index.leftIndex);
			animatedTransformRotation.Y = boneStartRotation.Y - (animatedTransformRotation.Y - boneStartRotation.Y);
			animatedTransformRotation.Z = boneStartRotation.Z - (animatedTransformRotation.Z - boneStartRotation.Z);
			SetAnimatedTransformsAnimateThisFrame(index.leftIndex, true);
			SetAnimatedTransformsRotation(index.leftIndex, animatedTransformRotation);
		}
	}
}
