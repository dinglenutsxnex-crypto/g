using Godot;
using System;

public class passAnimationFrameToShader : Node3D
{
	public override void _Process(double delta)
	{
		AnimationPlayer component = GetNode<AnimationPlayer>();
		float normalizedTime = component["Scene"].As<float>();
		float num = 1f;
		float value = 1f;
		if (normalizedTime <= 0.2f)
		{
			num = normalizedTime / 0.18f;
		}
		else if (normalizedTime > 0.2f && normalizedTime <= 0.53f)
		{
			num = 1f;
		}
		else if (normalizedTime > 0.53f && normalizedTime <= 0.75f)
		{
			num = 1f - (normalizedTime - 0.53f) / 0.22000003f;
			value = num;
		}
		else
		{
			num = 0f;
			value = 0f;
		}
		GD.Print(num);
		MeshInstance3D[] componentsInChildren = GetComponentsInChildren<MeshInstance3D>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].MaterialOverride.Set("_AnimState", num);
			componentsInChildren[i].MaterialOverride.Set("_Transparency", value);
		}
	}
}
