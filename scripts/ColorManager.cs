using Godot;
using System;
using System.Linq;

public class ColorManager : Node
{
	[Export]
	private ColorPreset[] skinColors;

	[Export]
	private ColorPreset[] hairColors;

	public static ColorManager Instance;

	public override void _Ready()
	{
		Instance = this;
	}

	public ColorPreset GetSkinColor(int id)
	{
		return skinColors.FirstOrDefault((ColorPreset x) => x.ID == id);
	}

	public ColorPreset GetHairColor(int id)
	{
		return hairColors.FirstOrDefault((ColorPreset x) => x.ID == id);
	}
}
