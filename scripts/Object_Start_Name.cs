using Godot;
using System.Threading.Tasks;

public partial class Object_Start_Name : Node
{
	public float StartTime;

	public Node myObject;

	private bool ifStart;

	public override void _Ready()
	{
		_ = Example();
	}

	private async Task Example()
	{
		await ToSignal(GetTree().CreateTimer(StartTime), SceneTreeTimer.SignalName.Timeout);
		ifStart = true;
	}

	public override void _Process(double delta)
	{
		if (ifStart)
		{
			myObject.Visible = true;
		}
	}
}
