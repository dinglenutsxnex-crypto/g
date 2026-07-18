using System.Text;
using Godot;
using SF3;
using SF3.Audio;
using SF3.UserData;

public partial class PauseWindow : UIModuleHolder
{
	public delegate void PauseEnabledEventHandler();

	public delegate void PauseDisabledEventHandler();

	private const float DISABLE_ALPHA = 0.33f;

	[Export]
	private Label _rulesLabel;

	[Export]
	private Button _resumeBtn;

	[Export]
	private Button _surrenderBtn;

	[Export]
	private Button _soundBtn;

	[Export]
	private Button _musicBtn;

	private readonly string[] _hideModules = new string[8] { "Joystick", "FightButtons", "PauseButton", "PlayerHUD", "FoeHUD", "RoundTimer", "RoundsUI", "ShadowPerks" };

	private static PauseWindow _instance;

	private static int _showCounter;

	private static NekkiUIModule _pauseButton;

	private static NekkiUIModule _roundTimer;

	private static TutorialComponent _pauseButtonTutorialComponent;

	private static TutorialComponent _roundTimerTutorialComponent;

	public static event PauseEnabledEventHandler OnPauseEnabled;

	public static event PauseDisabledEventHandler OnPauseDisabled;

	public static void ResetShowCounter()
	{
		_showCounter = 0;
		RefreshDependentOnShowCounter();
	}

	public static void IncrementShowCounter()
	{
		AddShowCounter(1);
	}

	public static void DecrementShowCounter()
	{
		AddShowCounter(-1);
	}

	private static void AddShowCounter(int delta)
	{
		_showCounter += delta;
		RefreshDependentOnShowCounter();
	}

	private static void RefreshDependentOnShowCounter()
	{
		if (!(_instance == null))
		{
			if (_pauseButton == null)
			{
				_pauseButton = NekkiUIRootModules.Instance.MountNativeModule("PauseButton");
			}
			if (_roundTimer == null)
			{
				_roundTimer = NekkiUIRootModules.Instance.MountNativeModule("RoundTimer");
			}
			if (_pauseButtonTutorialComponent == null)
			{
				_pauseButtonTutorialComponent = _pauseButton.GetNode<TutorialComponent>(new NodePath("TutorialComponent"));
			}
			if (_roundTimerTutorialComponent == null)
			{
				_roundTimerTutorialComponent = _roundTimer.GetNode<TutorialComponent>(new NodePath("TutorialComponent"));
			}
			_pauseButton.Visible = _pauseButtonTutorialComponent.GetVisible() && _showCounter > 0;
			_roundTimer.Visible = _pauseButtonTutorialComponent.GetVisible() && _showCounter > 0;
		}
	}

	public bool IsNative(string moduleName)
	{
		switch (moduleName)
		{
		case "Joystick":
			return false;
		case "FightButtons":
			return false;
		case "PlayerHUD":
			return false;
		case "FoeHUD":
			return false;
		case "RoundsUI":
			return false;
		case "PauseButton":
			return true;
		case "RoundTimer":
			return true;
		case "ShadowPerks":
			return true;
		default:
			return false;
		}
	}

	public override void _Ready()
	{
		base._Ready();
		_resumeBtn.Pressed += OnResumeClick;
		if (UserManager.GetGlobalVariable("TUTORIAL") == "1")
		{
			_surrenderBtn.Visible = false;
		}
		else
		{
			_surrenderBtn.Pressed += OnSurrenderClick;
		}
		_soundBtn.Pressed += OnSoundClick;
		_musicBtn.Pressed += OnMusicClick;
		UpdateBtn(_soundBtn, AudioManager.volume);
		UpdateBtn(_musicBtn, AudioManager.musicVolume);
		Visible = false;
		_instance = this;
		ResetShowCounter();
	}

	private void OnSurrenderClick()
	{
		BattleController.Instance.fightController.SetFightResult(2, true);
		Visible = false;
	}

	private void OnResumeClick()
	{
		Pause(false);
	}

	private void OnSoundClick()
	{
		AudioManager.Instance.SetVolume((!(AudioManager.volume > 0f)) ? 1 : 0);
		UpdateBtn(_soundBtn, AudioManager.volume);
	}

	private void OnMusicClick()
	{
		AudioManager.Instance.SetMusicVolume((!(AudioManager.musicVolume > 0f)) ? 1 : 0);
		UpdateBtn(_musicBtn, AudioManager.musicVolume);
	}

	public static void Pause()
	{
		if (!(_instance == null))
		{
			_instance.Pause(true);
		}
	}

	public static void UnPause()
	{
		if (!(_instance == null))
		{
			_instance.Pause(false);
		}
	}

	public void Pause(bool isPause)
	{
		Visible = isPause;
		if (isPause)
		{
			BattleController.SystemPause();
			_rulesLabel.Text = BuildRulesString();
			if (PauseWindow.OnPauseEnabled != null)
			{
				PauseWindow.OnPauseEnabled();
			}
		}
		else
		{
			BattleController.SystemResume();
			if (PauseWindow.OnPauseDisabled != null)
			{
				PauseWindow.OnPauseDisabled();
			}
		}
		ShowModules(!isPause);
	}

	private string BuildRulesString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (Rule allRule in ModelsManager.Instance.GetAllRules())
		{
			string attributeByType = allRule.GetAttributeByType("Description");
			if (!string.IsNullOrEmpty(attributeByType))
			{
				string @string = Localization.Get(attributeByType).String;
				if (!string.IsNullOrEmpty(@string))
				{
					stringBuilder.AppendLine(@string);
				}
			}
		}
		if (stringBuilder.Length > 0)
		{
			stringBuilder.Length--;
		}
		return stringBuilder.ToString();
	}

	private void ShowModules(bool show)
	{
		string[] hideModules = _hideModules;
		foreach (string moduleName in hideModules)
		{
			NekkiUIModule nekkiUIModule = ((!IsNative(moduleName)) ? NekkiUIRootModules.Instance.MountNGUIModule(moduleName) : NekkiUIRootModules.Instance.MountNativeModule(moduleName));
			TutorialComponent component = nekkiUIModule.GetNode<TutorialComponent>(new NodePath("TutorialComponent"));
			if (component != null)
			{
				nekkiUIModule.Visible = show && component.GetVisible();
			}
			else
			{
				nekkiUIModule.Visible = show;
			}
		}
	}

	public override void _Process(double delta)
	{
		if (Input.IsKeyPressed(Key.Escape))
		{
			Pause(!GameTimeController.gamePaused);
		}
	}

	private void UpdateBtn(Button button, float volume)
	{
		Color normalColor = new Color(1, 1, 1, volume > 0f ? 1f : 0.33f);
		button.Modulate = normalColor;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		ResetShowCounter();
		_instance = null;
	}
}
