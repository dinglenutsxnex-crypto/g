using System.Globalization;
using SimpleJSON;

namespace SF3.Moves
{
	internal static class JsonUtils
	{
		internal static bool TryGetString(out string result, JSONClass baseClass, string key, string defaultValue = null)
		{
			result = GetText(baseClass, key);
			if (result != null) return true;
			result = defaultValue;
			return false;
		}

		internal static bool TryGetBool(out bool result, JSONClass baseClass, string key, bool defaultValue)
		{
			if (TryGetString(out string s, baseClass, key))
			{
				result = s.Equals("1") || s.ToUpper().Equals("TRUE");
				return true;
			}
			result = defaultValue;
			return false;
		}

		internal static bool TryGetFloat(out float result, JSONClass baseClass, string key, float defaultValue)
		{
			if (TryGetString(out string s, baseClass, key) && float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out float r))
			{
				result = r;
				return true;
			}
			result = defaultValue;
			return false;
		}

		internal static bool TryGetInt(out int result, JSONClass baseClass, string key, int defaultValue)
		{
			if (TryGetString(out string s, baseClass, key) && int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out int r))
			{
				result = r;
				return true;
			}
			result = defaultValue;
			return false;
		}

		internal static bool TryParseVector2(out Godot.Vector2 result, JSONClass baseClass, string key, Godot.Vector2 defaultValue)
		{
			JSONNode node = GetNode(baseClass, key);
			if (node != null)
			{
				result = new Godot.Vector2(GetFloat(node, "X"), GetFloat(node, "Y"));
				return true;
			}
			result = defaultValue;
			return false;
		}

		public static float GetFloat(JSONNode mapp, string name, float defValue = 0f)
		{
			string text = GetText(mapp, name);
			return text == null ? defValue : float.Parse(text, CultureInfo.InvariantCulture);
		}

		public static string GetText(JSONNode jsonClass, string name, string defValue = null)
		{
			JSONNode node = GetNode(jsonClass, name);
			return node != null ? ((JSONData)node).Value : defValue;
		}

		public static JSONNode GetNode(JSONNode node, string name)
		{
			if (node is JSONClass jc) return jc[name];
			if (node is JSONData) return node;
			if (node is JSONArray arr)
			{
				foreach (JSONNode child in arr.Children)
				{
					JSONNode found = GetNode(child, name);
					if (found != null) return found;
				}
			}
			return null;
		}
	}
}
