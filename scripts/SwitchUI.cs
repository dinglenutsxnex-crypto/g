using Godot;

public partial class SwitchUI : Node
{
	[Export] public Node UIRoot;
	[Export] public Node UnityUI;

	private bool unityUIActive;

	public override void _Ready()
	{
		unityUIActive = true;
		ChangeUI();
	}

	public void ChangeUI()
	{
		unityUIActive = !unityUIActive;
		if (UIRoot != null)
			UIRoot.Visible = !unityUIActive;
		if (UnityUI != null)
			UnityUI.Visible = unityUIActive;
	}
}
