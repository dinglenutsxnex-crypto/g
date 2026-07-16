using Godot;

public partial class CameraConfiguration : Node
{
	public enum AspectRatio
	{
		_4X3 = 0,
		_16X9 = 1
	}

	public class CameraSettings
	{
		public AspectRatio aspect;
		public float minXPosition;
		public float maxXPosition;
		public float minYPosition;
		public float maxYPosition;
		public float startFollowYUp;
		public float startFollowYBot;
		public float startRotateYUp;
		public float startRotateYBot;
		public float farClipPlane;
		public float botVerticalAngle;
		public float upVerticalAngle;
		public float leftHorizontalAngle;
		public float rightHorizontalAngle;
		public float camZOffset;
	}

	[Export] public CameraSettings[] settings;

	public static CameraSettings Current { get; private set; }

	public override void _Ready()
	{
		if (settings != null && settings.Length > 0)
		{
			Current = settings[0];
			Camera3D cam = GetViewport().GetCamera3D();
			if (cam != null)
			{
				float aspect = (float)DisplayServer.WindowGetSize().X / (float)DisplayServer.WindowGetSize().Y;
				if (aspect >= 1.5f)
				{
					Current = GetSettingsByAspect(AspectRatio._16X9);
				}
				else if (aspect >= 1.3f)
				{
					Current = GetSettingsByAspect(AspectRatio._4X3);
				}
				GD.Print(string.Format("Current camera aspect [{0}]", Current.aspect));
			}
		}
	}

	private CameraSettings GetSettingsByAspect(AspectRatio aspect)
	{
		foreach (CameraSettings cameraSettings in settings)
		{
			if (cameraSettings.aspect == aspect)
			{
				return cameraSettings;
			}
		}
		return null;
	}
}
