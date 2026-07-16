using System.Collections.Generic;
using Godot;
using Node = Godot.Node;
using Nekki;
using Nekki.Yaml;
using Network.core.events;
using SF3;
using SF3.BattleUtils;
using SF3.UserData;

public class DebugUI : UIModuleHolder
{
	private enum DebugUIStates
	{
		Short = 0,
		Full = 1
	}

	[Export]
	private MultiTweenTransition _multiTween;

	[Export]
	private Label fpsLabelShort;

	[Export]
	private Button _expand;

	[Export]
	private Button _collapse;

	[Export]
	private Button _devServerChooserClose;

	[Export]
	private Color FPSGood;

	[Export]
	private Color FPSNorm;

	[Export]
	private Color FPSLow;

	[Export]
	private Label ConsoleTextOutput;

	[Export]
	private Node fullGO;

	[Export]
	private Node listContainerPrefab;

	[Export]
	private Node serverSelectPrefab;

	[Export]
	private DebugDevelopmentServerChooser devServerChooser;

	private List<DebugListContainerController> lineContainers = new List<DebugListContainerController>();

	private int currentContainerID;

	private DebugUIStates state;

	private static DebugUI _instance;

	private float nextUpdateTime;

	private static float _lastObjRequest = -1f;

	private static int _lastObjectsCount;

	private static float _lastDrawcallsRequest = -1f;

	private static int _lastDrawcallsCount;

	private MultiTweenTransition _devChooserTween;

	private bool _devChooserShowed;

	public static string MemoryInfo
	{
		get
		{
			return string.Format("mem: [a {0} r {1}] / dv {2} dg {3}]", GetAllocatedMemory, GetReservedMemory, "0.0", "0.0");
		}
	}

	public static string FpsInfo
	{
		get
		{
			if (_instance == null)
			{
				_instance = new DebugUI();
				_instance.Name = "_debugInfo";
			}
			return SF3BattleUtils.GetFPS().ToString("0.0");
		}
	}

	public static float FpsInfoFloat
	{
		get
		{
			if (_instance == null)
			{
				_instance = new DebugUI();
				_instance.Name = "_debugInfo";
			}
			return SF3BattleUtils.GetFPS();
		}
	}

	public static string FullInfo
	{
		get
		{
			return string.Format("{0} {1} {2} {3}", FpsInfo, MemoryInfo, GetDrawCalls, GetObjects);
		}
	}

	public static string GetDrawCalls
	{
		get
		{
			if (_lastDrawcallsRequest + 1f < GameTimeController.time)
			{
				_lastDrawcallsRequest = GameTimeController.time;
				_lastDrawcallsCount = 1;
			}
			return string.Empty + _lastDrawcallsCount;
		}
	}

	public static string GetObjects
	{
		get
		{
			if (_lastObjRequest + 1f < GameTimeController.time)
			{
				_lastObjRequest = GameTimeController.time;
				_lastObjectsCount = 0;
			}
			return string.Empty + _lastObjectsCount;
		}
	}

	public static string GetAllocatedMemory
	{
		get
		{
			return "0.0";
		}
	}

	public static string GetSystemMemory
	{
		get
		{
			return "0.0";
		}
	}

	public static string GetGraphicsMemory
	{
		get
		{
			return "0.0";
		}
	}

	public static string GetReservedMemory
	{
		get
		{
			return "0.0";
		}
	}

	private static string GetConnectionStatus
	{
		get
		{
			return (!NetworkConnection.current.IsNetworkEstablished()) ? "Cut off" : "Established";
		}
	}

	public static void RecursiveMappingOpening(Nekki.Yaml.Node node)
	{
		if (node is Mapping)
		{
			GD.Print(">> " + (string)node.Get("key"));
			{
				foreach (Nekki.Yaml.Node item in (System.Collections.IEnumerable)((Mapping)node).Get("nodesInside"))
				{
					RecursiveMappingOpening(item);
				}
				return;
			}
		}
		if (node is Sequence)
		{
			GD.Print("--- " + (string)node.Get("key"));
			{
				foreach (Nekki.Yaml.Node item2 in (System.Collections.IEnumerable)((Sequence)node).Get("nodesInside"))
				{
					RecursiveMappingOpening(item2);
				}
				return;
			}
		}
		if (node is Scalar)
		{
			GD.Print((string)node.Get("key") + " = " + (string)node.Get("value"));
		}
	}

	public override void _Ready()
	{
		base._Ready();
		_instance = this;
	}

