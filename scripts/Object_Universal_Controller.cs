using Godot;
using System.Threading.Tasks;

public partial class Object_Universal_Controller : Node3D
{
	[Export] public float StartTime;
	[Export] public float MoveSpeedX;
	[Export] public float MoveSpeedY;
	[Export] public float MoveSpeedZ;
	[Export] public float RotateSpeedX;
	[Export] public float RotateSpeedY;
	[Export] public float RotateSpeedZ;
	[Export] public float ScaleSpeedX;
	[Export] public float ScaleSpeedY;
	[Export] public float ScaleSpeedZ;
	[Export] public float MaterialFadeIn;
	[Export] public float MaterialLifeTime;
	[Export] public float MaterialFadeOut;

	private bool _ifStart;

	public override void _Ready()
	{
		_ = Example();
	}

	private async Task Example()
	{
		await ToSignal(GetTree().CreateTimer(StartTime), "timeout");
		_ifStart = true;
		await ToSignal(GetTree().CreateTimer(MaterialFadeIn + MaterialLifeTime), "timeout");
		_ifStart = false;
	}

	public override void _Process(double delta)
	{
		if (_ifStart)
		{
			Translate(new Vector3(MoveSpeedX * (float)delta, MoveSpeedY * (float)delta, MoveSpeedZ * (float)delta));
			Rotate(new Vector3(RotateSpeedX * (float)delta, RotateSpeedY * (float)delta, RotateSpeedZ * (float)delta));
			Scale += new Vector3(ScaleSpeedX * (float)delta, ScaleSpeedY * (float)delta, ScaleSpeedZ * (float)delta);
		}
	}
}