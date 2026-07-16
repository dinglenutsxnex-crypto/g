// SKELETON STUB: SF3/Items/ShopItemDescription.cs
// [MONO] Original used NGUI (UISprite, UILabel, UIButton) + DOTween.
// Needs full UI rebuild with Godot Control nodes.
using System;
using System.Collections.Generic;
using Godot;

namespace SF3.Items
{
	public partial class ShopItemDescription : Node
	{
		[Serializable]
		public class AttributesViewSettings
		{
			public float iconScale = 1f;
			public int valueFontSize = 50;
			public int nameFontSize = 20;
			public float positionX = -90f;
		}

		[Export]
		public NekkiUILabel _itemNameLbl;

		[Export]
		public Node _rarityObject;

		[Export]
		public Node _attributesObject;

		[Export]
		public Node _buyForCoinsBtn;

		[Export]
		public Node _buyForBonusBtn;

		[Export]
		public NekkiUILabel _restriction;

		[Export]
		public NekkiUILabel _requirement;

		[Export]
		public Node _subjectInformationObject;

		[Export]
		public NekkiUILabel _subjectInformationTextLbl;

		private TextureRect _rarityLine;
		private NekkiUILabel _rarityText;
		private List<Node> _boosterpackLootPics = new List<Node>();
		private Label _buyForCoinsLbl;
		private Label _buyForBonusLbl;
		private Vector3 _coinsBtnPos;
		private Vector3 _bonusBtnPos;

		[Export]
		private AttributesViewSettings[] _attributesViewSettins;

		[Export]
		private AttributesDrawer _attributes;

		[Export]
		private AttributesDrawer _upgradeAttributes;

		[Export]
		private Node _progressPlaceholder;

		[Export]
		private Node _progressPrf;

		private MultiCardProgress _progressCard;

		[Export]
		private NekkiUILabel _upgradeLabel;

		[Export]
		private Node _itemDescription;

		[Export]
		private Node _boosterpackDescription;

		[Export]
		private NekkiUILabel _boosterpackDescriptionLabel;

		[Export]
		private Node _boosterpackLoot;

		[Export]
		private Node _lootIconPrefab;

		[Export]
		private float _itemDescriptionPositionY;

		[Export]
		private float _itemDescriptionUpgradePositionY;

		[Export]
		private float _upgradeFadeDuration = 0.5f;

		[Export]
		private float _upgradeShowAttrTime = 1f;

		private Sequence _upgradeAttributesSequence;
		private SF3.UserData.ShopItem _currentShopItem;

		public new Node gameObject { get; private set; }
		public new Node3D transform { get; private set; }

		public Collider OverlapCollider { get; private set; }
		public Button BuyForCoinsBtn { get; private set; }
		public Button BuyForBonusBtn { get; private set; }

		public override void _Ready()
		{
			gameObject = this;
			transform = this as Node3D;
			OverlapCollider = gameObject.GetComponent<Collider>();
			if (_attributesViewSettins == null || _attributesViewSettins.Length == 0)
			{
				_attributesViewSettins = new AttributesViewSettings[1]
				{
					new AttributesViewSettings()
				};
			}
		}

		public void Initialize()
		{
			// STUB: NGUI UIButton→Button, UISprite→TextureRect, DOTween sequence rebuild needed
			GD.PrintErr("ShopItemDescription.Initialize: STUB - NGUI/DOTween rebuild needed");
		}

		public void ClearDescription(bool isAllCardsPurchased = false)
		{
			// STUB
		}

		public void ShowDescription(SF3.UserData.ShopItem itemShop, float animDuration = 0f)
		{
			// STUB
		}

		public void AnimateProgressOnSelectedItem()
		{
			// STUB
		}

		public void AnimateAddedProgressOnSelectedItem()
		{
			// STUB
		}

		public void AddProgressAnimationCallback(MultiCardProgressAnimationEnd callback)
		{
			// STUB
		}

		public void BreakProgressAnimation()
		{
			// STUB
		}
	}
}
