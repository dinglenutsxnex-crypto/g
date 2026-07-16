using System;
using System.IO;
using System.Text;
using Godot;
using Newtonsoft.Json;

public class FilesUtil
{
    public static void WriteFileBytes(string path, byte[] bytes)
    {
        CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllBytes(path, bytes);
    }

    public static void WriteFileText(string path, string content = "")
    {
        AppendFileText(path, content, true);
    }

    public static void AppendFileText(string path, string content = "", bool isReplace = false)
    {
        CreateDirectory(Path.GetDirectoryName(path));
        if (!isReplace && IsFileExists(path))
            File.AppendAllText(path, content);
        else
            File.WriteAllText(path, content);
    }

    public static void CreateTextWriter(string path, Action<TextWriter> action, bool append = false)
    {
        using (TextWriter writer = CreateTextWriter(path, append))
        {
            action(writer);
        }
    }

    public static TextWriter CreateTextWriter(string path, bool append = false)
    {
        CreateDirectory(Path.GetDirectoryName(path));
        return new StreamWriter(path, append, Encoding.UTF8);
    }

    public static void SaveJSON(string path, object obj, Formatting formatting = Formatting.None)
    {
        try
        {
            string content = JsonConvert.SerializeObject(obj, formatting);
            WriteFileText(path, content);
        }
        catch (Exception ex)
        {
            GD.PrintErr("Error SaveJSON config [" + ex.Message + "]");
        }
    }

    public static T ReadFileJson<T>(string path) where T : class
    {
        string value = ReadFileText(path);
        if (!string.IsNullOrEmpty(value))
        {
            try { return JsonConvert.DeserializeObject<T>(value); }
            catch (Exception ex) { GD.PrintErr("Error ReadFileJson [" + path + " " + ex.Message + "]"); }
        }
        return (T)null;
    }

    public static string ReadFileText(string path)
    {
        if (IsFileExists(path))
            return File.ReadAllText(path);
        return null;
    }

    public static string[] ReadFileLines(string path)
    {
        if (IsFileExists(path))
            return File.ReadAllLines(path);
        return null;
    }

    public static byte[] ReadFileBytes(string path)
    {
        if (IsFileExists(path))
            return File.ReadAllBytes(path);
        return null;
    }

    public static bool IsFileExists(string path) { return File.Exists(path); }
    public static bool IsDirectoryExists(string path) { return Directory.Exists(path); }

    public static bool DeleteFile(string path)
    {
        if (IsFileExists(path)) { File.Delete(path); return true; }
        return false;
    }

    public static bool DeleteDirectory(string path)
    {
        if (IsDirectoryExists(path)) { Directory.Delete(path, true); return true; }
        return false;
    }

    public static void CreateDirectory(string path)
    {
        if (!IsDirectoryExists(path))
            Directory.CreateDirectory(path);
    }
}
