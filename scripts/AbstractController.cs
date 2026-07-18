using Godot;
using System;
using System.Collections.Generic;

public abstract partial class AbstractController : ExtentionBehaviour
{
	public partial class Key
	{
		public Godot.Key KeyCode { get; private set; }
		public EQuadrants Quadrant { get; private set; }
		public int ModelID { get; private set; }

		public Key(Godot.Key keyCode, EQuadrants quadrant, int modelID)
		{
			Quadrant = quadrant;
			KeyCode = keyCode;
			ModelID = modelID;
		}
	}

	public const int onKeyPress = 0;
	public const int onKeyRelease = 1;

	protected List<Key> keyCodes = new List<Key>();

	public void AddTrakedKey(Godot.Key key, EQuadrants quadrant, int modelID = -1)
	{
		keyCodes.Add(new Key(key, quadrant, modelID));
	}

	protected void TrackKeys()
	{
		for (int i = 0; i < keyCodes.Count; i++)
		{
			if (Input.IsKeyPressed(keyCodes[i].KeyCode))
			{
				callEvent(0, keyCodes[i]);
			}
			if (Input.IsKeyPressed(keyCodes[i].KeyCode))
			{
				callEvent(1, keyCodes[i]);
			}
		}
	}

	public void OpenInterface()
	{
		foreach (Key keyCode in keyCodes)
		{
			if (Input.IsKeyPressed(keyCode.KeyCode))
			{
				callEvent(1, new Key(keyCode.KeyCode, keyCode.Quadrant, 1));
			}
		}
	}
}
