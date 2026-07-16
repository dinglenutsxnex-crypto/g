using Godot;

[Tool]
public partial class AdvancedUIScrollBar : ScrollBar
{
	public bool disableKeys;

	public override void _Input(InputEvent @event)
	{
		if (!disableKeys)
		{
			base._Input(@event);
		}
	}
}
