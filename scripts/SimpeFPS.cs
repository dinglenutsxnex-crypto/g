using Godot;

public partial class SimpeFPS : Control
{
	[Export]
	public float refreshRate = 0.5f;
	[Export]
	public Label text;
	[Export]
	public Color Good;
	[Export]
	public Color Norm;
	[Export]
	public Color Low;

	private int counter;
	private float timer;

	public override void _Ready()
	{
		if (text == null)
		{
			text = GetNode<Label>(".");
		}
	}

	public override void _Process(double delta)
	{
		if (text == null)
		{
			return;
		}
		timer += (float)delta;
		counter++;
		if (timer >= refreshRate)
		{
			float num = (float)counter / refreshRate;
			if (num > 50f)
			{
				text.Modulate = Good;
			}
			else if (num > 20f)
			{
				text.Modulate = Norm;
			}
			else
			{
				text.Modulate = Low;
			}
			text.Text = "FPS: " + num.ToString("00.0");
			timer = 0f;
			counter = 0;
		}
	}
}
