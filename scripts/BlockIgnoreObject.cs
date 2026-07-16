using Godot;

public class BlockIgnoreObject
{
	private Node go;

	private bool pointerIn;

	private bool beginDrag;

	private bool isPressed;

	public bool Empty
	{
		get
		{
			return go == null;
		}
	}

	public BlockIgnoreObject(Node ignoredObject)
	{
		go = ignoredObject;
		pointerIn = false;
		beginDrag = false;
	}

	public bool Contains(Vector2 point)
	{
		if (go == null)
		{
			return false;
		}
		if (go is Control c)
		{
			Rect2 rect = c.GetGlobalRect();
			return rect.HasPoint(point);
		}
		return false;
	}

	public void CheckPointerEnterOrExit(Vector2 point)
	{
		bool flag = Contains(point);
		if (pointerIn != flag)
		{
			pointerIn = flag;
			// Godot equivalent: fire mouse_enter/mouse_exit via input handling
		}
	}

	public void Execute<T>(InputEvent data, System.Func<Node, InputEvent, bool> functor) where T : class
	{
		if (go == null)
		{
			return;
		}
		// Porting Unity's ExecuteEvents to Godot requires custom input routing
	}
}
