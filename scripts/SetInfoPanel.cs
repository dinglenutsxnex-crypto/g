using Godot;

public partial class SetInfoPanel : Node
{
	public bool Active
	{
		get { return Visible; }
	}

	public void DisablePanel()
	{
		Visible = false;
	}

	public void ActivatePanel()
	{
		Visible = true;
	}
}
