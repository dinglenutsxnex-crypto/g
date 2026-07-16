using System;
using System.Collections.Generic;

[Serializable]
public class DeviceQuality
{
    public const float TABLET_DPI = 6.5f;
    public const float TABLET_RESOLUTION = 768f;

    [NonSerialized]
    public string name;

    public string presetName;

    public QualityDeviceTypes type;

    [NonSerialized]
    public DevicePreset preset;

    private static Dictionary<string, QualityDeviceTypes> _types;

    public static Dictionary<string, QualityDeviceTypes> types
    {
        get
        {
            if (_types == null)
            {
                _types = new Dictionary<string, QualityDeviceTypes>
                {
                    { "phone", QualityDeviceTypes.PHONE },
                    { "tablet", QualityDeviceTypes.TABLET },
                    { "console", QualityDeviceTypes.CONSOLE },
                    { "standalone", QualityDeviceTypes.STANDALONE }
                };
            }
            return _types;
        }
    }

    public DeviceQuality()
    {
        name = "unknown";
        preset = new DevicePreset();
        type = QualityDeviceTypes.TABLET;
    }

    public void SetType(string type)
    {
        this.type = GetType(type);
    }

    public static QualityDeviceTypes GetType(string type)
    {
        if (types.ContainsKey(type))
            return types[type];
        return QualityDeviceTypes.TABLET;
    }

    public static string GetType(QualityDeviceTypes value)
    {
        foreach (KeyValuePair<string, QualityDeviceTypes> kv in types)
        {
            if (kv.Value == value)
                return kv.Key;
        }
        return string.Empty;
    }
}
