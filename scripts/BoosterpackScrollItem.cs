// ⚠️ STUB: needs full port — original used NGUI UIDragScrollView, UILabel, UIButton, UISprite, EventDelegate
using SF3.Settings;
using Godot;
using sf3DTO;

public partial class BoosterpackScrollItem : Control
{
	[Export]
	private Label _countLabel;
	[Export]
	private Label _zoneLabel;
	[Export]
	private Label _rarityLabel;
	[Export]
	private Button _button;
	[Export]
	private TextureRect _Image;

	private int _modelID;
	private int _count;
	private Rarity _rarity;

	public int ModelID { get { return _modelID; } }
	public Rarity Rarity { get { return _rarity; } }

	public void Init(int modelID, int count, Action onClick = null)
	{
		_modelID = modelID;
		_count = count;
		_rarity = JS.Instance.GetBoosterByID(_modelID).GetRarityType();
		if (onClick != null)
		{
			_button.Pressed += onClick;
		}
		RefreshGraphics();
	}

	public bool WillBeDeletedAfterSpend()
	{
		return _count <= 1;
	}

	public void SpendBoosterpack()
	{
		_count--;
		if (_count > 0)
		{
			RefreshCountLabel();
		}
		else
		{
			QueueFree();
		}
	}

	private void RefreshGraphics()
	{
		RefreshCountLabel();
		RefreshZoneLabel();
		RefreshRarityLabel();
	}

	private void RefreshCountLabel()
	{
		if (_count > 1)
		{
			_countLabel.Visible = true;
			_countLabel.Text = string.Format("X{0}", _count);
		}
		else
		{
			_countLabel.Visible = false;
		}
	}

	private void RefreshZoneLabel()
	{
		_zoneLabel.Text = string.Format("CHAPTER {0}", JS.Instance.GetBoosterByID(_modelID).Zone);
		_zoneLabel.Modulate = GameSettings.ItemSettings.GetBoosterTextColor(_rarity);
	}

	private void RefreshRarityLabel()
	{
		_rarityLabel.Text = string.Format("{0}", _rarity.ToString());
		_rarityLabel.Modulate = GameSettings.ItemSettings.GetBoosterTextColor(_rarity);
	}
}
