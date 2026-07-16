using System;
using Godot;

public static class ConstantsSF3
{
	public const string VERSION = "1.0.0";
	public const int MAX_PLAYER_LEVEL = 100;
	public const int MAX_PERK_SLOTS = 4;
	public const int MAX_EQUIPMENT_SLOTS = 5;
	public const float SHADOW_ENERGY_MAX = 100f;

	public enum ELocationSceneModule
	{
		None = 0,
		DojoInterface,
		Fight,
		Shop,
		Inventory,
		Map,
		Boosterpack,
		Settings,
		Tutorial,
		Brawler,
		DailyBattle,
		Survival,
		Quest,
		Character,
		Perks,
		Equipment,
		Items,
		BattlePass,
		Event,
		Loading,
		Login,
		MainMenu,
		Result,
		Selection,
		Splash,
		WarriorSelection,
		ZoneSelection
	}

	public enum DecorationType
	{
		None,
		Pattern,
		Sticker,
		Color,
		Skin,
		Effect,
		Accessory
	}

	public static string GetVersion()
	{
		return VERSION;
	}
}
