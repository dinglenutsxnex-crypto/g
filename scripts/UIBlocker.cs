using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class UIBlocker : Control
{
	public enum Priority
	{
		Min = 1,
		Dialogs = 100,
		Feedback = 200,
		Preloader = 300,
		LoginEmailDialog = 350,
		ServerSystemAlert = 400,
		Max = 1000
	}

	private static UIBlocker _instance;

	private static SortedDictionary<int, List<BlockIgnoreObject>> _ignoredObjects = new SortedDictionary<int, List<BlockIgnoreObject>>();

	private List<Node> _ignoredPanels = new List<Node>();

	private bool _inited;

	private int _blockCounter;

	public static UIBlocker Instance
	{
		get
		{
			if (_instance == null)
			{
				// NekkiUIRootModules equivalent - placeholder
			}
			return _instance;
		}
	}

	public bool IsLocked
	{
		get
		{
			return _blockCounter > 0;
		}
	}

	public override void _Ready()
	{
		_instance = this;
		Init();
	}

	private void Init()
	{
		if (!_inited)
		{
			MouseFilter = MouseFilterEnum.Ignore;
			_inited = true;
		}
	}

	public override void _Process(double delta)
	{
		foreach (BlockIgnoreObject currentIgnoredObject in GetCurrentIgnoredObjects())
		{
			currentIgnoredObject.CheckPointerEnterOrExit(GetGlobalMousePosition());
		}
	}

	private void AddIgnoreObjects(List<Node> ignoredObjs, int priorityIdx)
	{
		foreach (Node ignoredObj in ignoredObjs)
		{
			_ignoredObjects[priorityIdx].Add(new BlockIgnoreObject(ignoredObj));
		}
	}

	public void AddIgnorePanel(Node panel)
	{
		_ignoredPanels.Add(panel);
	}

	private List<BlockIgnoreObject> GetCurrentIgnoredObjects()
	{
		List<BlockIgnoreObject> list = _ignoredObjects.Values.LastOrDefault();
		if (list == null)
		{
			list = new List<BlockIgnoreObject>();
		}
		return list;
	}

	private int GetCurrentPriority()
	{
		return _ignoredObjects.Keys.LastOrDefault();
	}

	private void ClearIgnoreObjects(int priorityIdx)
	{
		_ignoredObjects.Remove(priorityIdx);
	}

	private void ClearIgnoredPanels()
	{
		_ignoredPanels.Clear();
	}

	public void Clear()
	{
		_ignoredObjects.Clear();
		ClearIgnoredPanels();
		_blockCounter = 0;
	}

	public void Block(List<Node> ignoredObj, Priority priority = Priority.Min)
	{
		int num = FindFreeIndex(priority);
		_ignoredObjects[num] = new List<BlockIgnoreObject>();
		AddIgnoreObjects(ignoredObj, num);
		_blockCounter++;
		Visible = true;
		MouseFilter = MouseFilterEnum.Stop;
	}

	public void Block(Node ignoredObj, Priority priority = Priority.Min)
	{
		List<Node> list = new List<Node>();
		list.Add(ignoredObj);
		Block(list, priority);
	}

	public void Block(Priority priority = Priority.Min)
	{
		List<Node> ignoredObj = new List<Node>();
		Block(ignoredObj, priority);
	}

	public void Unblock(Priority priority = Priority.Min)
	{
		if (!_ignoredObjects.ContainsKey((int)priority))
		{
			GD.PushWarning("Unblock empty layer");
			return;
		}
		_blockCounter--;
		if (_blockCounter < 0)
		{
			GD.PushWarning("More Unblock called than Block");
			_blockCounter = 0;
			return;
		}
		int priorityIdx = FindFreeIndex(priority) - 1;
		ClearIgnoreObjects(priorityIdx);
		if (_blockCounter == 0)
		{
			ClearIgnoredPanels();
			Visible = false;
			MouseFilter = MouseFilterEnum.Ignore;
		}
	}

	private int FindFreeIndex(Priority priority)
	{
		int i;
		for (i = (int)priority; _ignoredObjects.ContainsKey(i); i++)
		{
		}
		return i;
	}
}
