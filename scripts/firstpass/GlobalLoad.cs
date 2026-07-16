using System;
using Godot;
using Newtonsoft.Json;

public class GlobalLoad : GlobalPath
{
    public static string GetLoadTextInternal(string internalAlias, string appendedPath = "")
    {
        string path = InternalSettings.GetPath(internalAlias);
        if (string.IsNullOrEmpty(path)) return null;
        return FilesUtil.ReadFileText(path);
    }

    public static string GetLoadText(string path)
    {
        return ResourceLoader.Load<TextFile>(path)?.Text;
    }

    public static T GetLoadJsonInternal<T>(string internalAlias, string appendedPath = "") where T : class
    {
        string text = GetLoadTextInternal(internalAlias, appendedPath);
        return GetJsonFromText<T>(text);
    }

    public static T GetLoadJson<T>(string path) where T : class
    {
        string text = GetLoadText(path);
        return GetJsonFromText<T>(text);
    }

    private static T GetJsonFromText<T>(string content) where T : class
    {
        if (!string.IsNullOrEmpty(content))
        {
            try { return JsonConvert.DeserializeObject<T>(content); }
            catch (Exception ex) { GD.PrintErr("Error GetLoadJson [" + ex.Message + "]"); }
        }
        return (T)null;
    }

    public static byte[] GetLoadBytesInternal(string internalAlias, string appendedPath = "")
    {
        string text = GetLoadTextInternal(internalAlias, appendedPath);
        return text != null ? System.Text.Encoding.UTF8.GetBytes(text) : null;
    }

    public static byte[] GetLoadBytes(string path)
    {
        string text = GetLoadText(path);
        return text != null ? System.Text.Encoding.UTF8.GetBytes(text) : null;
    }

    public static void Unload(object obj, bool immediate = true) { }
    public static void Destroy(object obj, bool immediate = true) { }
    public static void UnloadUnusedAssets() { }
    public static void GCCollectImmediately()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
}

public class TextFile : Resource
{
    [Export]
    public string Text { get; set; }
}
