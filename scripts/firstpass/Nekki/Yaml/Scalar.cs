using System;

namespace Nekki.Yaml
{
	[Serializable]
	public class Scalar : Node
	{
		public delegate void ScalarUpdateEventHandler();
		public static event ScalarUpdateEventHandler TextUpdate;

		public string text { get; private set; }

		public Scalar(string keyNew, string valueNew)
		{
			typeNode = "Scalar";
			key = keyNew;
			value = valueNew;
			text = valueNew;
		}

		public void SetText(string valueNew)
		{
			text = valueNew;
			value = valueNew;
			if (TextUpdate != null)
				TextUpdate();
		}

		public string GetText() { return text; }
	}
}
