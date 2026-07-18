using Godot;
using System;
using System.Collections.Generic;
using Nekki.Yaml;
using Node = Nekki.Yaml.Node;

public partial class LoadScreen : UIModuleHolder
{
    private class LoadScreenProcess
    {
        public bool Show { get; set; }
        public bool Hide { get; set; }

        public void Clear()
        {
            Show = false;
            Hide = false;
        }
    }

    private static LoadScreen _instance;
    private readonly List<string> _aliases = new List<string>();
    private int _tipIndex;
    [Export] private Label _tip;
    [Export] private TextureRect _loader;
    [Export] private float _defaultTweenDuration;
    [Export] private float _hideRoundEndDuration;
    private LoadScreenProcess _processLoader;
    private readonly List<Action> _onHideLoader = new List<Action>();

    public static LoadScreen Instance
    {
        get
        {
            if (_instance == null)
                NekkiUIRootModules.Instance.MountNativeModule("LoadScreen");
            return _instance;
        }
    }

    public static bool LoaderVisible => _instance != null && _instance._loader.Modulate.A > 0.001f;

    public static void Clear() { _instance = null; }

    public override void _Ready()
    {
        base._Ready();
        _instance = this;
        string loadTextInternal = GlobalLoad.GetLoadTextInternal("GameSettings", "lore_hint_config.yaml");
        Node root = YamlDocumentNekki.FromYamlContent(loadTextInternal).GetRoot("Lines");
        foreach (object item in root)
            _aliases.Add(item.ToString());
        _processLoader = new LoadScreenProcess();
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        OnDisableLoader();
        _instance = null;
    }

    public override void _Process(double delta)
    {
        if (!LoaderVisible && _onHideLoader.Count > 0)
            OnDisableLoader();
    }

    public static void ShowLoader(Action onDone = null, float showDelay = 0f, bool instantly = false)
    {
        Instance._loader.Visible = true;
        float duration = (!instantly) ? Instance._defaultTweenDuration : 0f;
        Instance.SetLoaderVisible(onDone, duration, showDelay);
    }

    public static void HideLoader(Action onDone)
    {
        HideLoader(onDone, Instance._hideRoundEndDuration);
    }

    private static void HideLoader(Action onDone = null, float duration = 0f, float delay = 0f)
    {
        Instance._processLoader.Hide = true;
        Instance._tip.Visible = false;
        Instance.TweenLoader(0f, duration, delay, onDone);
    }

    public void SheduleOnHideLoader(Action onHide)
    {
        _onHideLoader.Add(onHide);
    }

    public void UnsheduleOnHideLoader(Action onHide)
    {
        _onHideLoader.Remove(onHide);
    }

    private void OnDisableLoader()
    {
        _processLoader.Clear();
        foreach (Action item in _onHideLoader)
            item();
        _onHideLoader.Clear();
    }

    private void LoaderTweenFinished()
    {
        if (!LoaderVisible)
        {
            _processLoader.Hide = false;
            _loader.Visible = false;
        }
        else
        {
            _tip.Visible = true;
            _processLoader.Show = false;
        }
    }

    public void RefreshTip()
    {
        _tipIndex = (int)GD.RandRange(0, _aliases.Count);
        Instance._tip.Text = Localization.Get(_aliases[_tipIndex]);
    }

    public void ShowFightStart(Action eventToThrow)
    {
        if (LoaderVisible)
            HideLoader(eventToThrow.InvokeSafe, 0.25f);
        else
            eventToThrow.InvokeSafe();
    }

    public void HideRoundEnd(Action onDone = null)
    {
        onDone.InvokeSafe();
    }

    public bool LoaderIsReady() => !_processLoader.Hide && !_processLoader.Show;

    private void SetLoaderVisible(Action onDone = null, float duration = 0f, float delay = 0f)
    {
        _processLoader.Show = true;
        RefreshTip();
        TweenLoader(1f, duration, delay, onDone);
    }

    private async void TweenLoader(float value, float duration = 0f, float delay = 0f, Action onDone = null)
    {
        if (duration > 0f || delay > 0f)
        {
            if (delay > 0f)
                await ToSignal(GetTree().CreateTimer(delay), "timeout");
            float startAlpha = _loader.Modulate.A;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += (float)Engine.GetProcessDeltaTime();
                var c = _loader.Modulate;
                c.A = Mathf.Lerp(startAlpha, value, elapsed / duration);
                _loader.Modulate = c;
                await ToSignal(GetTree(), "process_frame");
            }
            var final = _loader.Modulate;
            final.A = value;
            _loader.Modulate = final;
            onDone.InvokeSafe();
            LoaderTweenFinished();
        }
        else
        {
            var c = _loader.Modulate;
            c.A = value;
            _loader.Modulate = c;
            onDone.InvokeSafe();
            LoaderTweenFinished();
        }
    }
}
