using System.Collections.Generic;
using System.Globalization;
using Godot;
using Jint.Native;

public struct LocationInfo
{
	private string _music;
	private Vector3 _position;
	private string _group;
	private string _locationName;

	public string music => _music;
	public Vector3 position => _position;
	public string group => _group;
	public string locationName => _locationName;

	public LocationInfo(Dictionary<string, JsValue> dictionary)
	{
		_locationName = dictionary["LocationName"].ToString();
		_position = new Vector3(
			int.Parse(dictionary["X"].ToString(), CultureInfo.InvariantCulture),
			int.Parse(dictionary["Y"].ToString(), CultureInfo.InvariantCulture),
			0f
		);
		_group = dictionary["Group"].ToString();
		_music = dictionary.ContainsKey("Music") ? dictionary["Music"].ToString() : string.Empty;
	}

	public bool InSamePlace(LocationInfo other)
	{
		return _position.X == other._position.X && _position.Y == other._position.Y;
	}
}
