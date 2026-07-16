// ⚠️ STUB: needs full port — original used NGUI UIButton and EventTrigger
using Godot;
using System;

namespace SF3
{
	public partial class ButtonAudio : Node
	{
		public enum EButtonState
		{
			PRESS,
			CLICK
		}

		public void Init(Button button)
		{
			button.Pressed += OnClickAction;
		}

		private void OnClickAction()
		{
		}
	}
}
