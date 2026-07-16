using Godot;

public partial class TestTextures : Node
{
	private static TestTextures _instance;

	private static bool _shedule;

	[Export] public Texture2D Body;

	[Export] public Texture2D Head;

	public override void _Ready()
	{
		_instance = this;
		if (_shedule)
		{
			Replace();
		}
		else
		{
			_shedule = true;
		}
	}

	public override void _Process(double delta)
	{
		if (_shedule)
		{
			Replace();
		}
	}

	public static void Replace()
	{
		if (_instance == null)
		{
			_shedule = true;
			return;
		}
		_shedule = false;
		Node node = _instance.GetTree().Root.FindChild("test_player", true, false);
		if (node == null) return;
		var meshRenderers = node.FindChildren("*", "SkinnedMeshInstance3D", true, false);
		foreach (SkinnedMeshInstance3D smi in meshRenderers)
		{
			Material mat = smi.MaterialOverride.Duplicate() as Material;
			if (mat != null)
			{
				if (smi.Name.Contains("prefab"))
					mat.SetShaderParameter("texture_albedo", _instance.Body);
				else
					mat.SetShaderParameter("texture_albedo", _instance.Head);
				smi.MaterialOverride = mat;
			}
		}
	}
}