using Godot;
using System.Collections.Generic;

public partial class NekkiUIModule : ExtentionBehaviour
{
	private NekkiUIElements _elements;
	private bool _visible = true;

	public Vector3 Position;
	public Vector3 Scale;
	public Vector3 Rotation;
	public string ModuleName;
	public int Layer { get; set; }

	public bool Visible
	{
		get => _visible;
		set
		{
			_visible = value;
			// Apply to self if this node is a CanvasItem (2D) or Node3D (3D)
			if (this is CanvasItem ci) ci.Visible = value;
			else if (this is Node3D n3) n3.Visible = value;
		}
	}

	public NekkiUIElements Elements
	{
		get
		{
			if (_elements == null)
				_elements = FindChild("NekkiUIElements", true, false) as NekkiUIElements
				            ?? GetComponent<NekkiUIElements>();
			return _elements;
		}
	}

	/// <summary>
	/// Unity-style GetComponent: checks self, then direct children.
	/// </summary>
	public new T GetComponent<T>() where T : class
	{
		if (this is T self) return self;
		foreach (Node child in GetChildren())
			if (child is T c) return c;
		return null;
	}

	/// <summary>
	/// Unity-style GetComponentsInChildren: populates list with all matching nodes in the subtree.
	/// </summary>
	public new void GetComponentsInChildren<T>(List<T> results) where T : Node
	{
		foreach (Node child in GetChildren())
		{
			if (child is T t) results.Add(t);
			foreach (Node grandchild in child.GetChildren())
				if (grandchild is T gt) results.Add(gt);
		}
	}

	public virtual void Setup(object settings) { }
	public virtual void UpdateModule() { }

	public override string ToString()
	{
		return string.Format("Modul [{0}]", ModuleName);
	}
}
