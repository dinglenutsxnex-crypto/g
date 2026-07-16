using System;
using Godot;
using Color = Godot.Color;
using SF3;
using SF3.Audio;
using SF3.Effects;
using sf3DTO;

public class RoundsUI : UIModuleHolder
{
	public struct Shake
	{
		public bool roundStart;

		public bool roundWin;

		public bool roundLose;

		public bool fightStart;

		public bool fightEnd;

		public bool perfect;

		public bool great;

		public bool timeout;
	}

	public class TextAnimation
	{
		public Label label;

		public TextureRect back;

		public bool active;

		public string SetText
		{
			set
			{
				label.Text = value;
				active = true;
			}
		}

		public void Init()
		{
			label.Visible = false;
			back.Visible = false;
		}

		private void PlayAppearingAnimation(bool useFogging)
		{
			if (useFogging)
			{
				FoggingController.Instance.ShowFogging();
			}
			back.Visible = true;
			label.Visible = true;
			Color backColor = new Color(1f, 1f, 1f, _minLabelVisibility);
			back.SelfModulate = backColor;
			Tween tween = back.CreateTween();
			tween.TweenProperty(back, "self_modulate:a", 0.65f, _animationTime);
			Tween scaleTween = label.CreateTween();
			scaleTween.TweenProperty(label, "scale", new Vector2(_finalLabelScale, _finalLabelScale), _animationTime);
			scaleTween.TweenCallback(Callable.From(ShakeCamera));
		}

		private void PlayDisapperaingAnimation()
		{
			Tween fadeTween = label.CreateTween();
			fadeTween.TweenProperty(label, "modulate:a", _minLabelVisibility, _animationTime);
			fadeTween.TweenCallback(Callable.From(() => label.Visible = false));
			FoggingController.Instance.HideFogging();
			back.SelfModulate = new Color(1f, 1f, 1f, _maxLabelVisibility);
			Tween backTween = back.CreateTween();
			backTween.TweenProperty(back, "self_modulate:a", _minLabelVisibility, _animationTime);
			backTween.TweenCallback(Callable.From(() => back.Visible = false));
		}

		public void Show(Vector3 from, Vector3 to, bool useFogging, Color? textColor = null)
		{
			label.Visible = true;
			label.Modulate = textColor ?? Colors.White;
			label.Scale = new Vector2(from.X, from.Y);
			PlayAppearingAnimation(useFogging);
		}

		public void Hide()
		{
			active = false;
			PlayDisapperaingAnimation();
		}
	}

	public class RoundMessageData
	{
		public int RoundNumber { get; private set; }

		public string PlayerName { get; private set; }

		public ERoundResult RoundResult { get; private set; }

		public sf3DTO.FightResult FightResultType { get; private set; }

		public sf3DTO.BattleType BattleType { get; private set; }

		public int RoundsWon { get; private set; }

		public RoundMessageData(int round)
		{
			RoundNumber = round;
		}

		public RoundMessageData(ERoundResult result)
		{
			RoundResult = result;
		}

		public RoundMessageData(ERoundResult result, string name)
		{
			RoundResult = result;
			PlayerName = name;
		}

		public RoundMessageData(SF3.FightResult result)
		{
			FightResultType = result.resultType;
			BattleType = result.BattleType;
			RoundsWon = result.roundsWon;
		}
	}

	public const string RoundNumberColor = "FE762B";

	[Export]
	private Color greatColor;

	[Export]
	private Color perfectColor;

	[Export]
	private static float shakeFrames;

	[Export]
	private static Vector3 shakeAmplitude;

	[Export]
	private static Vector3 shakePeriod;

	[Export]
	private static Shake shakeCamera;

	[Export]
	private static float _animationTime = 0.4f;

	[Export]
	private static float _finalLabelScale = 1f;

	[Export]
	private static float _minLabelVisibility;

	[Export]
	private static float _maxLabelVisibility = 0.65f;

	private static bool shake;

	public TextAnimation[] textAnimations;

	public float startSize;

	[Export]
	private string sound_RoundStart = string.Empty;

	[Export]
	private string sound_RoundFightStart = string.Empty;

	[Export]
	private string sound_RoundFightWin = string.Empty;

	[Export]
	private string sound_RoundFightFailed = string.Empty;

	[Export]
	private string sound_FightWin = string.Empty;

