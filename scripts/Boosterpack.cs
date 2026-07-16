// ⚠️ STUB: needs full port — original used NGUI UITexture, UISprite, UILabel, NekkiUICamera, ParticleSystem
using System;
using SF3.Settings;
using Godot;

public partial class Boosterpack : Control
{
	private BoosterpackScrollItem _scrollItem;
	[Export]
	private TextureRect _slicableTexture;
	[Export]
	private TextureRect _graphicsSprite;
	[Export]
	private Label _zoneLabel;
	[Export]
	private Label _rarityLabel;
	private int _modelID;
	[Export]
	private GpuParticles2D _shining;
	[Export]
	private GpuParticles2D _explosion;
	private Action _onClick;

	public BoosterpackScrollItem ScrollItem { get { return _scrollItem; } }
	public TextureRect SlicableTexture { get { return _slicableTexture; } }
	public int ModelID { get { return _modelID; } }

	public override void _EnterTree()
	{
	}

	public void Init(BoosterpackScrollItem _parentScrollItem, Action onClick = null)
	{
		_scrollItem = _parentScrollItem;
		_modelID = _parentScrollItem.ModelID;
		_onClick = onClick;
		RefreshGraphics();
	}

	public void RefreshGraphics()
	{
		RefreshZoneLabel();
		RefreshRarityLabel();
	}

	public void OnClick()
	{
		if (_onClick != null)
		{
			_onClick();
		}
	}

	public void ShineOn()
	{
		_shining.Emitting = true;
	}

	public void Explode()
	{
		_shining.Emitting = false;
		_explosion.Emitting = true;
	}

	private void RefreshZoneLabel()
	{
		_zoneLabel.Text = string.Format("CHAPTER {0}", JS.Instance.GetBoosterByID(_modelID).Zone);
		_zoneLabel.Modulate = GameSettings.ItemSettings.GetBoosterTextColor(_scrollItem.Rarity);
	}

	private void RefreshRarityLabel()
	{
		_rarityLabel.Text = string.Format("{0}", _scrollItem.Rarity.ToString());
		_rarityLabel.Modulate = GameSettings.ItemSettings.GetBoosterTextColor(_scrollItem.Rarity);
	}
}
