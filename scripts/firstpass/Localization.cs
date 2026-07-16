using System;
using System.Collections.Generic;
using Godot;

public static class Localization
{
    public delegate void LanguageSwitchEventHandler();

    private static Func<string, LocaleImport.LocaleString> OverridenGetLocaleFunc;
    private static readonly Dictionary<SystemLanguage, Dictionary<string, LocaleImport.LocaleString>> Data = new Dictionary<SystemLanguage, Dictionary<string, LocaleImport.LocaleString>>();

    public static SystemLanguage Language { get; private set; }
    public static SystemLanguage DefaultLanguage { get; private set; }

    public static Dictionary<string, LocaleImport.LocaleString> GetCurrentLocaleData
    {
        get
        {
            if (Data.ContainsKey(Language)) return Data[Language];
            if (Data.ContainsKey(DefaultLanguage)) return Data[DefaultLanguage];
            return null;
        }
    }

    public static event LanguageSwitchEventHandler LanguageSwitched;

    static Localization()
    {
        Language = SystemLanguage.English;
        DefaultLanguage = SystemLanguage.English;
        Import(Language);
        Import(DefaultLanguage);
    }

    public static void Init(SystemLanguage? language, SystemLanguage? defaultLanguage = null, Func<string, LocaleImport.LocaleString> overridenGetLocaleFunc = null)
    {
        OverridenGetLocaleFunc = overridenGetLocaleFunc;
        if (language.HasValue && Language != language.Value)
        {
            Language = language.Value;
            Import(Language);
            OnLanguageSwitched();
        }
        if (defaultLanguage.HasValue && DefaultLanguage != defaultLanguage.Value)
        {
            DefaultLanguage = defaultLanguage.Value;
            Import(DefaultLanguage);
            if (!Data.ContainsKey(Language))
                OnLanguageSwitched();
        }
    }

    public static void Reset()
    {
        Data.Clear();
        Import(Language);
        Import(DefaultLanguage);
    }

    private static void OnLanguageSwitched()
    {
        LanguageSwitched?.Invoke();
    }

    private static void Import(SystemLanguage language)
    {
        if (!Data.ContainsKey(language))
        {
            LocaleImport localeImport = new LocaleImport(language);
            Data.Add(language, localeImport.Data);
        }
    }

    public static LocaleImport.LocaleString Get(string key)
    {
        if (!Data.ContainsKey(Language))
            return new LocaleImport.LocaleString(string.Format("err_{0}", Language));
        LocaleImport.LocaleString localeString = OverridenGetLocaleFunc != null ? OverridenGetLocaleFunc(key) : GetLocaleDefault(key);
        if (localeString == null)
            return new LocaleImport.LocaleString(string.Format("err_{0}", key));
        return localeString;
    }

    public static LocaleImport.LocaleString Get(string key, int symbolsInLine, string[] lastReplacement)
    {
        LocaleImport.LocaleString localeString = Get(key);
        return localeString.SplitByRows(symbolsInLine, lastReplacement);
    }

    private static LocaleImport.LocaleString GetLocaleDefault(string key)
    {
        return GetLocale(key);
    }

    public static LocaleImport.LocaleString GetLocale(string key)
    {
        if (string.IsNullOrEmpty(key)) return null;
        if (Data[Language].ContainsKey(key))
            return Data[Language][key];
        if (Data[DefaultLanguage].ContainsKey(key))
            return Data[DefaultLanguage][key];
        return null;
    }

    public static bool HasKey(string key)
    {
        return Data[Language].ContainsKey(key) || Data[DefaultLanguage].ContainsKey(key);
    }

    public static string Format(string key, params object[] replacements)
    {
        return string.Format(Get(key), replacements);
    }

    public static bool Contains(string alias, bool checkInDefaultLanguage = false)
    {
        return (Data.ContainsKey(Language) && Data[Language].ContainsKey(alias)) ||
               (checkInDefaultLanguage && Data.ContainsKey(DefaultLanguage) && Data[DefaultLanguage].ContainsKey(alias));
    }
}
