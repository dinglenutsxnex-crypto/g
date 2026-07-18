using System.Collections.Generic;
using Godot;

public partial class NekkiUIRootModules : Node
{
	private static NekkiUIRootModules _instance;
	public static NekkiUIRootModules Instance => _instance;

	private readonly Dictionary<string, NekkiUIModule> _modules = new Dictionary<string, NekkiUIModule>();

	public override void _Ready()
	{
		_instance = this;
		foreach (Node child in GetChildren())
		{
			if (child is NekkiUIModule module && !string.IsNullOrEmpty(module.ModuleName))
				_modules[module.ModuleName] = module;
		}
	}

	public NekkiUIModule GetModule(string name)
	{
		_modules.TryGetValue(name, out var module);
		return module;
	}

	public NekkiUIModule MountNGUIModule(string name)
	{
		if (!_modules.TryGetValue(name, out var module))
		{
			module = new NekkiUIModule { ModuleName = name };
			AddChild(module);
			_modules[name] = module;
		}
		module.Visible = true;
		return module;
	}

	public T MountNativeModule<T>(string type, object settings = null) where T : NekkiUIModule, new()
	{
		if (_modules.TryGetValue(type, out var existing) && existing is T typed)
		{
			typed.Visible = true;
			typed.Setup(settings);
			return typed;
		}
		T module = new T { ModuleName = type };
		AddChild(module);
		_modules[type] = module;
		module.Setup(settings);
		module.Visible = true;
		return module;
	}

	public NekkiUIModule MountNativeModule(string type, object settings = null)
	{
		return MountNGUIModule(type);
	}

	public void UnmountModule(string name)
	{
		if (_modules.TryGetValue(name, out var module))
			module.Visible = false;
	}

	public void UnmountModule(NekkiUIModule module)
	{
		if (module != null)
			module.Visible = false;
	}

	public void EnableModules(string[] names)
	{
		if (names == null) return;
		foreach (string name in names)
			if (_modules.TryGetValue(name, out var module))
				module.Visible = true;
	}

	public void DisableModules(string[] names)
	{
		if (names == null) return;
		foreach (string name in names)
			if (_modules.TryGetValue(name, out var module))
				module.Visible = false;
	}
}
