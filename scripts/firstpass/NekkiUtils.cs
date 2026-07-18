using System;
using Godot;

public static class NekkiUtils
{
	public static bool IsDebug => OS.IsDebugBuild();

	public static bool IsPhone()
	{
		string platform = OS.GetName();
		return platform == "Android" || platform == "iOS";
	}

	public static DateTime GetUnixDateTimeFromMilliseconds(long ms)
	{
		return DateTimeOffset.FromUnixTimeMilliseconds(ms).LocalDateTime;
	}

	public static string CurrentTimeAsString => DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

	public static Color HexToColor(string hex)
	{
		return Color.FromHtml(hex);
	}

	public static void ClearAllApplication()
	{
		Engine.GetMainLoop().Quit();
	}
}
