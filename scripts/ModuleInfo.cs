using Godot;
using System;

[Serializable]
public class ModuleInfo
{
	public string moduleName;
	public bool visibleAtStart;
	public Button moduleBtn;
	private Node moduleObj;

	public void Init()
	{
		if (!NekkiUtils.IsDebug) return;
		moduleObj = NekkiUIRootModules.Instance?.MountNGUIModule(moduleName) as Node;
		if (moduleObj != null)
		{
			moduleBtn.Pressed += () => moduleObj.Call("ShowHide");
			if (!visibleAtStart)
				moduleObj.Call("ShowHide");
		}
	}
}