	public override void _EnterTree()
	{
		base._EnterTree();
		_expand.Pressed += Full;
		_collapse.Pressed += Short;
		_devServerChooserClose.Pressed += ToggleDevServerChooser;
		_devChooserTween = fullGO.GetNode<MultiTweenTransition>(new NodePath("MultiTweenTransition"));
		_multiTween.TweenOut();
		Clear();
		nextUpdateTime = (float)Engine.GetProcessTime() + 0.5f;
		DebugListContainerController debugListContainerController = AddListContainer("SYSTEM");
		debugListContainerController.AddLine("FPS", updateFPS);
		debugListContainerController.AddLine("Memory reserved", delegate(DebugTextLineController line)
		{
			line.value.Text = string.Format("{0} Mb", GetReservedMemory);
		});
		debugListContainerController.AddLine("Memory allocated", delegate(DebugTextLineController line)
		{
			line.value.Text = string.Format("{0} Mb", GetAllocatedMemory);
		});
		debugListContainerController.AddLine("System memory", delegate(DebugTextLineController line)
		{
			line.value.Text = string.Format("{0} Mb", GetSystemMemory);
		});
		debugListContainerController.AddLine("Graphics memory", delegate(DebugTextLineController line)
		{
			line.value.Text = string.Format("{0} Mb", GetGraphicsMemory);
		});
		debugListContainerController.AddLine("Draw calls", delegate(DebugTextLineController line)
		{
			line.value.Text = GetDrawCalls;
		});
		debugListContainerController.AddLine("Objects", delegate(DebugTextLineController line)
		{
			line.value.Text = GetObjects;
		});
		debugListContainerController.AddLine(string.Empty, delegate(DebugTextLineController line)
		{
			line.value.Text = string.Empty;
		}).HideSeparator();
		debugListContainerController.AddButton("RESET", onResetButtonPress);
		DebugListContainerController debugListContainerController2 = AddListContainer("ONLINE");
		debugListContainerController2.AddLine(string.Empty, delegate(DebugTextLineController line)
		{
			line.value.Text = NetworkConnection.current.UserData.Auth.Login;
		}).HideSeparator();
		debugListContainerController2.AddLine("Player ID", delegate(DebugTextLineController line)
		{
			line.value.Text = UserManager.ServerPlayerID.ToString();
		});
		debugListContainerController2.AddLine("Ping", delegate(DebugTextLineController line)
		{
			line.value.Text = string.Format("{0}ms", SF3BattleUtils.GetPing());
		});
		debugListContainerController2.AddLine("Network Status", updateNetworkStatus);
		debugListContainerController2.AddLine("Inside Pocket", updatePocketStatus);
		debugListContainerController2.AddLine("Current Config ver", delegate(DebugTextLineController line)
		{
			line.value.Text = NetworkConnection.current.CurrentConfigVersion.full;
		});
		debugListContainerController2.AddLine("IP", updateServerIP).HideSeparator();
		debugListContainerController2.AddLine("Can Create Player", delegate(DebugTextLineController line)
		{
			line.value.Text = NetworkConnection.current.canCreatePlayer().ToString();
		});
		debugListContainerController2.AddLine("Current State", delegate(DebugTextLineController line)
		{
			line.value.Text = NetworkConnection.CurrentStateName;
		});
		AddServerLine(debugListContainerController2);
		DebugListContainerController debugListContainerController3 = AddListContainer("FIGHT");
		debugListContainerController3.AddLine("Module", delegate(DebugTextLineController line)
		{
			line.value.Text = BaseModuleController.CurrentName;
		});
		BattleInfo battle = BattlesManager.currentBattle.GetBattleInfo();
		debugListContainerController3.AddLine("BattleID", delegate(DebugTextLineController line)
		{
			line.value.Text = battle.id.ToString();
		});
		debugListContainerController3.AddLine("BattleName", delegate(DebugTextLineController line)
		{
			line.value.Text = battle.name;
		});
		debugListContainerController3.AddLine("FightID", delegate(DebugTextLineController line)
		{
			line.value.Text = (battle.GetCurrentFight().fightID + 1).ToString();
		});
		openContainerAtID(currentContainerID);
	}

	private void AddServerLine(DebugListContainerController online)
	{
		string name = InternalSettingsSF3.ForceBalancer;
		if (string.IsNullOrEmpty(name))
		{
			DebugCurrentServerController @object = online.AddLine(serverSelectPrefab, null) as DebugCurrentServerController;
			devServerChooser.DevServerChosen -= @object.OnDevServerChosen;
			devServerChooser.DevServerChosen += @object.OnDevServerChosen;
		}
		else
		{
			online.AddLine(string.Empty, delegate(DebugTextLineController line)
			{
				line.value.Text = "Forcing server: " + name;
				line.value.Modulate = Colors.Red;
			}).HideSeparator();
		}
	}

