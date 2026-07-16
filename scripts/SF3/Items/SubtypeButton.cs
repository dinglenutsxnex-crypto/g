using System;
using SF3.UserData;
using Godot;

namespace SF3.Items
{
	[Serializable]
	public partial class SubtypeButton
	{
		public TextureRect Badge;

		public Label BadgeLabel;

		public EquipmentType TypeEquipment;

		public Node3D subTypeButton;

		public static SubtypeButton CurrentlySelected;

		private Tween tweenPosition;

		private bool inited;

		public Area3D Collider { get; private set; }

		public long NewItems { get; private set; }

		public static void Reset()
		{
			CurrentlySelected = null;
		}

		public void Init()
		{
			if (!inited)
			{
				inited = true;
				tweenPosition = subTypeButton.Node.CreateTween();
				tweenPosition.TweenProperty(subTypeButton, "position", subTypeButton.Position + new Vector3(14f, 0f, 0f), 0.15f);
				tweenPosition.Stop();
				Collider = subTypeButton.GetNode<Area3D>("Area3D");
			}
		}

		public void UpateBadge()
		{
			NewItems = UserBadgesManager.Instance.WhichItemsisNew(UserBadgesManager.BadgeTypes.Inventory, UserManager.UserModelInfo.GetEquipmentOfType(TypeEquipment));
			Badge.Visible = NewItems > 0;
			BadgeLabel.Text = NewItems.ToString();
		}

		public void ResetBadge()
		{
			Badge.Visible = false;
		}

		public void Select()
		{
			if (!inited)
			{
				Init();
			}
			if (CurrentlySelected != null)
			{
				if (CurrentlySelected == this)
				{
					return;
				}
				CurrentlySelected.Unselect();
			}
			CurrentlySelected = this;
			ResetBadge();
		}

		public void Unselect()
		{
			if (!inited)
			{
				Init();
			}
		}
	}
}
