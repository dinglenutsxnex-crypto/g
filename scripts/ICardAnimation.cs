using System.Collections.Generic;
public interface ICardAnimationPlayer
{
	event CardAnimationEnd onAnimationEnd;
	void Init(RewardDataProvider rewardDataProvider, List<IReelItemAnimationPlayer> reelItemAnimations, RewardInfo rewardInfo);
	void Animate();
	void FadeIn();
	void Break();
	void AnimateSelectCard(ReelItem item);
}

