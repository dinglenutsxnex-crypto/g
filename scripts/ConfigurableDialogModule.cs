using System;
using Godot;

public partial class ConfigurableDialogModule : NekkiUIModule
{
	public delegate void DialogOpened(DialogConfig config);

	public static DialogOpened onDialogOpened;

	public static void DialogWasOpened(DialogConfig config)
	{
		if (onDialogOpened != null)
		{
			onDialogOpened(config);
		}
	}
}
