using Godot;
using System;
using System.Collections.Generic;

[Tool]
public partial class Tutorial : Node2D
{
    [Serializable]
    public partial class ColorSync
    {
        private enum SyncDirrections { Forward, Backward }
        private Color _currentColor;
        [Export] public Color From;
        [Export] public Color To;
        [Export] public Curve Curve;
        [Export] public float Duration;
        private float _currentTime;
        private SyncDirrections _dirrection;
        private List<TextureRect> _sprites = new List<TextureRect>();

        public void Update()
        {
            if (_sprites.Count == 0) return;
            _currentTime += Engine.TimeScale * (float)((_dirrection == SyncDirrections.Forward) ? 1 : (-1));
            if (_currentTime > Duration) { _currentTime = Duration; _dirrection = SyncDirrections.Backward; }
            if (_currentTime < 0f) { _currentTime = 0f; _dirrection = SyncDirrections.Forward; }
            _currentColor = From.Lerp(To, Curve.Sample(_currentTime / Duration));
            foreach (var sprite in _sprites)
                sprite.SelfModulate = _currentColor;
        }

        public void Add(List<TextureRect> uiSprites)
        {
            foreach (var s in uiSprites)
            {
                if (!_sprites.Contains(s))
                    _sprites.Add(s);
            }
        }
    }

    public ColorSync Sync;
    private static int _activeCount;

    public static Tutorial Instance { get; private set; }
    public static bool Active => _activeCount > 0;

    public static void SetTutor(EQuadrants[] quadrants, bool active)
    {
        Instance.Sync.Add(ActionButtons.Instance.SetTutor(quadrants, active));
        Stick.Instance.SetTutor(quadrants, active);
        if (active)
            _activeCount += quadrants.Length;
        else
            _activeCount = (_activeCount > quadrants.Length) ? (_activeCount - quadrants.Length) : 0;
    }

    public static void Reset()
    {
        if (ActionButtons.Instance != null && Stick.Instance != null)
        {
            ActionButtons.Instance.SetTutor(null);
            Stick.Instance.SetTutor(null);
            _activeCount = 0;
        }
    }

    public override void _Ready()
    {
        Instance = this;
    }

    public override void _Process(double delta)
    {
        if (Active && Sync != null)
            Sync.Update();
    }
}
