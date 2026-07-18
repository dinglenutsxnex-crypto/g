using System;
using System.Collections.Generic;
using Godot;
using Nekki;
using Nekki.UI;
using SF3;
using SF3.UserData;

public partial class BaseModuleController : NekkiUIModule
{
	[Export]
	private NekkiUIModule _currentModule;

	private static BaseModuleController _instance;

	private static Queue<BaseModuleController> _history;

	public static string CurrentName
	{
		get
		{
			if (!(_instance != null))
			{
				return string.Empty;
			}
			return _instance._currentModule.Name;
		}
	}

	public static BaseModuleController Instance
	{
		get
		{
			return _instance;
		}
	}

	public override void _Ready()
	{
		base._Ready();
		_instance = this;
		_history = new Queue<BaseModuleController>();
	}

	public void GoToModule(NekkiUIModule module)
	{
		if (module != null)
		{
			_history.Enqueue(this);
			_currentModule = module;
			_currentModule.Visible = true;
		}
	}

	public void GoBack()
	{
		if (_history.Count > 0)
		{
			BaseModuleController previousModule = _history.Dequeue();
			_currentModule.Visible = false;
			_currentModule = previousModule;
			_currentModule.Visible = true;
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (_instance == this)
		{
			_instance = null;
		}
	}
}
