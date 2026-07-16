using Godot;

public partial class TrailAnimatedTexture : Node
{
	[Export] public int colCount = 2;
	[Export] public int rowCount = 6;
	[Export] public int rowNumber;
	[Export] public int colNumber;
	[Export] public int totalCells = 12;
	[Export] public int fps = 16;
	[Export] public bool loop;
	[Export] public int tile = 1;

	private float StartTime;
	private Material material;

	public override void _Ready()
	{
		Node3D trail = GetNode<Node3D>(".");
		if (trail != null)
			material = trail.MaterialOverride;
		StartTime = Engine.GetProcessTime();
	}

	public override void _Process(double delta)
	{
		SetSpriteAnimation(colCount, rowCount, rowNumber, colNumber, totalCells, fps, loop, tile);
	}

	private void SetSpriteAnimation(int colCount, int rowCount, int rowNumber, int colNumber, int totalCells, int fps, bool loop, int tile)
	{
		if (material == null) return;

		float time = Engine.GetProcessTime() - StartTime;
		int frame = (int)(time * fps);

		if (!loop)
		{
			if (frame < totalCells)
			{
				frame %= totalCells;
				float x = 1f / colCount;
				float y = 1f / rowCount;
				Vector2 scale = new Vector2(x, y);
				Vector2 offset = new Vector2((frame % colCount + colNumber) * scale.X, 1f - scale.Y - (frame / colCount + rowNumber) * scale.Y);
				GD.Print("SetSpriteAnimation non-loop - port SetShaderParameter for UV scale/offset");
			}
		}
		else
		{
			frame %= totalCells;
			float x = 1f / colCount;
			float y = 1f / rowCount;
			Vector2 scale = new Vector2(x, y);
			Vector2 offset = new Vector2((frame % colCount + colNumber) * scale.X, 1f - scale.Y - (frame / colCount + rowNumber) * scale.Y);
			GD.Print("SetSpriteAnimation loop - port SetShaderParameter for UV scale/offset");
		}
	}
}
