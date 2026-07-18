// Stub: Unity NGUI UIButton compatibility shim
using Godot;

public partial class UIButton : Control
{
    public EventDelegateList onClick = new EventDelegateList();

    // UIButtonColor.State enum used in InventoryManager
    public UIButtonColor.State state { get; set; }
}

public static class UIButtonColor
{
    public enum State { Normal, Hover, Pressed, Disabled }
}
