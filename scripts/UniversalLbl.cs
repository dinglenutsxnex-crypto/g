using Godot;
using System;

[Serializable]
public class UniversalLbl
{
	public Godot.Label label;

	public Godot.Label uiLabel;

	public string Text
	{
		get
		{
			if (label != null)
			{
				return label.Text;
			}
			if (uiLabel != null)
			{
				return uiLabel.Text;
			}
			return string.Empty;
		}
		set
		{
			if (label != null)
			{
				label.Text = value;
			}
			else if (uiLabel != null)
			{
				uiLabel.Text = value;
			}
		}
	}
}
