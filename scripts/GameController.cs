using System.Linq;
using System.Threading.Tasks;
using Godot;

public partial class GameController : ExtentionBehaviour
{
	private bool _punchEnabled;
	private bool _kickEnabled;
	private bool _stickEnabled = true;
	private bool _isStartController;

	public const int onControlPressed = 0;
	public const int onControlReleased = 1;

	private static GameController _instance;
	private bool _quadrantsInverted;

	public static GameController Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new GameController();
				_instance.InitController(true, true, true);
			}
			return _instance;
		}
	}

	private bool ActiveController
	{
		get
		{
			return _isStartController;
		}
		set
		{
			_isStartController = value;
			Stick.Instance.Visible = value;
			KeyboardController.Instance.Visible = value;
		}
	}

	public void InitController(bool punchEnabled, bool kickEnabled, bool joystickEnabled)
	{
		_punchEnabled = punchEnabled;
		_kickEnabled = kickEnabled;
		if (!AssemblyController.market.isSteam)
		{
			AddActionButtons(_punchEnabled, _kickEnabled);
			AddNavigator();
		}
		AddKeyboard();
		AddGamepad();
		_quadrantsInverted = false;
	}

	private void AddGamepad()
	{
		if (Input.GetConnectedJoypads().ToList().Exists((string name) => name != null))
		{
			GamepadController.Instance.removeEventListener(0, OnControlQuadrantPress);
			GamepadController.Instance.removeEventListener(1, OnControlQuadrantRelease);
			GamepadController.Instance.addEventListener(0, OnControlQuadrantPress);
			GamepadController.Instance.addEventListener(1, OnControlQuadrantRelease);
			GamepadController.Instance.AddTrakedKey(Key.JoypadButton8, EQuadrants.Zero);
			GamepadController.Instance.AddTrakedKey(Key.Joypad1Button0, EQuadrants.Kick, 1);
			GamepadController.Instance.AddTrakedKey(Key.Joypad1Button1, EQuadrants.Punch, 1);
			GamepadController.Instance.AddTrakedKey(Key.Joypad1Button2, EQuadrants.Magic, 1);
			GamepadController.Instance.AddTrakedKey(Key.Joypad1Button3, EQuadrants.Missile, 1);
			GamepadController.Instance.AddTrakedKey(Key.Joypad2Button0, EQuadrants.Kick, 2);
			GamepadController.Instance.AddTrakedKey(Key.Joypad2Button1, EQuadrants.Punch, 2);
			GamepadController.Instance.AddTrakedKey(Key.Joypad2Button2, EQuadrants.Magic, 2);
			GamepadController.Instance.AddTrakedKey(Key.Joypad2Button3, EQuadrants.Missile, 2);
		}
	}

	public void startController()
	{
		ActiveController = true;
	}

	private void StopController()
	{
		ActiveController = false;
	}

	private async void AddActionButtons(bool punchEnabled, bool kickEnabled)
	{
		await AddActionButtonsRoutine(punchEnabled, kickEnabled);
	}

	private async Task AddActionButtonsRoutine(bool punchEnabled, bool kickEnabled)
	{
		int trys = 0;
		while (ActionButtons.Instance == null)
		{
			await ToSignal(Engine.GetMainLoop(), "process_frame");
			if (trys++ > 20)
			{
				return;
			}
		}
		if (ActionButtons.Instance != null)
		{
			ActionButtons.Instance.removeEventListener(0, OnControlQuadrantPress);
			ActionButtons.Instance.removeEventListener(1, OnControlQuadrantRelease);
			ActionButtons.Instance.addEventListener(0, OnControlQuadrantPress);
			ActionButtons.Instance.addEventListener(1, OnControlQuadrantRelease);
		}
		else
		{
			GD.PushWarning("ActionButtons has no instance");
		}
	}

	private async void AddKeyboard()
	{
		await AddKeysboardRoutine();
	}

	private async Task AddKeysboardRoutine()
	{
		while (KeyboardController.Instance == null)
		{
			await ToSignal(Engine.GetMainLoop(), "process_frame");
		}
		KeyboardController.Instance.removeEventListener(0, OnControlQuadrantPress);
		KeyboardController.Instance.removeEventListener(1, OnControlQuadrantRelease);
		KeyboardController.Instance.addEventListener(0, OnControlQuadrantPress);
		KeyboardController.Instance.addEventListener(1, OnControlQuadrantRelease);
		KeyboardController.Instance.AddTrakedKey(Key.Key0, EQuadrants.Zero);
		AddKeysModels();
		KeyboardController.Instance.AddTrakedKey(Key.Right, EQuadrants.NextFrameButton);
		KeyboardController.Instance.AddTrakedKey(Key.Key1, EQuadrants.WinRoundButton);
		KeyboardController.Instance.AddTrakedKey(Key.Key2, EQuadrants.WinFightButton);
		KeyboardController.Instance.AddTrakedKey(Key.Key3, EQuadrants.ResetRoundButton);
		KeyboardController.Instance.AddTrakedKey(Key.Key4, EQuadrants.ResetFightButton);
		KeyboardController.Instance.AddTrakedKey(Key.Key5, EQuadrants.LossRoundButton);
		KeyboardController.Instance.AddTrakedKey(Key.Key6, EQuadrants.LossFightButton);
		KeyboardController.Instance.AddTrakedKey(Key.F1, EQuadrants.SlowModeKey);
		KeyboardController.Instance.AddTrakedKey(Key.F3, EQuadrants.ShowEdgesButton);
		KeyboardController.Instance.AddTrakedKey(Key.F4, EQuadrants.PauseButton);
		KeyboardController.Instance.AddTrakedKey(Key.F5, EQuadrants.ShowDebugPerksButton);
		KeyboardController.Instance.AddTrakedKey(Key.F10, EQuadrants.FullscreenMode);
		KeyboardController.Instance.AddTrakedKey(Key.F11, EQuadrants.EnableMinScale);
		KeyboardController.Instance.AddTrakedKey(Key.F12, EQuadrants.TestTactic);
		KeyboardController.Instance.AddTrakedKey(Key.M, EQuadrants.SoundMuteButton);
		KeyboardController.Instance.AddTrakedKey(Key.B, EQuadrants.StartBenchmarkKey);
		KeyboardController.Instance.AddTrakedKey(Key.U, EQuadrants.StartSuper);
	}

	private async void AddKeysModels()
	{
		await addKeysModelsRoutine();
	}

	private async Task addKeysModelsRoutine()
	{
		while (KeyboardController.Instance == null)
		{
			await ToSignal(Engine.GetMainLoop(), "process_frame");
		}
		if (!AssemblyController.market.isSteam)
		{
			KeyboardController.Instance.AddTrakedKey(Key.W, EQuadrants.One, 1);
			KeyboardController.Instance.AddTrakedKey(Key.E, EQuadrants.Two, 1);
			KeyboardController.Instance.AddTrakedKey(Key.D, EQuadrants.Three, 1);
			KeyboardController.Instance.AddTrakedKey(Key.C, EQuadrants.Four, 1);
			KeyboardController.Instance.AddTrakedKey(Key.X, EQuadrants.Five, 1);
			KeyboardController.Instance.AddTrakedKey(Key.Z, EQuadrants.Six, 1);
			KeyboardController.Instance.AddTrakedKey(Key.A, EQuadrants.Seven, 1);
			KeyboardController.Instance.AddTrakedKey(Key.Q, EQuadrants.Eight, 1);
			KeyboardController.Instance.AddTrakedKey(Key.Kp8, EQuadrants.One, 2);
			KeyboardController.Instance.AddTrakedKey(Key.Kp9, EQuadrants.Two, 2);
			KeyboardController.Instance.AddTrakedKey(Key.Kp6, EQuadrants.Three, 2);
			KeyboardController.Instance.AddTrakedKey(Key.Kp3, EQuadrants.Four, 2);
			KeyboardController.Instance.AddTrakedKey(Key.Kp2, EQuadrants.Five, 2);
			KeyboardController.Instance.AddTrakedKey(Key.Kp1, EQuadrants.Six, 2);
			KeyboardController.Instance.AddTrakedKey(Key.Kp4, EQuadrants.Seven, 2);
			KeyboardController.Instance.AddTrakedKey(Key.Kp7, EQuadrants.Eight, 2);
		}
		KeyboardController.Instance.AddTrakedKey(Key.O, EQuadrants.Punch, 1);
		KeyboardController.Instance.AddTrakedKey(Key.P, EQuadrants.Kick, 1);
		KeyboardController.Instance.AddTrakedKey(Key.K, EQuadrants.Missile, 1);
		KeyboardController.Instance.AddTrakedKey(Key.L, EQuadrants.Magic, 1);
		KeyboardController.Instance.AddTrakedKey(Key.Insert, EQuadrants.Punch, 2);
		KeyboardController.Instance.AddTrakedKey(Key.Delete, EQuadrants.Kick, 2);
		KeyboardController.Instance.AddTrakedKey(Key.PageDown, EQuadrants.Missile, 2);
		KeyboardController.Instance.AddTrakedKey(Key.PageUp, EQuadrants.Magic, 2);
		KeyboardController.Instance.AddTrakedKey(Key.T, EQuadrants.TEST1);
		KeyboardController.Instance.AddTrakedKey(Key.Y, EQuadrants.TEST2);
	}

	private async void AddNavigator()
	{
		await AddNavigatorRoutine();
	}

	private async Task AddNavigatorRoutine()
	{
		while (Stick.Instance == null)
		{
			await ToSignal(Engine.GetMainLoop(), "process_frame");
		}
		Stick.Instance.addEventListener(0, OnControlQuadrantPress);
		Stick.Instance.addEventListener(1, OnControlQuadrantPress);
		Stick.Instance.addEventListener(2, OnControlQuadrantRelease);
	}

	private void OnControlQuadrantPress(CallEventArgs args)
	{
		callQuadrant(0, args);
	}

	private void OnControlQuadrantRelease(CallEventArgs args)
	{
		callQuadrant(1, args);
	}

	private void callQuadrant(int type, CallEventArgs args)
	{
		AbstractController.Key key = (AbstractController.Key)args.Content;
		EQuadrants eQuadrants = key.Quadrant;
		int modelID = key.ModelID;
		if (_quadrantsInverted)
		{
			switch (eQuadrants)
			{
			case EQuadrants.One:
				eQuadrants = EQuadrants.Five;
				break;
			case EQuadrants.Two:
				eQuadrants = EQuadrants.Six;
				break;
			case EQuadrants.Three:
				eQuadrants = EQuadrants.Seven;
				break;
			case EQuadrants.Four:
				eQuadrants = EQuadrants.Eight;
				break;
			case EQuadrants.Five:
				eQuadrants = EQuadrants.One;
				break;
			case EQuadrants.Six:
				eQuadrants = EQuadrants.Two;
				break;
			case EQuadrants.Seven:
				eQuadrants = EQuadrants.Three;
				break;
			case EQuadrants.Eight:
				eQuadrants = EQuadrants.Four;
				break;
			}
		}
		if (IsQuadrantEnabled(eQuadrants))
		{
			callEvent(type, new int[2] { modelID, (int)eQuadrants });
		}
	}

	public void InvertQuadrants(bool useInvertQuadrants)
	{
		_quadrantsInverted = useInvertQuadrants;
	}

	private bool IsQuadrantEnabled(EQuadrants key)
	{
		if (!_punchEnabled && key == EQuadrants.Punch)
		{
			return false;
		}
		if (!_kickEnabled && key == EQuadrants.Kick)
		{
			return false;
		}
		if (!_stickEnabled && key.isStickQuadrant())
		{
			return false;
		}
		return true;
	}
}
