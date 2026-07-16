using Godot;
using System.Threading.Tasks;

public partial class Object_End_Name : Node
{
	public float EndTime;

	public Node myObject;

	private bool ifStart;

	public override void _Ready()
	{
		_ = Example();
	}

	private async Task Example()
	{
		await ToSignal(GetTree().CreateTimer(EndTime), SceneTreeTimer.SignalName.Timeout);
		ifStart = true;
	}

	public override void _Process(double delta)
	{
		if (ifStart)
		{
			myObject.Visible = false;
		}
	}
}
