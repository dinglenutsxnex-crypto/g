using Godot;

namespace SF3.Settings
{
	public partial class DojoSettings : Node
	{
		[Export]
		private Color _defaultLocationColor = Colors.White;
		[Export]
		private Color _locationColorInModule = Colors.Gray;
		[Export]
		private float _locationColorChangeTime = 1f;

		public Color DefaultLocationColor
		{
			get { return _defaultLocationColor; }
		}

		public Color LocationColorInModule
		{
			get { return _locationColorInModule; }
		}

		public float LocationColorChangeTime
		{
			get { return _locationColorChangeTime; }
		}
	}
}
