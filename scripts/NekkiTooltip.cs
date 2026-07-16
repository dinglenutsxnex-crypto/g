using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class NekkiTooltip : Node
{
	[Export] private string _alias;

	public string[] LastReplacement;

	public List<ImageData> ImagesInfo;

	public int MaxSymbols = 30;

	public float ShowTime = 4f;

	public string Alias
	{
		get => _alias;
		set
		{
			_alias = value;
		}
	}

	public void SelectReplacementAtIndex(string source)
	{
		string[] array = source.Split('|');
		int num = int.Parse(array[1]);
		LastReplacement[num] = array[0];
	}

	public ImageData GetImageDataByID(int sectionID)
	{
		if (ImagesInfo == null) return null;
		foreach (var item in ImagesInfo)
		{
			if (item.sectionID == sectionID) return item;
		}
		var newData = new ImageData { sectionID = sectionID };
		ImagesInfo.Add(newData);
		return newData;
	}

	public async void Format(Label label, TextureRect back, Node2D arrow)
	{
		await FormatDelay(label, back, arrow);
	}

	private async System.Threading.Tasks.Task FormatDelay(Label label, TextureRect back, Node2D arrow)
	{
		for (int i = 0; i < 3; i++)
		{
			await ToSignal(GetTree(), "process_frame");
			Vector2 size = label.Size;
			back.Size = new Vector2(size.X + 50, size.Y + 20);
			float ARROW_LONG = 50f;
			float ARROW_SHORT = 40f;
			switch (arrow.Name)
			{
				case "0":
					arrow.Position = new Vector2(-back.Size.X / 2f, back.Size.Y / 2f);
					back.Position = new Vector2(back.Size.X / 2f, -back.Size.Y / 2f - ARROW_LONG);
					break;
				case "1":
					arrow.Position = new Vector2(0f, back.Size.Y / 2f);
					back.Position = new Vector2(0f, -back.Size.Y / 2f - ARROW_SHORT);
					break;
				case "2":
					arrow.Position = new Vector2(back.Size.X / 2f, back.Size.Y / 2f);
					back.Position = new Vector2(-back.Size.X / 2f, -back.Size.Y / 2f - ARROW_LONG);
					break;
				case "3":
					arrow.Position = new Vector2(-back.Size.X / 2f, 0f);
					back.Position = new Vector2(back.Size.X / 2f + ARROW_SHORT, 0f);
					break;
				case "4":
					arrow.Position = new Vector2(back.Size.X / 2f, 0f);
					back.Position = new Vector2(-back.Size.X / 2f - ARROW_SHORT, 0f);
					break;
				case "5":
					arrow.Position = new Vector2(-back.Size.X / 2f, -back.Size.Y / 2f);
					back.Position = new Vector2(back.Size.X / 2f, back.Size.Y / 2f + ARROW_LONG);
					break;
				case "6":
					arrow.Position = new Vector2(0f, -back.Size.Y / 2f);
					back.Position = new Vector2(0f, back.Size.Y / 2f + ARROW_SHORT);
					break;
				case "7":
					arrow.Position = new Vector2(back.Size.X / 2f, -back.Size.Y / 2f);
					back.Position = new Vector2(-back.Size.X / 2f, back.Size.Y / 2f + ARROW_LONG);
					break;
			}
			back.Visible = true;
			arrow.Visible = true;
		}
	}

	public override void _Ready()
	{
	}

	[Serializable]
	public class ImageData
	{
		public int sectionID;
	}
}