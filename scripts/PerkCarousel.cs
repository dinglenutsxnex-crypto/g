using System;
using System.Collections.Generic;
using Godot;

public partial class PerkCarousel : Node
{
	[Export]
	public Node itemContainer;

	[Export]
	public Button prevButton;

	[Export]
	public Button nextButton;

	[Export]
	public int maxVisibleItems = 3;

	private List<Node> _items = new List<Node>();
	private int _currentIndex;

	public override void _Ready()
	{
		GD.Print("STUB: PerkCarousel._Ready");
	}

	public void AddItem(Node item)
	{
		_items.Add(item);
		GD.Print("STUB: PerkCarousel.AddItem");
	}

	public void RemoveItem(Node item)
	{
		_items.Remove(item);
	}

	public void Clear()
	{
		_items.Clear();
	}

	public void ShowPrevious()
	{
		if (_currentIndex > 0)
		{
			_currentIndex--;
			UpdateDisplay();
		}
	}

	public void ShowNext()
	{
		if (_currentIndex < _items.Count - maxVisibleItems)
		{
			_currentIndex++;
			UpdateDisplay();
		}
	}

	private void UpdateDisplay()
	{
		GD.Print("STUB: PerkCarousel.UpdateDisplay: " + _currentIndex);
	}
}
