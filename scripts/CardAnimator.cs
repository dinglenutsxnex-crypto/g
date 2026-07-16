using System;
using System.Collections.Generic;
using Godot;

public class CardAnimator : Node
{
	public Action onAnimationEnd;

	[Export]
	public bool enableOutlineEffect;

	[Export]
	public Texture2D reelBgTexture;

	[Export]
	public Texture2D reelFlowBgTexture;

	[Export]
	public Node reelAnimationPrefab;

	[Export]
	public Node reelAnimationContainer;

	[Export]
	public Node cardContainer;

	[Export]
	public Node cardAnimationContainer;

	public void Init(object rewardDataProvider, List<object> reelItems, object rewardInfo)
	{
		GD.Print("STUB: CardAnimator.Init");
	}

	public void InitCardAnimation()
	{
		GD.Print("STUB: CardAnimator.InitCardAnimation");
	}

	public void Animate()
	{
		GD.Print("STUB: CardAnimator.Animate");
	}

	public void AnimateSelectCard(object component)
	{
		GD.Print("STUB: CardAnimator.AnimateSelectCard");
	}

	public void Break()
	{
		GD.Print("STUB: CardAnimator.Break");
	}
}