	[Export]
	private string sound_FightLose = string.Empty;

	[Export]
	private string sound_Perfect = string.Empty;

	[Export]
	private string sound_Great = string.Empty;

	[Export]
	private string sound_Timeout = string.Empty;

	public override void _Ready()
	{
		base._Ready();
		for (int i = 0; i < textAnimations.Length; i++)
		{
			textAnimations[i].Init();
		}
		AudioManager.Instance.LoadSound(sound_RoundStart, sound_RoundFightStart, sound_RoundFightWin, sound_RoundFightFailed, sound_FightWin, sound_FightLose, sound_Perfect, sound_Great, sound_Timeout);
		ConfigurableDialogModule.onDialogOpened += onDialogOpened;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		ConfigurableDialogModule.onDialogOpened -= onDialogOpened;
	}

	public void ShowText(string text, bool useFogging = false, Color? textColor = null)
	{
		Clear();
		for (int i = 0; i < textAnimations.Length; i++)
		{
			if (!textAnimations[i].active)
			{
				textAnimations[i].SetText = text;
				textAnimations[i].Show(startSize * Vector3.One, Vector3.One, useFogging, textColor);
				break;
			}
		}
	}

	public static void ShakeCamera()
	{
		if (shake)
		{
			EffectsManager.ShakeCamera(shakeFrames, shakeAmplitude, shakePeriod);
		}
	}

	public void ShowPerfect()
	{
		ShowText("PERFECT!", false, perfectColor);
		PlaySoundAndShake(sound_Perfect, shakeCamera.perfect);
	}

	public void ShowGreat()
	{
		ShowText("GREAT!", false, greatColor);
		PlaySoundAndShake(sound_Great, shakeCamera.great);
	}

	public void ShowRoundEnd(RoundMessageData data)
	{
		if (data.RoundResult == ERoundResult.TIME_OUT)
		{
			string text = "TIMEOUT!";
			PlaySoundAndShake(sound_Timeout, shakeCamera.timeout);
			ShowText(text);
		}
	}

	public void ShowRoundEnd_PVP(RoundMessageData data)
	{
		string text;
		if (data.RoundResult == ERoundResult.TIME_OUT)
		{
			text = "TIMEOUT!";
			PlaySoundAndShake(sound_Timeout, shakeCamera.timeout);
		}
		else if (data.RoundResult == ERoundResult.PLAYER_WIN)
		{
			text = data.PlayerName + " WINS";
			PlaySoundAndShake(sound_RoundFightWin, shakeCamera.roundWin);
		}
		else
		{
			text = data.PlayerName + " WINS";
			PlaySoundAndShake(sound_RoundFightFailed, shakeCamera.roundLose);
		}
		ShowText(text);
	}

	public void ShowRoundStart(RoundMessageData data)
	{
		ShowText("ROUND " + data.RoundNumber.ToColored("FE762B"), true);
		PlaySoundAndShake(sound_RoundStart, shakeCamera.roundStart);
	}

	public void ShowFightStart()
	{
		ShowText("FIGHT!");
		PlaySoundAndShake(sound_RoundFightStart, shakeCamera.fightStart);
	}

	public void ShowFightEnd(RoundMessageData data)
	{
		if (data.BattleType == sf3DTO.BattleType.Survival)
		{
			ShowText(string.Format("ROUNDS WON: " + data.RoundsWon.ToColored("FE762B")));
			return;
		}
		if (data.FightResultType == sf3DTO.FightResult.Win)
		{
			ShowText("YOU WIN!");
			PlaySoundAndShake(sound_FightWin, shakeCamera.fightEnd);
		}
		else
		{
			ShowText("YOU LOSE!");
		}
	}

	private void PlaySoundAndShake(string soundName, bool enable = false)
	{
		AudioManager.Instance.PlaySound(soundName);
		shake = enable;
	}

	public void Clear()
	{
		for (int i = 0; i < textAnimations.Length; i++)
		{
			if (textAnimations[i].active)
			{
				textAnimations[i].Hide();
			}
		}
	}

	private void onDialogOpened(DialogConfig config)
	{
		foreach (TextAnimation textAnimation in textAnimations)
		{
			if (textAnimation.label != null)
			{
				textAnimation.label.Visible = false;
			}
		}
	}
}
