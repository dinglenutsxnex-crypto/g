// SKELETON STUB: SF3/Items/ItemDetailsPanel.cs
// [MONO] Original used NGUI (UISprite, Object.Instantiate, UIProgressBar).
// Needs full UI rebuild with Godot Control nodes.
using Godot;
using Nekki.UI;

namespace SF3.Items
{
	public partial class ItemDetailsPanel : Node
	{
		public Node3D itemCardPosition;

		public NekkiUILabel itemNameLbl;

		public Node itemCardPrefab;

		public Button detailsButton;

		public string normalButtonName;

		public string pressedButtonName;

		public AttributesDrawer attributeDrawer;

		public Node3D progressPosition;

		public Node progressPrf;

		public Node3D perkInfoBarPosition;

		public Node perkInfoBarPrf;

		public Node perkInfoBarEmtyCellPrf;

		private PerkInfoDrawer _perkInfroDrawer;

		private ProgressBar _progress;

		private CardItem itemCard;

		public bool active
		{
			get { return Visible; }
		}

		private void SwitchButtonImage(string spriteName)
		{
			// STUB: NGUI UISprite.spriteName → need TextureRect rebuild
		}

		public void Init()
		{
			// STUB: NGUI Object.Instantiate → need PackedScene.Instantiate
			GD.PrintErr("ItemDetailsPanel.Init: STUB - NGUI rebuild needed");
		}

		public void DisablePanel()
		{
			SwitchButtonImage(normalButtonName);
			Visible = false;
		}

		public void ActivatePanel(BaseItem item)
		{
			SwitchButtonImage(pressedButtonName);
			Visible = true;
			if (itemCard != null)
			{
				itemCard.Init(item);
				itemCard.UpdateShade(0f);
				itemNameLbl.Alias = item.alias;
				itemCard.SetImage();
			}
			IAttributable attributable = item as IAttributable;
			if (attributable != null)
			{
				attributeDrawer.Draw(attributable.GetAttributesForDisplayData());
			}
			IStackable stackable = item as IStackable;
			if (stackable != null)
			{
				_progress.Visible = true;
				_progress.Value = stackable.GetBarValue() * 100f;
			}
			else
			{
				_progress.Visible = false;
			}
			if (_perkInfroDrawer != null)
				_perkInfroDrawer.Draw(item);
		}

		public void ActivatePanel(int id)
		{
			ActivatePanel(UserManager.UserModelInfo.GetItemByID(id));
		}

		public void Refresh(BaseItem item)
		{
			SwitchButtonImage(pressedButtonName);
			if (itemCard != null)
			{
				itemCard.Init(item);
				itemNameLbl.Alias = item.alias;
				itemCard.SetImage();
			}
		}
	}
}
