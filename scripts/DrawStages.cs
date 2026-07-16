using Godot;

[Tool]
public partial class DrawStages : Node3D
{
	[Export]
	public float Radius;

	[Export]
	public int StageNumber;

	[Export]
	public int MaxInRow = 3;

	[Export]
	public Vector2 Indent;

	[Export]
	public bool Draw;

	public override void _Ready()
	{
		if (!NekkiUtils.IsDebug)
			QueueFree();
	}

	public override void _Process(double delta)
	{
		if (!Draw) return;
		float rows = Mathf.Ceil((float)StageNumber / (float)MaxInRow);
		Vector3 zero = Vector3.Zero;
		if (StageNumber > 1)
		{
			if (StageNumber < MaxInRow)
				zero.X = -Indent.X * ((float)StageNumber / 2f);
			else
				zero.X = -Indent.X * ((float)MaxInRow / 2f);
			zero.X += Indent.X / 2f;
		}
		if (rows > 1f)
		{
			zero.Y = -Indent.Y * (rows / 2f);
			zero.Y += Indent.Y / 2f;
		}
		int count = 0;
		for (int i = 0; i < rows; i++)
		{
			int remaining = StageNumber - count;
			if (remaining < MaxInRow)
			{
				zero.X = 0f;
				if (remaining > 1)
				{
					if (remaining < MaxInRow)
						zero.X = -Indent.X * ((float)remaining / 2f);
					zero.X += Indent.X / 2f;
				}
			}
			for (int j = 0; j < MaxInRow; j++)
			{
				Vector3 pos = zero + new Vector3(j * Indent.X, i * Indent.Y, 0f);
				pos.X *= Scale.X;
				pos.Y *= Scale.Y;
				pos.Z *= Scale.Z;
				DebugDrawSphere(GlobalPosition + pos, Radius);
				count++;
				if (count == StageNumber) return;
			}
		}
	}

	private void DebugDrawSphere(Vector3 center, float radius)
	{
		// Debug draw using ImmediateMesh or DebugDraw plugin
	}
}
