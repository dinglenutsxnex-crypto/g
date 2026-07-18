using Godot;
using System.Collections.Generic;

public partial class MultiTweenTransition : Node
{
    public Node[] Tweens;
    private bool _initDone;
    public bool Automatic = true;

    internal void Init()
    {
        if (Automatic)
        {
            List<Node> list = new List<Node>();
            foreach (Node child in GetChildren())
            {
                if (child is Tween)
                    list.Add(child);
            }
            Tweens = list.ToArray();
        }
        _initDone = true;
    }

    public void TweenOut()
    {
        if (!_initDone) Init();
        foreach (Node t in Tweens)
        {
            if (t is Tween tween)
                tween.Play();
        }
    }

    public void TweenIn()
    {
        if (!_initDone) Init();
        foreach (Node t in Tweens)
        {
            if (t is Tween tween)
                tween.Play();
        }
    }
}
