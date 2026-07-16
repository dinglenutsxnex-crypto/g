using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class QualityPresets
{
    public Dictionary<string, DevicePreset> Presets = new Dictionary<string, DevicePreset>();
    public Dictionary<string, DeviceQuality> Devices = new Dictionary<string, DeviceQuality>();
    public Dictionary<string, DeviceCondition> Conditions = new Dictionary<string, DeviceCondition>();

    public void InitPreset()
    {
        foreach (KeyValuePair<string, DevicePreset> preset in Presets)
        {
            preset.Value.name = preset.Key;
        }
        foreach (KeyValuePair<string, DeviceQuality> device in Devices)
        {
            device.Value.preset = GetPresetByName(device.Value.presetName);
        }
        foreach (KeyValuePair<string, DeviceCondition> condition in Conditions)
        {
            condition.Value.preset = GetPresetByName(condition.Value.presetName);
        }
    }

    public DevicePreset GetConditionPreset()
    {
        string platformName = GetRuntimePlatform().ToLower();
        int processorCount = 2;
        int systemMemorySize = 1024;
        DeviceCondition deviceCondition = null;
        foreach (KeyValuePair<string, DeviceCondition> condition in Conditions)
        {
            if (condition.Value.Equal(platformName, processorCount, systemMemorySize) && (deviceCondition == null || deviceCondition.priority < condition.Value.priority))
            {
                deviceCondition = condition.Value;
            }
        }
        return (deviceCondition == null) ? null : deviceCondition.preset;
    }

    public DeviceQuality GetDevice()
    {
        return null;
    }

    public DevicePreset GetPresetByName(string name)
    {
        if (!name.IsNullOrEmpty() && Presets.ContainsKey(name))
        {
            return Presets[name];
        }
        return null;
    }

    public QualityDeviceTypes GetConditionDeviceType()
    {
        return QualityDeviceTypes.PHONE;
    }

    private string GetRuntimePlatform()
    {
        return "windows";
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}
