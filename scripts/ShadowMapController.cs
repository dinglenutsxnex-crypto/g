using Godot;

public partial class ShadowMapController : Node3D
{
	[Export]
	private Texture2D _shadowMapNormal;
	[Export]
	private Texture2D _shadowMapInShadowForm;
	[Export]
	private float _changeShadowMapByFrame = 60f;
	[Export]
	private float _leftBorder = -660f;
	[Export]
	private float _rightBorder = 767f;
	[Export]
	private float _offset = 88f;

	private static ShadowMapController _instance;
	private Texture2D _currentShadowMap;
	private bool _isShadowMapChanging;
	private float _currentMapShadowChangeDelta;
	private float _currentMapShadowChangeTimer;

	public static ShadowMapController Instance
	{
		get { return _instance; }
	}

	public override void _Ready()
	{
		_instance = this;
		_leftBorder = SceneConfig.LeftBorderX;
		_rightBorder = SceneConfig.RightBorderX;
		_currentShadowMap = _shadowMapNormal;
		_isShadowMapChanging = false;
	}

	public Color CalculateShadowAtPoint(float xPos)
	{
		if (_shadowMapNormal == null)
		{
			GD.PrintErr("Missing shadowMapNormal at ShadowMapController");
			return Colors.White;
		}
		if (_shadowMapInShadowForm == null)
		{
			GD.PrintErr("Missing shadowMapInShadowForm at ShadowMapController");
			return Colors.White;
		}
		float num = Mathf.InverseLerp(_leftBorder - _offset, _rightBorder + _offset, xPos);
		int x = Mathf.RoundToInt((float)_currentShadowMap.GetWidth() * num);
		if (!_isShadowMapChanging)
		{
			Color col = _currentShadowMap.GetPixel(x, 0);
			return col;
		}
		Color pixel = ((!(_currentShadowMap == _shadowMapNormal)) ? _shadowMapNormal : _shadowMapInShadowForm).GetPixel(x, 0);
		Color pixel2 = _currentShadowMap.GetPixel(x, 0);
		return pixel2.Lerp(pixel, _currentMapShadowChangeDelta);
	}

	public void SwitchShadowMap(bool isInShadowForm, bool instantly = false)
	{
		_currentShadowMap = isInShadowForm ? _shadowMapInShadowForm : _shadowMapNormal;
		if (instantly)
		{
			_isShadowMapChanging = true;
			_currentMapShadowChangeTimer = _changeShadowMapByFrame;
		}
	}

	public override void _Process(double delta)
	{
		if (_isShadowMapChanging)
		{
			_currentMapShadowChangeTimer -= 1f;
			_currentMapShadowChangeDelta = _currentMapShadowChangeTimer / _changeShadowMapByFrame;
			if (_currentMapShadowChangeTimer <= 0f)
			{
				_isShadowMapChanging = false;
				_currentMapShadowChangeTimer = 0f;
			}
		}
	}
}
