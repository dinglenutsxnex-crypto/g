using System;
using Godot;
using Nekki.UI;
using SF3;
using SF3.GameModels;

public class HUD : UIModuleHolder
{
	[Serializable]
	public class ScoreUnit
	{
		[Export]
		private Label _lblScore;

		private int _score;

		private float _scoreValue;

		public float _visualizationSpeed;

		public int Score
		{
			get
			{
				return _score;
			}
			set
			{
				if (value < _score)
				{
					Reset();
				}
				_score = value;
			}
		}

		public void Update(double delta)
		{
			if (!(_scoreValue > (float)_score))
			{
				_scoreValue += _visualizationSpeed * (float)Engine.GetProcessDeltaTime();
				if (_scoreValue > (float)_score)
				{
					_lblScore.Text = _score + AdditionStringInfo();
				}
				else
				{
					_lblScore.Text = (int)_scoreValue + AdditionStringInfo();
				}
			}
		}

		private string AdditionStringInfo()
		{
			return (FightController.Settings.ScoreCount == 0) ? string.Empty : string.Format(" / {0}", FightController.Settings.ScoreCount);
		}

		public void Reset()
		{
			_score = 0;
			_scoreValue = 0f;
			_lblScore.Text = _score + AdditionStringInfo();
		}
	}

	[Serializable]
	public abstract class BaseSlideUnit
	{
		public enum DefaultStates
		{
			Empty = 0,
			Full = 1
		}

		[Export]
		protected DefaultStates _defaultState;

		public string Name;

		protected float _scale;

		public abstract float Percent { get; set; }

		public void Init(float scale = 1f)
		{
			_scale = scale;
			Reset();
		}

		public virtual void Reset()
		{
			Percent = ((_defaultState == DefaultStates.Full) ? 1 : 0);
		}
	}

	[Serializable]
	public class SlideUnit : BaseSlideUnit
	{
		[Export]
		private ProgressBar _bar;

		private float _percent;

		public override float Percent
		{
			get
			{
				return _percent;
			}
			set
			{
				if (_bar != null)
				{
					_percent = value;
					_bar.Value = _percent * _scale * 100.0;
				}
			}
		}

		public void HideThumb()
		{
		}

		public void ShowThumb()
		{
		}
	}

	[Serializable]
	public class ParticlesSlideUnit : BaseSlideUnit
	{
		public Node fullBarEffect;

		public GpuParticles3D _particles;

		public float maxParticlesRate;

		public Node3D scaleTransform;

		public override float Percent
		{
			get
			{
				return scaleTransform.Scale.X;
			}
			set
			{
				scaleTransform.Scale = new Vector3(value, scaleTransform.Scale.Y, scaleTransform.Scale.X);
			}
		}

		public override void Reset()
		{
			fullBarEffect.Visible = false;
			base.Reset();
		}
	}

	private const float SCALE_FOR_3X4 = 0.75f;

	private bool _hpEnable;

	private bool _scoreEnable;

	[Export]
	private Node _hpRoot;

	[Export]
	private Node _scoreRoot;

	public Node3D roundsUIParent;

	public Node roundSegmentPrefab;

	public float roundsUISpawnOffset;

	[Export]
	private TextureRect _avatar;

	[Export]
	private Label _name16x9;

	[Export]
	private Label _name4x3;

	[Export]
	private SlideUnit _hpMain;

	[Export]
	private SlideUnit _hpFall;

	[Export]
	private SlideUnit _hpBg;

	[Export]
	private ScoreUnit _score;

	[Export]
	private Node _hpHolder;

	[Export]
	private Node _shadowEnergyHolder;

	[Export]
	private ParticlesSlideUnit _shadowEnergy;

	[Export]
	private SlideUnit _shadowEnergySlide;

	private const float HP_UPADTE_TIME = 0.32f;

	private const float HP_FALL_UPDATE_TIME = 1f;

	private const float CRAZY_DECREASE_SPEED = 0.08f;

	private float _currentHP;

	private float _timeToHP;

	private float _currentSE;

	private float _hpUpdateSpeed;

	private float _hpFallUpdateSpeed;

	private SlideUnit _currentCrazySlide;

	private float _lastHitTime = -1f;

	public string Name
	{
		get
		{
			return currentNameLabel.Text;
		}
		set
		{
			currentNameLabel.Text = value;
		}
	}

	public Label currentNameLabel
	{
		get
		{
			if (CameraConfiguration.Current.aspect == CameraConfiguration.AspectRatio._16X9)
			{
				return _name16x9;
			}
			return _name4x3;
		}
	}

	public string Alias
	{
		set
		{
		}
	}

	public string Avatar
	{
		get
		{
			return "";
		}
		set
		{
		}
	}

