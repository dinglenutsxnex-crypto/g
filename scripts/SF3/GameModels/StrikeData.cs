using System;
using SF3.Moves;
using Godot;

namespace SF3.GameModels
{
	[Serializable]
	public partial class StrikeData
	{
		public Capsule collisionEdge;
		public Capsule attackEdge;
		public Vector3 strikePoint;
		public Vector3 strikeEffectPoint;
		public Model attackingModel;
		public int direction;
		public IntervalAttack intervalAttack;
		public int criticalHit;
		public string attackingEdgeMat;
		public string collisionEdgeMat;

		public StrikeData()
		{
		}

		public StrikeData(IntervalAttack interval)
			: this()
		{
			intervalAttack = interval;
		}

		public void Reset()
		{
			attackEdge = null;
			collisionEdge = null;
			attackingEdgeMat = string.Empty;
			collisionEdgeMat = string.Empty;
			strikePoint = Vector3.Zero;
			strikeEffectPoint = Vector3.Zero;
		}

		public void GetMaterials(Model attacking)
		{
			if (attackEdge != null && collisionEdge != null)
			{
				attackingEdgeMat = attacking.modelComponents.GetModelMaterialAtPointInCapsule(attackEdge.name, strikePoint);
				collisionEdgeMat = attacking.enemy.modelComponents.GetModelMaterialAtPointInCapsule(collisionEdge.name, strikePoint);
			}
		}

		public Vector2 GetCapsuleMovingDirection()
		{
			if (attackEdge == null)
			{
				return Vector2.Zero;
			}
			Vector3 normalized = (attackEdge.startVertex.position - attackEdge.startVertex.previousPosition).Normalized();
			Vector3 normalized2 = (attackEdge.finishVertex.position - attackEdge.finishVertex.previousPosition).Normalized();
			return (Vector2)((normalized + normalized2) / 2f);
		}
	}
}
