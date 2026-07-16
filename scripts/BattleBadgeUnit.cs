using Godot;
using System;

public class BattleBadgeUnit : Node
{
	private UISprite _sprite;
	private Label _label;

	public int Id { get; private set; }

	public event Action<bool> onUpdateState;

	public override void _ExitTree()
	{
		if (UserBadgesManager.Instance != null)
		{
			UserBadgesManager.Instance.UnregisterUnit(this);
		}
	}

	public void SetId(int idBattle)
	{
		Id = idBattle;
		UserBadgesManager.Instance.RegisterUnit(this);
	}

	public void Refresh(bool show)
	{
		Visible = show;
		onUpdateState?.Invoke(show);
	}

	public void SetDepth(int depth)
	{
		// Z-index equivalent in Godot
	}
}