	public float ShadowEnergy
	{
		get
		{
			return _currentSE;
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 1f);
			if (_currentSE != value)
			{
				_currentSE = value;
				_shadowEnergy.fullBarEffect.Visible = _currentSE >= 0.99999f;
			}
		}
	}

	public float HP
	{
		get
		{
			return _hpMain.Percent;
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 1f);
			if (_currentHP != value)
			{
				_currentHP = value;
				_lastHitTime = GameTimeController.time;
				_hpUpdateSpeed = (_hpMain.Percent - _currentHP) / 0.32f;
				_hpFallUpdateSpeed = (_hpFall.Percent - _currentHP) / 1f;
				if (Math.Abs(value - _hpMain.Percent) > 0.01f)
				{
					_timeToHP = 1f;
				}
				else
				{
					_timeToHP = 0.02f;
				}
			}
		}
	}

	public int Score
	{
		get
		{
			return _score.Score;
		}
		set
		{
			_score.Score = value;
		}
	}

	public void SetAvatarTexture(Texture2D tex)
	{
	}

	public void SetupRoundUI(int number)
	{
	}

	public override void _Ready()
	{
		base._Ready();
		Init();
		if (CameraConfiguration.Current.aspect == CameraConfiguration.AspectRatio._16X9)
		{
			_name16x9.Visible = true;
			_name4x3.Visible = false;
		}
		else
		{
			_name16x9.Visible = false;
			_name4x3.Visible = true;
		}
	}

	private void Init()
	{
		ScoreBarEnable(false);
		HPBarEnable(true);
		ShadowFormEnable(true);
		if (CameraConfiguration.Current.aspect == CameraConfiguration.AspectRatio._16X9)
		{
			_hpFall.Init();
			_hpMain.Init();
			_hpBg.Init();
		}
		else
		{
			_hpFall.Init(0.75f);
			_hpMain.Init(0.75f);
			_hpBg.Init(0.75f);
		}
		_shadowEnergySlide.Init();
		_currentHP = HP;
	}

	public void ScoreBarEnable(bool isEnable)
	{
		_scoreEnable = isEnable;
		_scoreRoot.Visible = _scoreEnable;
	}

	public void HPBarEnable(bool isEnable)
	{
		_hpEnable = isEnable;
		_hpHolder.Visible = _hpEnable;
	}

	public void ShadowFormEnable(bool isEnable)
	{
		_shadowEnergyHolder.Visible = isEnable;
	}

	public void Reset()
	{
		_hpFall.Reset();
		_hpMain.Reset();
		_hpBg.Reset();
		_score.Reset();
		_shadowEnergySlide.Reset();
		_currentHP = HP;
	}

	protected virtual void OnHit()
	{
	}

	public override void _Process(double delta)
	{
		if (_hpEnable)
		{
			if (_hpMain.Percent - _currentHP > 0f)
			{
				_hpMain.Percent = Math.Max(0f, _hpMain.Percent - GameTimeController.unscaledDeltaTime * _hpUpdateSpeed);
			}
			else if (_hpMain.Percent - _currentHP < 0f)
			{
				_hpMain.Percent = _currentHP;
			}
			if (_hpMain.Percent <= 0f)
			{
				_hpMain.HideThumb();
			}
			if (_hpFall.Percent <= 0.8f)
			{
				_hpBg.Percent = _hpFall.Percent + 0.2f;
			}
			else
			{
				_hpBg.Percent = 1f;
			}
			if (_hpFall.Percent - _hpMain.Percent > 0f && _lastHitTime + _timeToHP < GameTimeController.time)
			{
				_hpFall.Percent = Math.Max(0f, _hpFall.Percent - GameTimeController.unscaledDeltaTime * _hpFallUpdateSpeed);
			}
			else if (_hpFall.Percent - _hpMain.Percent < 0f)
			{
				_hpFall.Percent = _hpMain.Percent;
			}
			if (_currentCrazySlide != null && _currentCrazySlide.Percent > 0f && _currentCrazySlide.Percent < 1f)
			{
				_currentCrazySlide.Percent = Math.Max(0f, _currentCrazySlide.Percent - GameTimeController.unscaledDeltaTime * 0.08f);
			}
		}
		if (_scoreEnable)
		{
			_score.Update(delta);
		}
	}

	private void UpdateShadowEnergyBar(float value)
	{
		_shadowEnergySlide.Percent = value;
		_shadowEnergy.fullBarEffect.Visible = value >= 1f;
	}

	public void SetModel(Model model)
	{
		if (model.isPlayer)
		{
			SetupRoundUI(FightController.Instance.CurrentFight.roundsToWin);
		}
		else
		{
			SetupRoundUI(FightController.Instance.CurrentFight.roundsToLose);
		}
		ShadowFormController.Instance.RegisterForEvent_SetShadowCharge(model.id, UpdateShadowEnergyBar);
	}
}
