using SF3;
using SF3.Audio;
using Godot;
public partial class DebugMenu : UIModuleHolder, ISceneInitializationObject
{
	private static DebugMenu _inctance;
	public Node scrollView;
	public string[] commands;
	public ModuleInfo[] debugModules;
	private bool show;
	private float savedTime;
	public Button showHideBtn;
	public Button hideButton;
	public Button backGroundMusic;
	public Button soundsBtn;
	private Label musicLbl;
	private Label soundsLbl;
	private Label changeEffectLbl;
	public Node menuObj;
	private bool _uiHidden;
	private bool _musicOn = true;
	private bool _soundsOn = true;
	public float secondsResume;
	private float _timeToResume;
	public static DebugMenu Inctance
	{
		get
		{
			return _inctance;
		}
	}
	public override void _Ready()
	{
		base.Awake();
		_inctance = this;
		showHideBtn.onClick.Add(new EventDelegate(ShowHide));
		hideButton.onClick.Add(new EventDelegate(ShowHideUI));
		backGroundMusic.onClick.Add(new EventDelegate(PauseResumeMusic));
		soundsBtn.onClick.Add(new EventDelegate(PauseResumeSounds));
		soundsLbl = soundsBtn.GetComponentInChildren<Label>();
		musicLbl = backGroundMusic.GetComponentInChildren<Label>();
		for (int i = 0; i < debugModules.Length; i++)
		{
			debugModules[i].Init();
		}
		string[] array = commands;
		foreach (string com in array)
		{
			Node Node = Object.GD.Instantiate(hideButton);
			Node.Node3D.parent = scrollView.Node3D;
			Node.GetComponent<Button>().onClick.Clear();
			Node.GetComponent<Button>().onClick.Add(new EventDelegate(delegate
			{
				NekkiConsole.ExecuteCommand(com);
			}));
			Node.GetComponentInChildren<Label>().text = com;
		}
		menuObj.SetActive(false);
	}
	public override void Initialize()
	{
		_musicOn = AudioManager.musicVolume > 0f;
		_soundsOn = AudioManager.volume > 0f;
		soundsLbl.text = ((!_soundsOn) ? "Resume " : "Pause ") + " SOUNDS";
		musicLbl.text = ((!_musicOn) ? "Resume " : "Pause ") + " MUSIC";
		if (!_soundsOn)
		{
			AudioManager.Instance.SetVolume(0f);
		}
		if (!_musicOn)
		{
			LocationAudioSettings.SetVolume(0f);
		}
	}
	public void DisposePreviousLocation()
	{
	}
	private void PauseResumeMusic()
	{
		_musicOn = !_musicOn;
		musicLbl.text = ((!_musicOn) ? "Resume " : "Pause ") + " MUSIC";
		AudioManager.Instance.SetMusicVolume((!_musicOn) ? 0f : 1f);
	}
	private void PauseResumeSounds()
	{
		_soundsOn = !_soundsOn;
		soundsLbl.text = ((!_soundsOn) ? "Resume " : "Pause ") + " SOUNDS";
		AudioManager.Instance.SetVolume((!_soundsOn) ? 0f : 1f, true);
	}
	private void ShowHideUI()
	{
		_uiHidden = !_uiHidden;
		UICamera3D.currentCamera3D.enabled = !_uiHidden;
	}
	public void ShowHide()
	{
		show = !show;
		menuObj.SetActive(show);
		if (show)
		{
			BattleController.PauseGame();
		}
		else
		{
			BattleController.ResumeGame();
		}
	}
	private void _Process(double delta)
	{
		if (_uiHidden)
		{
			if (Input.IsMouseButtonJustPressed(MouseButton.Left))
			{
				_timeToResume = Time.GetTicksMsec()/1000f + secondsResume;
			}
			if (Input.IsMouseButtonPressed(MouseButton.Left) && _timeToResume <= Time.GetTicksMsec()/1000f)
			{
				ShowHideUI();
			}
		}
	}
}

