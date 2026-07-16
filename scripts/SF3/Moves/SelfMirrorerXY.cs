using Godot;

namespace SF3.Moves
{
	public class SelfMirrorerXY : AnimationMirrorer
	{
		protected override void CalculateAndSet(MirrorIndexes index)
		{
			Vector3 boneStartRotation = GetBoneStartRotation(index.leftIndex);
			Vector3 animatedTransformRotation = GetAnimatedTransformRotation(index.leftIndex);
			animatedTransformRotation.X = boneStartRotation.X - (animatedTransformRotation.X - boneStartRotation.X);
			animatedTransformRotation.Y = boneStartRotation.Y - (animatedTransformRotation.Y - boneStartRotation.Y);
			SetAnimatedTransformsAnimateThisFrame(index.leftIndex, true);
			SetAnimatedTransformsRotation(index.leftIndex, animatedTransformRotation);
		}
	}
}
