// Stub: Unity NGUI EventDelegate compatibility shim
using System;

/// <summary>
/// Minimal NGUI EventDelegate shim. Wraps an Action so NGUI-style
/// onClick.Add(new EventDelegate(...)) calls compile.
/// </summary>
public class EventDelegate
{
    private readonly Action _action;

    public EventDelegate(Action callback) { _action = callback; }
    public EventDelegate(Action<object[]> callback) { }

    public void Execute() => _action?.Invoke();

    // Implicit conversion from Action so bare Add(MyMethod) works too
    public static implicit operator EventDelegate(Action a) => new EventDelegate(a);
}

/// <summary>Minimal List<EventDelegate> wrapper used by UIButton.onClick</summary>
public class EventDelegateList : System.Collections.Generic.List<EventDelegate>
{
    public void Execute()
    {
        foreach (var d in this) d.Execute();
    }
}
