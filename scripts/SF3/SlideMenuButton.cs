using Godot;

namespace SF3
{
	public partial class SlideMenuButton : Node
	{
		private CanvasItem _canvasGroup;

		[Export]
		private float _onSelectAlpha = 1f;

		private float _regularAlpha;

		public override void _Ready()
		{
			_canvasGroup = GetNode<CanvasItem>(".");
			_regularAlpha = _canvasGroup.Modulate.A;
		}

		public void Select()
		{
			Color c = _canvasGroup.Modulate;
			c.A = _onSelectAlpha;
			_canvasGroup.Modulate = c;
		}

		public void Deselect()
		{
			Color c = _canvasGroup.Modulate;
			c.A = _regularAlpha;
			_canvasGroup.Modulate = c;
		}
	}
}
