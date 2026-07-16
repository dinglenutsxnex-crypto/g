using System.Collections.Generic;
using Godot;

public partial class TiledMap : Node
{
	[Export]
	private string tilePrefix = "map_";

	[Export]
	private int tileWidth = 2048;

	[Export]
	private int tileHeight = 2048;

	[Export]
	private string pathToTile = "UI/Map";

	private Control widget;

	private List<TextureRect> tiles;

	private int fillX;

	private int fillY;

	public override void _Ready()
	{
		widget = GetNode<Control>(".");
		if (widget == null)
		{
			GD.PrintErr("Control node not found.");
			return;
		}
		tiles = new List<TextureRect>();
		Tiled();
	}

	private void Tiled()
	{
		if (tiles.Count > 0)
		{
			Clear();
		}
		float num = Mathf.Ceil(widget.Size.X / (float)tileWidth);
		float num2 = Mathf.Ceil(widget.Size.Y / (float)tileHeight);
		int num3 = 0;
		Vector2 size = widget.Size;
		Vector2 topLeft = -size / 2f;
		for (int i = 0; (float)i < num2; i++)
		{
			for (int j = 0; (float)j < num; j++)
			{
				int w = (fillX + tileWidth > (int)widget.Size.X) ? ((int)widget.Size.X - fillX) : tileWidth;
				int h = (fillY + tileHeight > (int)widget.Size.Y) ? ((int)widget.Size.Y - fillY) : tileHeight;
				TextureRect tile = CreateTileObject(num3, w, h);
				tile.Position = new Vector2(topLeft.X + (tileWidth * j), topLeft.Y + (tileHeight * i));
				num3++;
				fillX += w;
			}
			fillY += h;
		}
	}

	private void Clear()
	{
		foreach (TextureRect tile in tiles)
		{
			if (tile.Texture != null)
			{
				tile.Texture = null;
			}
			tile.QueueFree();
		}
		fillX = 0;
		fillY = 0;
	}

	private TextureRect CreateTileObject(int tileNumber, int width, int height)
	{
		string text = tileNumber.ToString();
		TextureRect tile = new TextureRect();
		tile.Name = "tile_" + text;
		Texture2D tex = GD.Load<Texture2D>(pathToTile + "/" + tilePrefix + text);
		if (tex != null)
		{
			tex.ResourceLocalToScene = true;
		}
		tile.Texture = tex;
		tile.Size = new Vector2(width, height);
		tile.StretchMode = TextureRect.StretchModeEnum.Keep;
		tiles.Add(tile);
		AddChild(tile);
		tile.Owner = GetTree().EditedSceneRoot;
		return tile;
	}
}