	private void updatePocketStatus(DebugTextLineController line)
	{
		bool insideDarkPocket = NetworkConnection.current.InsideDarkPocket;
		line.value.Text = string.Format("{0}", insideDarkPocket.ToString());
		if (insideDarkPocket)
		{
			line.value.Modulate = FPSNorm;
		}
		else
		{
			line.value.Modulate = FPSGood;
		}
	}

	private void onResetButtonPress()
	{
		NetworkConnection.Send("cheat_reset_player", null, delegate(NetworkEvent e)
		{
			if (!e.success)
			{
				GD.PrintErr("Error: Reset Player Failed");
			}
			NekkiUtils.ClearAllApplication();
		});
	}

	private void openContainerAtID(int ID)
	{
		foreach (DebugListContainerController lineContainer in lineContainers)
		{
			lineContainer.setVisible(false);
		}
		lineContainers[ID].setVisible(true);
	}

	public void onPrevListClick()
	{
		currentContainerID--;
		if (currentContainerID < 0)
		{
			currentContainerID = lineContainers.Count - 1;
		}
		openContainerAtID(currentContainerID);
	}

	public void onNextListClick()
	{
		currentContainerID++;
		if (currentContainerID >= lineContainers.Count)
		{
			currentContainerID = 0;
		}
		openContainerAtID(currentContainerID);
	}

	private void updateFPS(DebugTextLineController line)
	{
		Label value = line.value;
		value.Text = FpsInfo;
		if (FpsInfoFloat >= 58f)
		{
			value.Modulate = FPSGood;
		}
		else if (FpsInfoFloat >= 35f)
		{
			value.Modulate = FPSNorm;
		}
		else
		{
			value.Modulate = FPSLow;
		}
	}

	private void updateNetworkStatus(DebugTextLineController line)
	{
		line.value.Text = string.Format("{0}", GetConnectionStatus);
		line.value.Modulate = (line.value.Text != "Established" ? FPSLow : FPSGood);
	}

	private void updateServerName(DebugTextLineController line)
	{
		line.value.Text = "to be implemented";
	}

	private void updateServerIP(DebugTextLineController line)
	{
		line.value.Text = NetworkConnection.current.Server + ":" + NetworkConnection.current.TCPPort;
	}

	private DebugListContainerController AddListContainer(string title)
	{
		Node node = fullGO.Duplicate(7);
		DebugListContainerController component = node.GetNode<DebugListContainerController>(new NodePath("DebugListContainerController"));
		component.setup(title);
		component.setVisible(false);
		lineContainers.Add(component);
		return component;
	}

	public override void _Process(double delta)
	{
		if ((float)Engine.GetProcessTime() >= nextUpdateTime)
		{
			nextUpdateTime = (float)Engine.GetProcessTime() + 0.5f;
			UpdateInfo();
		}
	}

	public static void Log(string text)
	{
		if (_instance != null)
		{
			Label consoleTextOutput = _instance.ConsoleTextOutput;
			consoleTextOutput.Text = consoleTextOutput.Text + text + "\n";
		}
	}

	public static void Clear()
	{
		if (_instance != null && _instance.ConsoleTextOutput != null)
		{
			_instance.ConsoleTextOutput.Text = string.Empty;
		}
	}

	private void UpdateInfo()
	{
		switch (state)
		{
		case DebugUIStates.Short:
			fpsLabelShort.Text = FpsInfo;
			if (FpsInfoFloat >= 58f)
			{
				fpsLabelShort.Modulate = FPSGood;
			}
			else if (FpsInfoFloat >= 35f)
			{
				fpsLabelShort.Modulate = FPSNorm;
			}
			else
			{
				fpsLabelShort.Modulate = FPSLow;
			}
			break;
		case DebugUIStates.Full:
		{
			foreach (DebugListContainerController lineContainer in lineContainers)
			{
				lineContainer.UpdateLines();
			}
			break;
		}
		}
	}

	private void Short()
	{
		_multiTween.TweenOut();
		state = DebugUIStates.Short;
	}

	private void Full()
	{
		_multiTween.TweenIn();
		state = DebugUIStates.Full;
	}

	public void ToggleDevServerChooser()
	{
		_devChooserShowed = !_devChooserShowed;
		if (_devChooserShowed)
		{
			_devChooserTween.TweenOut();
		}
		else
		{
			_devChooserTween.TweenIn();
		}
	}
}
