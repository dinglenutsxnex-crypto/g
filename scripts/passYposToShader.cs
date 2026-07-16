using Godot;

public partial class passYposToShader : Node
{
	public override void _Process(double delta)
	{
		var children = FindChildren("*", "MeshInstance3D");
		foreach (Node child in children)
		{
			if (child is MeshInstance3D mi)
			{
				for (int i = 0; i < mi.GetSurfaceOverrideMaterialCount(); i++)
				{
					Material material = mi.GetSurfaceOverrideMaterial(i);
					if (material != null)
					{
						material.SetShaderParameter("_GameObYPos", GlobalPosition.Y);
					}
				}
			}
		}
	}
}
