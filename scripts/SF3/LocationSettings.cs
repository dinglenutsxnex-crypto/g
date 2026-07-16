using System.Collections.Generic;
using Godot;

namespace SF3
{
	[Tool]
	public partial class LocationSettings : Node
	{
		public static LocationSettings Instance;

		public string currentLocation = "dojo";

		public bool useCurrentLocation = true;

		public List<ESurfaceType> surfaceTypes;

		public LocationFloorSurfaceSettings locationSettingsObject;

		public override void _Ready()
		{
			Instance = this;
			surfaceTypes = SetSurfaceTypes();
		}

		private List<ESurfaceType> SetSurfaceTypes()
		{
			List<ESurfaceType> list = new List<ESurfaceType>();
			if (Instance.locationSettingsObject != null)
			{
				LocationFloorSurfaceSettings.FloorSurface[] floorSurface = Instance.locationSettingsObject.floorSurface;
				foreach (LocationFloorSurfaceSettings.FloorSurface floorSurface2 in floorSurface)
				{
					list.Add(floorSurface2.surfaceType);
				}
			}
			return list;
		}

		public static ESurfaceType GetFloorSurface(float coordX)
		{
			ESurfaceType result = ESurfaceType.None;
			if (Instance.locationSettingsObject != null)
			{
				LocationFloorSurfaceSettings.FloorSurface[] floorSurface = Instance.locationSettingsObject.floorSurface;
				foreach (LocationFloorSurfaceSettings.FloorSurface floorSurface2 in floorSurface)
				{
					if (coordX >= floorSurface2.startX && coordX <= floorSurface2.endX)
					{
						result = floorSurface2.surfaceType;
						break;
					}
				}
			}
			return result;
		}
	}
}
