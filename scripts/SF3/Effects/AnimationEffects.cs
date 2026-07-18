using System;
using System.Collections.Generic;
using Godot;
using SF3.GameModels;

namespace SF3.Effects
{
	public partial class AnimationEffects : Node
	{
		private Dictionary<string, Node> _effectCache = new Dictionary<string, Node>();

		[Export]
		private Node _hitEffectPrefab;

		[Export]
		private Node _sparkEffectPrefab;

		[Export]
		private Node _dustEffectPrefab;

		public void PlayHitEffect(Vector3 position, Node3D target = null)
		{
			if (_hitEffectPrefab != null)
			{
				Node effect = _hitEffectPrefab.Duplicate();
				AddChild(effect);
				if (effect is Node3D n3d)
				{
					n3d.Position = position;
				}
			}
		}

		public void PlaySparkEffect(Vector3 position)
		{
			if (_sparkEffectPrefab != null)
			{
				Node effect = _sparkEffectPrefab.Duplicate();
				AddChild(effect);
				if (effect is Node3D n3d)
				{
					n3d.Position = position;
				}
			}
		}

		public void PlayDustEffect(Vector3 position)
		{
			if (_dustEffectPrefab != null)
			{
				Node effect = _dustEffectPrefab.Duplicate();
				AddChild(effect);
				if (effect is Node3D n3d)
				{
					n3d.Position = position;
				}
			}
		}

		public void ClearEffects()
		{
			foreach (var child in GetChildren())
			{
				if (child is Node childNode)
				{
					if (childNode.Name.ToString().StartsWith("Effect_"))
					{
						childNode.QueueFree();
					}
				}
			}
			_effectCache.Clear();
		}

		public void PlayEffectByName(string effectName, Vector3 position)
		{
			GD.Print("AnimationEffects.PlayEffectByName: " + effectName);
		}
	}
}
