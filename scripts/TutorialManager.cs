using System;
using System.Collections.Generic;
using Godot;

public partial class TutorialManager : Node
{
	private static TutorialManager _instance;

	private Queue<string> _tutorialQueue = new Queue<string>();

	private bool _isPlaying;

	public static TutorialManager Instance
	{
		get { return _instance; }
	}

	public override void _Ready()
	{
		base._Ready();
		_instance = this;
	}

	public void StartTutorial(string tutorialId)
	{
		GD.Print("STUB: TutorialManager.StartTutorial: " + tutorialId);
		_tutorialQueue.Enqueue(tutorialId);
		if (!_isPlaying)
			PlayNext();
	}

	private void PlayNext()
	{
		if (_tutorialQueue.Count > 0)
		{
			_isPlaying = true;
			string step = _tutorialQueue.Dequeue();
			GD.Print("STUB: TutorialManager.PlayNext: " + step);
		}
		else
		{
			_isPlaying = false;
		}
	}

	public void CompleteCurrentStep()
	{
		GD.Print("STUB: TutorialManager.CompleteCurrentStep");
		PlayNext();
	}

	public void SkipAll()
	{
		_tutorialQueue.Clear();
		_isPlaying = false;
		GD.Print("STUB: TutorialManager.SkipAll");
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (_instance == this)
			_instance = null;
	}
}
