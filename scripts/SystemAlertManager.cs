using Nekki.Yaml;
using System;

public static partial class SystemAlertManager
{
	private static Action _callback;

	internal static void Show(Mapping configSource, Action callback)
	{
		_callback = callback;
		SystemAlertConfig systemAlertConfig = new SystemAlertConfig(configSource);
		SystemMessage systemMessage = SystemMessage.ShowAlert(systemAlertConfig.labels[0].Alias, systemAlertConfig.title.Alias);
		foreach (ButtonConfig button in systemAlertConfig.buttons)
		{
			systemMessage.AddButton(button.Alias, OnSystemAlertClose);
		}
		systemMessage.Show();
	}

	private static void OnSystemAlertClose()
	{
		if (_callback != null)
		{
			_callback();
		}
	}
}
