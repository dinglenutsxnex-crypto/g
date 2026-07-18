using Godot;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

[Tool]
public partial class TextPic : RichTextLabel
{
    private class Attributes
    {
        private Dictionary<string, string> attributes;

        public Attributes(Dictionary<string, string> attributes)
        {
            this.attributes = attributes;
        }

        public string GetValue(string name, string defaultValue = "")
        {
            return attributes.ContainsKey(name) ? attributes[name] : defaultValue;
        }

        public float GetValueAsFloat(string name, float defaultValue = 0f)
        {
            float result = defaultValue;
            if (HasAttribute(name))
                float.TryParse(GetValue(name, string.Empty), out result);
            return result;
        }

        public Color GetValueAsColor(string name, Color defaultValue = default)
        {
            Color color = defaultValue;
            if (HasAttribute(name))
                Color.FromHtml(GetValue(name, string.Empty), out color);
            return color;
        }

        public bool HasAttribute(string name) => attributes.ContainsKey(name);
    }

    [Serializable]
    public struct IconName
    {
        public string name;
        public Texture2D sprite;
    }

    private static readonly Regex s_Regex = new Regex("<quad (.*?)/>", RegexOptions.Singleline);
    private static readonly Regex s_AttributeRegex = new Regex("([a-zA-Z0-9]+)\\s*=\\s*([#a-zA-Z0-9/_\\-\\+\\\\\\.]+)", RegexOptions.Singleline);
    private string m_OutputText;
    public IconName[] inspectorIconList;
    private Dictionary<string, Texture2D> iconList = new Dictionary<string, Texture2D>();

    public override void _Ready()
    {
        if (inspectorIconList != null && inspectorIconList.Length > 0)
        {
            foreach (var icon in inspectorIconList)
                iconList.Add(icon.name, icon.sprite);
        }
    }

    private Attributes ParseAttributes(string attributesStr)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        foreach (Match item in s_AttributeRegex.Matches(attributesStr))
            dictionary.Add(item.Groups[1].Value, item.Groups[2].Value);
        return new Attributes(dictionary);
    }
}
