using System;
using Nekki;
using Godot;
public static class BootLogger
{
	private static string _file;
	private static string AllocatedMemory
	{
		get
		{
			return ((float)OS.GetStaticMemoryUsage() / 1024f / 1024f).ToString("0.0");
		}
	}
	private static string UsedMemory
	{
		get
		{
			return ((float)OS.GetDynamicMemoryUsage() / 1024f / 1024f).ToString("0.0");
		}
	}
	public static void Init()
	{
		if (NekkiUtils.IsDebug)
		{
			_file = string.Format("{0}/BootLog_{1}.log", GlobalPath.LogsPath, NekkiUtils.CurrentTimeAsString);
			FilesUtil.AppendFileText(_file, string.Format("Device            : {0}\n", OS.GetModelName()));
			FilesUtil.AppendFileText(_file, string.Format("ProcessorCount    : {0}\n", OS.GetProcessorCount()));
			FilesUtil.AppendFileText(_file, string.Format("System memory     : {0}\n", OS.GetStaticMemoryUsage()));
			FilesUtil.AppendFileText(_file, "\n");
		}
	}
	public static void Call(Action action, string name)
	{
		DateTime now = DateTime.Now;
		action();
		if (NekkiUtils.IsDebug)
		{
			double totalSeconds = (DateTime.Now - now).TotalSeconds;
			string text = string.Format("[Time:{0:0.0000} UsedMemory:{1}, AllocatedMemory:{2}] {3}\n", totalSeconds, UsedMemory, AllocatedMemory, name);
			FilesUtil.AppendFileText(_file, text);
			GD.Print("BootLogger Call " + text);
		}
	}
	public static void Write(string log)
	{
		if (NekkiUtils.IsDebug)
		{
			FilesUtil.AppendFileText(_file, string.Format("{0}\n", log));
		}
	}
}
