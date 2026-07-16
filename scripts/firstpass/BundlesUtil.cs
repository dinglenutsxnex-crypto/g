using System.Collections.Generic;
using Godot;

public class BundlesUtil
{
    private static readonly Dictionary<string, object> bundlesCache = new Dictionary<string, object>();
    private static readonly List<object> bundlesDependencies = new List<object>();
    private static BundleConfig currentConfig;

    public static void InitConfig(BundleConfig config)
    {
        currentConfig = config;
        UnloadDependencies();
        LoadDependencies();
    }

    private static void UnloadDependencies()
    {
        bundlesDependencies.Clear();
    }

    private static void LoadDependencies()
    {
        List<string> allDependencies = currentConfig.GetAllDependencies();
        foreach (string item in allDependencies)
        {
            object bundle = GetBundle(item);
            if (bundle != null)
                bundlesDependencies.Add(bundle);
        }
    }

    public static T[] GetObjects<T>(string path) where T : class
    {
        return null;
    }

    public static T GetObject<T>(string path) where T : class
    {
        return null;
    }

    public static void UnloadAsset(object obj) { }

    public static void UnloadUnusedAssets() { }

    private static object GetBundle(string name)
    {
        if (bundlesCache.ContainsKey(name))
            return bundlesCache[name];
        return null;
    }
}
