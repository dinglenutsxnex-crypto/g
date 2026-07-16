using Godot;

public class TutorialComponentNative : TutorialComponent
{
	private ButtonSelectionNative _selector;

	private TutorialArrowNative _arrow;

	[Export]
	protected TextureRect _selectorIcon;

	[Export]
	protected Label _selectorLabel;

	private Control _selectMask;

	private Control _rect;

	public override Vector3 Viewport
	{
		get
		{
			Camera3D canvasCamera = NekkiCanvasRoot.instance.GetCanvasCamera();
			return canvasCamera.WorldToViewportPoint(GlobalPosition);
		}
	}

	public override bool IsNative()
	{
		return true;
	}

	protected override void _Process(double delta)
	{
	}

	public override void Select()
	{
		SetBlockLayer(true);
		SelectComponent(true);
		TutorialManager.Instance.TutorialBlockNative.ShowDarkness(true);
		TutorialManager.Instance.TutorialBlockNGUI.ShowDarkness(false);
	}

	public override void _Ready()
	{
		TrySetIdFromPrefab();
		InitComponents();
		InitColor();
	}

	protected override void InitComponents()
	{
		_rect = this as Control;
		Button component = this as Button;
		if (component != null)
		{
			component.Pressed += base.OnClick;
		}
	}

	protected override void InitColor()
	{
		if (_selectorIcon != null)
		{
			_iconColor = _selectorIcon.Modulate;
		}
		if (_selectorLabel != null)
		{
			_labelColor = _selectorLabel.Modulate;
		}
	}

	protected override void CreateArrow()
	{
		if (arrowPosition != ArrowPosition.None)
		{
			if (_arrow == null)
			{
				_arrow = TutorialManager.Instance.TutorialBlockNative.CreateArrow();
			}
			_arrow.SetPosition(_rect, arrowPosition);
		}
	}

	protected override void CreateSelectionBorder()
	{
		if (selectorPrf == null)
		{
			if (_selector == null)
			{
				_selector = TutorialManager.Instance.TutorialBlockNative.AddSelection(selectorPrf);
			}
			_selector.Set(_rect, _selectorIcon, _selectorLabel);
		}
	}

	protected override void CreateSelectionMask()
	{
		if (selectMaskPrf != null && _selectMask == null)
		{
			_selectMask = TutorialMaskGenerator.Instance.AddMask(selectMaskPrf, _rect);
		}
	}

	protected override void SelectComponent(bool select)
	{
		_isSelect = select;
		isDragable = false;
		isDragTarget = false;
		if (select)
		{
			CreateSelectionParts();
			SetSelectionIconColor(Colors.Transparent);
			SetSelectionLabelColor(Colors.Transparent);
		}
		else
		{
			DestroySelectionParts();
			SetSelectionIconColor(_iconColor);
			SetSelectionLabelColor(_labelColor);
		}
	}

	protected void SetSelectionIconColor(Color color)
	{
		if (_selectorIcon != null)
		{
			_selectorIcon.Modulate = color;
		}
	}

	protected void SetSelectionLabelColor(Color color)
	{
		if (_selectorLabel != null)
		{
			_selectorLabel.Modulate = color;
		}
	}

	protected override void DestroySelectionParts()
	{
		base.DestroySelectionParts();
		if (_selector != null)
		{
			_selector.QueueFree();
		}
		if (_arrow != null)
		{
			_arrow.QueueFree();
		}
		if (_selectMask != null)
		{
			_selectMask.QueueFree();
		}
	}
}
