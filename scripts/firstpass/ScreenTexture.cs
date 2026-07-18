using System;
using System.Collections.Generic;
using Godot;

public partial class ScreenTexture : Node
{
    public enum TextureOutputFilter { None = 0, Blur = 1 }
    public enum TextureOutputCamera { Both = 0, Main = 1, Ui = 2 }

    public Action OnScreenBlocked;
    public Action OnScreenUnblocked;
    private Action _onScreenTaken;

    public TextureRect _screenTexture;
    private bool _isInit;
    private TextureOutputFilter _currentTextureOutputFilter;
    private TextureOutputCamera _currentTextureOutputCamera;
    private float _screenFadeDuration = 0.5f;
    private List<string> _textureCallersNames = new List<string>();
    private List<string> _blockCallersNames = new List<string>();
    public bool MainCameraEnabled = true;
    public ImageTexture renderTexture;

    public static ScreenTexture Instance { get; private set; }

    public bool Active { get { return _screenTexture != null && _screenTexture.Visible; } }

    public override void _Ready()
    {
        Instance = this;
        MainCameraEnabled = true;
    }

    public void BlockScreen(string callersName) { }
    public void UnBlockScreen(string callersName, float duration = 0.5f, Action callback = null) { callback?.Invoke(); }
    public bool IsScreenBlocked() { return false; }

    public void SetTexture(string callersName, TextureOutputCamera tCamera, TextureOutputFilter tFilter, Action callback = null, int depth = 9999)
    {
        callback?.Invoke();
    }

    public void SetTexture(string callersName, TextureOutputCamera tCamera, Action callback = null)
    {
        SetTexture(callersName, tCamera, TextureOutputFilter.None, callback);
    }

    public void SetTexture(string callersName, Action callback = null)
    {
        SetTexture(callersName, TextureOutputCamera.Both, TextureOutputFilter.None, callback);
    }

    public void Clear(string callersName, float duration = 0.5f, Action callBack = null)
    {
        MainCameraEnabled = true;
        callBack?.Invoke();
    }

    public void AddOverlayPanel(Node overlayPanel, int priority = 1) { }
    public void AddOverlay(Node gameObject, int priority = 1) { }
    public void RemoveOverlayPanel(Node overlayPanel) { }
}
