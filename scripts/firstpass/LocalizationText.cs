using Godot;
using System;

public class LocalizationText : Node
{
    [Export]
    private string _alias;

    [Export]
    private string[] _args;

    private Label _label;

    public void SetAlias(string newAlias)
    {
        _alias = newAlias;
        UpdateText();
    }

    public void SetAlias(string newAlias, string[] newArgs)
    {
        _args = newArgs;
        SetAlias(newAlias);
    }

    public string GetAlias()
    {
        return _alias;
    }

    public void SetColor(Color color)
    {
        if (_label != null)
            _label.Modulate = color;
    }

    public override void _Ready()
    {
        Localization.LanguageSwitched += UpdateText;
        _label = GetNode<Label>("..");
        if (_label == null)
            _label = GetParent() as Label;
        UpdateText();
    }

    private void UpdateText()
    {
        if (_label == null)
        {
            _label = GetParent() as Label;
            if (_label == null) return;
        }
        string text = Localization.Get(_alias).String;
        if (_args != null && _args.Length > 0)
            text = string.Format(text, _args);
        _label.Text = text;
    }

    public void Destroy()
    {
        Localization.LanguageSwitched -= UpdateText;
    }
}
