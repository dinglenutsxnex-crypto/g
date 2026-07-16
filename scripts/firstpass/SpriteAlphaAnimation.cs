using Godot;
using System.Collections.Generic;

public class SpriteAlphaAnimation : Node
{
    public enum EAnimationStyle { LOOP = 0, PING_PONG = 1, ONCE = 2 }

    [Export]
    private float _animationTime = 8f;

    [Export]
    private Curve _animationCurve;

    [Export]
    private EAnimationStyle _animationStyle;

    [Export]
    private bool _activateOnStart = true;

    private float _timer;
    private float _alphaValue;
    private bool _backAnimation;
    private bool _active;
    private float _currentAnimationTime;

    private List<CanvasItem> _targets = new List<CanvasItem>();

    public override void _Ready()
    {
        _timer = 0f;
        _backAnimation = false;

        foreach (Node child in GetChildren())
        {
            if (child is CanvasItem ci)
                _targets.Add(ci);
        }
        var parent = GetParent() as CanvasItem;
        if (parent != null)
            _targets.Add(parent);

        if (_activateOnStart)
            Activate(_animationTime);
    }

    public void Disable(float byTimeSeconds)
    {
        _timer = 0f;
        _active = false;
    }

    public void Disable(bool instantly = false)
    {
        Disable(instantly ? 0f : _animationTime);
    }

    public void Activate(EAnimationStyle newAnimationStyle, float timeSeconds)
    {
        _currentAnimationStyle = newAnimationStyle;
        _timer = 0f;
        _active = true;
        _currentAnimationTime = timeSeconds;
    }

    public void Activate(float timeSeconds)
    {
        Activate(_animationStyle, timeSeconds);
    }

    public void Activate()
    {
        Activate(_animationStyle, _animationTime);
    }

    public override void _Process(double delta)
    {
        if (!_active) return;

        _timer += delta;
        float t = _timer / _currentAnimationTime;
        if (_animationCurve != null)
            _alphaValue = _animationCurve.Interpolate(!_backAnimation ? t : 1f - t);
        else
            _alphaValue = !_backAnimation ? t : 1f - t;

        UpdateAlpha(_alphaValue);

        if (_timer >= _currentAnimationTime)
        {
            if (_currentAnimationStyle == EAnimationStyle.PING_PONG)
                _backAnimation = !_backAnimation;
            else if (_currentAnimationStyle == EAnimationStyle.LOOP)
                _backAnimation = false;
            else if (_currentAnimationStyle == EAnimationStyle.ONCE)
            {
                _backAnimation = false;
                _active = false;
            }
            _timer = 0f;
        }
    }

    private void UpdateAlpha(float value)
    {
        foreach (CanvasItem target in _targets)
        {
            Color c = target.Modulate;
            c.a = value;
            target.Modulate = c;
        }
    }
}
