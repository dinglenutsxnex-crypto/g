using System.Collections.Generic;
using Godot;

public partial class ShopAttributeDiff : Node
{
	private Label _label;
	private List<float> _data;
	private string _key;
	private float _showDuration;

	private void Init()
	{
		_label = GetNode<Label>(".");
	}

	public void SetData(List<float> data, float duration = 0f)
	{
		_data = data;
		_key = null;
		_showDuration = duration;
		SetDataOuter();
	}

	public void Clear()
	{
		if (_label != null)
			_label.Text = string.Empty;
	}

	public void SetDataFromLocalization(string key, float duration = 0f)
	{
		_key = key;
		_data = null;
		_showDuration = duration;
		SetDataOuter();
	}

	public override void _Ready()
	{
		if (_label == null)
		{
			Init();
		}
	}

	private void SetDataOuter()
	{
		SetDataInner();
	}

	private void SetDataInner()
	{
		Clear();
		if (_data != null)
		{
			bool flag = true;
			foreach (float datum in _data)
			{
				_label.Text += (flag ? string.Empty : "|") + ((!(datum > 0f)) ? string.Empty : "+") + datum;
				flag = false;
			}
		}
		else
		{
			if (_key == null)
				return;
			_label.Text = _key;
		}
	}
}
