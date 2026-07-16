using Godot;
using System;

public class Stick : UIModuleHolder
{
	public class ControlPoint
	{
		private EDirections _direction;
		private Node3D _t;
		private bool _posObtained;
		private Vector3 _pos;

		public Vector3 Position
		{
			get
			{
				if (!_posObtained)
				{
					_pos = _t.Position;
					_posObtained = true;
				}
				return _pos;
			}
		}

		public EDirections Direction
		{
			get
			{
				return _direction;
			}
		}
	}

	public class StickUnit
	{
		public string Name;
		public EQuadrants Quadrant;
		public UISprite Normal;
		public UISprite Active;
		public Node TutorUnit;
		public Node ShadowHintUnit;

		public bool Tutor { get; private set; }

		public void SetTutor(EQuadrants[] array, bool active)
		{
			if (array == null)
			{
				SetState(false);
				return;
			}
			foreach (EQuadrants eQuadrants in array)
			{
				if (eQuadrants == Quadrant)
				{
					SetState(active);
				}
			}
		}

		private void SetState(bool state)
		{
			Tutor = state;
			Normal.Visible = !state;
			Active.Visible = !state;
			TutorUnit.Visible = state;
		}

		private bool IsStickQuadrant(EQuadrants quadrant)
		{
			switch (quadrant)
			{
			case EQuadrants.Zero:
			case EQuadrants.One:
			case EQuadrants.Two:
			case EQuadrants.Three:
			case EQuadrants.Four:
			case EQuadrants.Five:
			case EQuadrants.Six:
			case EQuadrants.Seven:
			case EQuadrants.Eight:
				return true;
			default:
				return false;
			}
		}
	}

	private UISprite Highlight;
	private UISprite ContainerNorm;
	private UISprite ContainerAction;
	private UISprite ContainerTutor;
	private Node ContainerShadowHints;
	private UISprite StickNorm;
	private UISprite StickAction;
	private Node StickTutor;

	public StickUnit[] StickUnits;
	private ControlPoint[] ControlPoints;

	public const int onStickBegan = 0;
	public const int onStickChange = 1;
	public const int onStickEnd = 2;

	private static Stick _instance;
	private static Vector3 _sheduledScale = Vector3.One;
	private bool informStickStart;
	private static EDirections _lastDirection = EDirections.Idle;

	public static Stick Instance
	{
		get
		{
			return _instance;
		}
	}

	public static EDirections Direction
	{
		get
		{
			return _lastDirection;
		}
		set
		{
			if (_lastDirection != value)
			{
				_instance.callEvent(2, new AbstractController.Key(Godot.Key.None, _lastDirection.GetQuadrant(), 1));
				_lastDirection = value;
				_instance.callEvent(1, new AbstractController.Key(Godot.Key.None, Direction.GetQuadrant(), 1));
			}
		}
	}

	public static EQuadrants Quadrant
	{
		get
		{
			return _lastDirection.GetQuadrant();
		}
	}

	public static void SetScale(float scale)
	{
		if (_instance != null)
		{
			_instance.Scale = new Vector3(scale, scale, 1f);
		}
		else
		{
			_sheduledScale = new Vector3(scale, scale, 1f);
		}
	}

	internal void Start()
	{
		_instance = this;
		SetTutor(null);
		Scale = _sheduledScale;
		GamepadController.Instance.SubscribeUIElement(this);
		if (NekkiUtils.IsPhone())
		{
			SetScale(1.3f);
		}
	}

	public static void OpenInterface()
	{
		_instance.callEvent(2, new AbstractController.Key(Godot.Key.None, _lastDirection.GetQuadrant(), 1));
	}

	public void DragProcess(Node3D stick, bool inSafe)
	{
		if (inSafe)
		{
			Direction = EDirections.Idle;
			return;
		}
		if (!informStickStart)
		{
			informStickStart = true;
			callEvent(2, new AbstractController.Key(Godot.Key.None, EDirections.Idle.GetQuadrant(), 1));
		}
		Vector3 position = stick.Position;
		ControlPoint controlPoint = null;
		float num = float.MaxValue;
		for (int i = 0; i < ControlPoints.Length; i++)
		{
			if (controlPoint == null)
			{
				controlPoint = ControlPoints[i];
				num = position.DistanceTo(controlPoint.Position);
			}
			else if (position.DistanceTo(ControlPoints[i].Position) < num)
			{
				controlPoint = ControlPoints[i];
				num = position.DistanceTo(controlPoint.Position);
			}
		}
		if (controlPoint != null)
		{
			Direction = controlPoint.Direction;
		}
	}

	public void Release()
	{
		if (informStickStart)
		{
			callEvent(2, new AbstractController.Key(Godot.Key.None, Direction.GetQuadrant(), 1));
			informStickStart = false;
		}
		Direction = EDirections.Idle;
	}

	public static void SetActive(bool value)
	{
		if (_instance != null)
		{
			_instance.Visible = value;
		}
	}

	public void SetTutor(EQuadrants[] buttons, bool active = true)
	{
		ContainerTutor.Visible = false;
		StickTutor.Visible = false;
		StickUnit[] stickUnits = StickUnits;
		foreach (StickUnit stickUnit in stickUnits)
		{
			stickUnit.SetTutor(buttons, active);
			if (stickUnit.Tutor)
			{
				ContainerTutor.Visible = true;
				StickTutor.Visible = true;
			}
		}
	}

	public void SetShadowHint(EQuadrants slot, bool active)
	{
		StickUnit[] stickUnits = StickUnits;
		foreach (StickUnit stickUnit in stickUnits)
		{
			if (stickUnit.Quadrant == slot && stickUnit.ShadowHintUnit != null)
			{
				stickUnit.ShadowHintUnit.Visible = active;
				stickUnit.Normal.Visible = !active;
				stickUnit.Active.Visible = !active;
			}
		}
	}
}
