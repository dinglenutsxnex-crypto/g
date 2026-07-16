using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class BundleConfig
{
	public string BundlesPath { get; set; }
	public Dictionary<string, bool> Available { get; private set; }

	public BundleConfig()
	{
		Available = new Dictionary<string, bool>();
	}

	public void SetAvailable(string name, bool available)
	{
		Available[name] = available;
	}

	public void Save(string path)
	{
		string json = JsonConvert.SerializeObject(this, Formatting.Indented);
		File.WriteAllText(path, json);
	}

	public static BundleConfig CreateFromFile(string path)
	{
		if (File.Exists(path))
		{
			string json = File.ReadAllText(path);
			return JsonConvert.DeserializeObject<BundleConfig>(json) ?? new BundleConfig();
		}
		return new BundleConfig();
	}
}
