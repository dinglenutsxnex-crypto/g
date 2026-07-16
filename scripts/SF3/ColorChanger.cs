using Godot;

namespace SF3
{
	public partial class ColorChanger : Node3D
	{
		[Export]
		public Color fromColor = Colors.White;
		[Export]
		public Color toColor = Colors.Black;
		[Export]
		public bool onlyAlpha;
		[Export]
		public float forFrames = 60f;
		[Export]
		public Curve tweenCurve;
		[Export]
		public string shaderColorField = string.Empty;
		[Export]
		public bool playOnEnable = true;

		private Material _currentMaterial;
		private float _startFrame;
		private float _endFrame;
		private bool _active;
		private bool _inited;

		private void Init()
		{
			MeshInstance3D component = GetNodeOrNull<MeshInstance3D>(".");
			if (component != null)
			{
				_currentMaterial = component.MaterialOverride;
			}
			else
			{
				MeshInstance3D component2 = GetNodeOrNull<MeshInstance3D>(".");
				if (component2 != null)
				{
					_currentMaterial = component2.MaterialOverride;
				}
			}
			_active = false;
			_inited = true;
		}

		public override void _EnterTree()
		{
			if (playOnEnable)
			{
				Play();
			}
		}

		public override void _Process(double delta)
		{
			if (_active)
			{
				float t = tweenCurve.Sample((GameTimeController.battleTime - _startFrame) / forFrames);
				Color color = fromColor.Lerp(toColor, t);
				if (_currentMaterial != null)
				{
					if (shaderColorField.Length != 0)
					{
						_currentMaterial.SetShaderParameter(shaderColorField, color);
					}
					else
					{
						_currentMaterial.SetShaderParameter("albedo_color", color);
					}
				}
				if (GameTimeController.battleTime >= _endFrame)
				{
					if (_currentMaterial != null)
					{
						if (shaderColorField.Length != 0)
						{
							_currentMaterial.SetShaderParameter(shaderColorField, toColor);
						}
						else
						{
							_currentMaterial.SetShaderParameter("albedo_color", toColor);
						}
					}
					_active = false;
				}
			}
		}

		public void Play()
		{
			if (!_inited)
			{
				Init();
			}
			if (onlyAlpha)
			{
				float a = fromColor.A;
				float a2 = toColor.A;
				if (shaderColorField.Length != 0 && _currentMaterial != null)
				{
					fromColor = (Color)_currentMaterial.GetShaderParameter(shaderColorField);
					toColor = fromColor;
				}
				else if (_currentMaterial != null)
				{
					fromColor = (Color)_currentMaterial.GetShaderParameter("albedo_color");
					toColor = fromColor;
				}
				fromColor = new Color(fromColor.R, fromColor.G, fromColor.B, a);
				toColor = new Color(toColor.R, toColor.G, toColor.B, a2);
			}
			_active = true;
			if (_currentMaterial != null)
			{
				if (shaderColorField.Length != 0)
				{
					_currentMaterial.SetShaderParameter(shaderColorField, fromColor);
				}
				else
				{
					_currentMaterial.SetShaderParameter("albedo_color", fromColor);
				}
			}
			_startFrame = GameTimeController.battleTime;
			_endFrame = _startFrame + forFrames;
		}
	}
}
