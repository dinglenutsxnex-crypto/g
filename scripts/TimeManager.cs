using System;
using System.Collections.Generic;
using System.Text;
using Nekki.Utils;
using SF3.Moves;
using Godot;

public partial class TimeManager : Node
{
	public partial class TimerUnit
	{
		public DateTime Expire;
		public string Name;

		public TimerUnit(string source)
		{
			string[] array = source.Split('|');
			Name = array[0];
			Expire = DateTime.FromBinary(long.Parse(array[1]));
		}

		public TimerUnit(string name, int seconds)
		{
			Name = name;
			Expire = DateTime.Now + TimeSpan.FromSeconds(seconds);
		}

		public string Serialize()
		{
			return string.Format("{0}|{1}", Name, Expire.ToBinary());
		}
	}

	private static TimeManager _instance;
	private static List<TimerUnit> _timers = new List<TimerUnit>();

	public static TimeManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new TimeManager();
				_instance.Name = "_timerManager";
				StaticObjectsManager.AddObject(_instance);
				_instance.Load();
			}
			return _instance;
		}
	}

	private static string DataPath
	{
		get
		{
			return string.Format("{0}/Config", GlobalPath.ExternalPath);
		}
	}

	private static string DataFile
	{
		get
		{
			return string.Format("{0}/timers.dat", DataPath);
		}
	}

	public static long GetUTC()
	{
		return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
	}

	public static long GetLocalMS()
	{
		return (GlobalTimer.Now.Ticks - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks) / 10000;
	}

	private void Save()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < _timers.Count; i++)
		{
			stringBuilder.AppendLine(_timers[i].Serialize());
		}
		FilesUtil.WriteFileText(DataFile, stringBuilder.ToString());
	}

	private void Load()
	{
		string[] array = FilesUtil.ReadFileLines(DataFile);
		if (array != null)
		{
			foreach (string source in array)
			{
				_timers.Add(new TimerUnit(source));
			}
		}
	}

	public void Add(string timerName, int seconds)
	{
		Remove(timerName);
		_timers.Add(new TimerUnit(timerName, seconds));
		Save();
	}

	public void Remove(string timerName)
	{
		foreach (TimerUnit timer in _timers)
		{
			if (timer.Name.Equals(timerName))
			{
				Remove(timer);
				break;
			}
		}
	}

	public void Remove(TimerUnit timer)
	{
		_timers.Remove(timer);
		Save();
	}

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	public void Init()
	{
	}
}
