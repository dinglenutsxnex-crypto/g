using System;
using Godot;

public partial class LinearExpBar : Control, IAnimatedExpBar
{
	public delegate void AnimationEndDelegate();

	private long _addedExp;

	[Export]
	private ProgressBar _progressBar;

	[Export]
	private Label _nextLevelLbl;

	[Export]
	private Label _currentLevelLbl;

	[Export]
	private float _progressDelay = 0.4f;

	[Export]
	private Control _givenExpObj;

	[Export]
	private Label _givenExpLbl;

	[Export]
	private Control _newLevelObj;

	[Export]
	private Label _levelLbl;

	private ProgressBarAnimation _progressBarAnimation;

	private long _currentLevel;

	private long _fromExp;

	private long _maxExp;

	private long _needExp;

	private long _animTo;

	private bool _hasNewLevel;

	public long CurrentLevel
	{
		get
		{
			return _currentLevel;
		}
		set
		{
			_currentLevel = value;
			_currentLevelLbl.Text = _currentLevel.ToString();
			_nextLevelLbl.Text = (_currentLevel + 1).ToString();
			_levelLbl.Text = _currentLevelLbl.Text;
			_maxExp = 100;
		}
	}

	public long FromExp
	{
		get
		{
			return _fromExp;
		}
		set
		{
			_fromExp = value;
			if (_maxExp > 0)
			{
				_progressBar.Value = (float)_fromExp / (float)_maxExp;
			}
		}
	}

	public long AddedExp
	{
		get
		{
			return _addedExp;
		}
		set
		{
			_addedExp = value;
			_needExp = FromExp + _addedExp;
			_givenExpLbl.Text = value + "xp";
		}
	}

	public event ExpBarAnimationEnd onAnimationEnd;

	public override void _Ready()
	{
		Init();
	}

	public override void _Process(double delta)
	{
	}

	private void Init()
	{
		_progressBarAnimation = GetNode<ProgressBarAnimation>("ProgressBarAnimation");
		_givenExpObj.Visible = false;
		_newLevelObj.Visible = false;
	}

	public void AnimateExp()
	{
		if (FromExp > _needExp)
		{
			GD.PrintErr("Argument 'to' less then 'currentExp'");
			return;
		}
		_hasNewLevel = false;
		_givenExpObj.Visible = false;
		_progressBar.Visible = true;
		_newLevelObj.Visible = false;
		Modulate = new Color(1, 1, 1, 0);
		_animTo = AddedExp;
		StartMainInAnimation();
	}

	public void BreakAnimation()
	{
		StopAllAnimation();
		CalculateNewLevel();
		_givenExpObj.Visible = true;
		Scale = Vector3.One;
		_progressBar.Visible = !_hasNewLevel;
		_givenExpObj.Visible = !_hasNewLevel;
		_newLevelObj.Visible = _hasNewLevel;
	}

	private void StartMainInAnimation()
	{
		Tween tween = CreateTween();
		tween.TweenProperty(this, "modulate", new Color(1, 1, 1, 1), 0.3f);
		tween.TweenCallback(Callable.From(StartExpInAnimation));
	}

	private void StartExpInAnimation()
	{
		if (_needExp == FromExp)
		{
			AnimationEnd();
		}
		else if (_animTo == 0)
		{
			StartProgressAnimation();
		}
		else
		{
			StartExpInAnimationCoroutine();
		}
	}

	private async void StartExpInAnimationCoroutine()
	{
		await ToSignal(GetTree().CreateTimer(_progressDelay), "timeout");
		_givenExpObj.Visible = true;
		Tween tween = CreateTween();
		tween.TweenProperty(_givenExpObj, "position", Vector2.Zero, 0.3f);
		tween.TweenCallback(Callable.From(StartProgressAnimation));
	}

	private void StartProgressAnimation()
	{
		if (_needExp < 0)
		{
			return;
		}
		if (_needExp >= _maxExp && _maxExp > 0)
		{
			_needExp -= _maxExp;
			_progressBarAnimation.Play(1f);
			_hasNewLevel = true;
			return;
		}
		if (_maxExp > 0)
		{
			_progressBarAnimation.Play((float)_needExp / (float)_maxExp);
		}
		_needExp = -1L;
	}

	private void AfterProgressAnimation()
	{
		if (_needExp >= 0)
		{
			HalloEndAnimation(0.5f);
			StartNextLevelBounceAnimation();
		}
		else if (_needExp < 0 && _hasNewLevel)
		{
			StartMainOutAnimation();
		}
		else
		{
			AnimationEnd();
		}
	}

	private void StartNextLevelBounceAnimation()
	{
		Tween tween = CreateTween();
		tween.TweenProperty(_nextLevelLbl, "scale", new Vector2(1.2f, 1.2f), 0.15f);
		tween.TweenProperty(_nextLevelLbl, "scale", Vector2.One, 0.15f);
		tween.TweenCallback(Callable.From(AfterBounceAnimation));
	}

	private void AfterBounceAnimation()
	{
		CurrentLevel++;
		if (_needExp == 0 && !_hasNewLevel)
		{
			StartMainOutAnimation();
			return;
		}
		FromExp = 0L;
		StartProgressAnimation();
	}

	private void StartMainOutAnimation()
	{
		Tween tween = CreateTween();
		tween.TweenProperty(this, "scale", Vector2.Zero, 0.3f);
		tween.TweenCallback(Callable.From(StartNewLevelAnimation));
	}

	private void StartNewLevelAnimation()
	{
		_progressBar.Visible = false;
		_givenExpObj.Visible = false;
		_newLevelObj.Visible = true;
		Scale = Vector2.One;
		Tween tween = CreateTween();
		tween.TweenProperty(_newLevelObj, "scale", Vector2.One, 0.3f);
		tween.TweenCallback(Callable.From(AnimationEnd));
	}

	private void AnimationEnd()
	{
		if (onAnimationEnd != null)
		{
			onAnimationEnd();
		}
	}

	private void StopAllAnimation()
	{
		_progressBarAnimation?.Break();
		AnimationEnd();
	}

	private void CalculateNewLevel()
	{
		while (_needExp > 0)
		{
			if (_needExp >= _maxExp && _maxExp > 0)
			{
				_needExp -= _maxExp;
				CurrentLevel++;
				_hasNewLevel = true;
			}
			else
			{
				FromExp = _needExp;
				_needExp = -1L;
			}
		}
	}

	public void Hide()
	{
		Visible = false;
	}

	public void Show()
	{
		Visible = true;
	}

	private void HalloEndAnimation(float duration)
	{
	}

	private void UpdateHalloPosition()
	{
	}
}
