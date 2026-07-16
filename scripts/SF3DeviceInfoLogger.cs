using Newtonsoft.Json.Linq;
using SF3;
using Godot;
public partial class SF3DeviceInfoLogger
{
	private static SF3DeviceInfoLogger _logger;
	public static SF3DeviceInfoLogger instance
	{
		get
		{
			if (_logger == null)
			{
				_logger = new SF3DeviceInfoLogger();
			}
			return _logger;
		}
	}
	public void SendLog()
	{
		JObject jObject = new JObject();
		jObject["device"] = OS.GetModelName();
		jObject["OS"] = OS.GetName();
		jObject["RAM"] = OS.GetStaticMemoryUsage();
		jObject["CPU"] = OS.GetProcessorCount();
		jObject["screenWidth"] = DisplayServer.WindowGetSize().X;
		jObject["screenHeight"] = DisplayServer.WindowGetSize().Y;
		jObject["qualityPreset"] = QualityManager.PresetName;
		jObject["systemLanguage"] = OS.GetLocale();
		jObject["subtype"] = "device_info";
		Analytics.Logger.AddEvent("APPLICATION_OPEN", jObject);
	}
}
