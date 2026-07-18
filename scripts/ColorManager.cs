using Godot;
using System;
using System.Linq;

public partial class ColorManager : Node
{
	private ColorPreset[] skinColors;

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
