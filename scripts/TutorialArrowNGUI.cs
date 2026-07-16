using Godot;
public partial class TutorialArrowNGUI : TutorialPointer
{
	public float offset;
	public float amplitude;
	public Curve animCurve;
	public Color startColor;
	public Color endColor;
	private Node3D _transf;
	private Control _widget;
	private TextureRect _sprite;
	public void Init(float duration)
	{
		if (!_init)
		{
			base.Init();
			_transf = GetComponent<Node3D>();
			_widget = GetComponent<Control>();
			Node3D child = _transf.GetChild(0);
			_sprite = child.GetComponent<TextureRect>();
			AddPositionTween(child, amplitude, duration, animCurve);
			AddColorTween(_sprite, startColor, endColor, duration);
		}
	}
	public void SetPosition(Control targetWidget, ArrowPosition position)
	{
		_widget.SetAnchor(targetWidget, 0, 0, 0, 0);
		switch (position)
		{
		case ArrowPosition.Bottom:
			SetRelativePosition(true, 0f, 0f - offset - (float)_sprite.height, 0f - offset, 0f);
			break;
		case ArrowPosition.Top:
			SetRelativePosition(true, 1f, offset, offset + (float)_sprite.height, 180f);
			break;
		case ArrowPosition.Left:
			SetRelativePosition(false, 0f, 0f - offset, 0f - offset - (float)_sprite.height, -90f);
			break;
		case ArrowPosition.Right:
			SetRelativePosition(false, 1f, offset + (float)_sprite.height, offset, 90f);
			break;
		}
	}
	private void SetRelativePosition(bool vertical, float relative, float offset1, float offset2, float angle)
	{
		_transf.localRotation = Quaternion.FromEuler(0f, 0f, angle);
		if (vertical)
		{
			_widget.bottomAnchor.Set(relative, offset1);
			_widget.topAnchor.Set(relative, offset2);
		}
		else
		{
			_widget.leftAnchor.Set(relative, offset1);
			_widget.rightAnchor.Set(relative, offset2);
		}
	}
}

