using Godot;

public class NekkiUIModule : ExtentionBehaviour
{
	private NekkiUIElements _elements;

	public Vector3 Position;
	public Vector3 Scale;
	public Vector3 Rotation;
	public string ModuleName;
	public int Layer { get; set; }

	public NekkiUIElements Elements
	{
		get
		{
			if (_elements == null)
				_elements = GetNode<NekkiUIElements>();
			return _elements;
		}
	}

	public virtual void Setup(object settings) { }
	public virtual void UpdateModule() { }

	public override string ToString()
	{
		return string.Format("Modul [{0}]", ModuleName);
	}
}
