using Godot;

public partial class NekkiLabel : Label
{
    [Export]
    private bool _localized;

    [Export]
    private string _alias;

    [Export]
    private string[] _args;

    public override void _Ready()
    {
        if (_localized && !string.IsNullOrEmpty(_alias))
            UpdateText();
    }

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

    private void UpdateText()
    {
        string text = Localization.Get(_alias).String;
        if (_args != null && _args.Length > 0)
            text = string.Format(text, _args);
        Text = text;
    }
}
