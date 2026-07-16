using System;
using System.IO;
using Godot;

public class GlobalPath
{
    private const string PathToResourcesFolder = "Resources";
    public static readonly string PathToLoaderFolder = "assets/gamedata/resources";

    public static string LogsPath { get { return GameDataPath + "/Logs"; } }
    public static string ExternalPath { get { return GameDataPath + "/Resources"; } }
    public static string ApplicationPath { get { return ApplicationGameDataPath + "/Resources"; } }

    public static string GameDataPath
    {
        get
        {
            return OS.GetUserDataDir() + "/gamedata";
        }
    }

    public static string ApplicationGameDataPath
    {
        get
        {
            return OS.GetExecutablePath() + "/gamedata";
        }
    }

    public static int GetIndexExtension(string path, int startindex = 0)
    {
        int num = path.LastIndexOf(".", StringComparison.OrdinalIgnoreCase);
        return (num <= -1) ? (path.Length - startindex) : (num - startindex);
    }

    public static string GameDataCombine(string path)
    {
        return Path.Combine(GameDataPath, path);
    }

    public static string GetInternalPath(string key, string name = "")
    {
        return GetPath(key) + name;
    }

    public static string GetResoursesPath(string path)
    {
        return PathToLoaderFolder + "/" + path;
    }

    private static string GetPath(string key)
    {
        return InternalSettings.GetPath(key);
    }
}
