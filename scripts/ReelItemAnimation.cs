using Godot;
using System;
using System.Linq;

[Tool]
public class ReelItemAnimation : Control, IReelItemAnimation
{
    [Serializable]
    private class Animation
    {
        public string name = string.Empty;
        public Curve rotationCurve = Curve.Create(0f, 0f, 1f, 1f);
        public Curve positionCurve = Curve.Create(0f, 0f, 1f, 1f);
        public Curve scaleCurve = Curve.Create(0f, 0f, 1f, 1f);
        public float duration = 1f;
    }

    [Export]
    private Animation[] animations;

    private string currentAnimation;
    private ReelItem _item;
    private bool isPlay;

    public ReelItem item => _item ?? (_item = GetNode<ReelItem>("."));

    public event ReelItemAnimationEnd onAnimationEnd;

    private Animation GetAnimationByName(string animationName)
    {
        return !string.IsNullOrEmpty(animationName) ? animations.FirstOrDefault(t => animationName.Equals(t.name)) : null;
    }

    public void Animate(string animationName, Vector3 moveTo)
    {
        Animation animation = SetAnimation(animationName);
        if (animation != null)
        {
            var tween = CreateTween();
            tween.TweenProperty(this, "position", new Vector2(moveTo.X, moveTo.Y), animation.duration);
        }
    }

    public void Animate(string animationName, Vector3 moveTo, Vector3 rotateTo)
    {
        Animate(animationName, moveTo);
    }

    public void Animate(string animationName, Vector3 moveTo, Vector3 rotateTo, Vector3 scaleTo)
    {
        Animate(animationName, moveTo);
    }

    private Animation SetAnimation(string animationName)
    {
        Animation anim = GetAnimationByName(animationName);
        if (anim == null)
        {
            GD.PrintErr("Animation not found");
            return null;
        }
        Visible = true;
        currentAnimation = animationName;
        isPlay = true;
        return anim;
    }

    public void Stop()
    {
        isPlay = false;
    }

    public override void _Ready()
    {
        _item = GetNode<ReelItem>(".");
    }

    private void AnimationEnd()
    {
        string text = currentAnimation;
        currentAnimation = null;
        isPlay = false;
        onAnimationEnd?.Invoke(text, this);
    }
}
