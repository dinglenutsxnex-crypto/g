using System;
namespace sf3DTO
{
	public class GetPlayerRequest
	{
		public string Version { get; set; } = string.Empty;
		public OfflineRequestBatch OfflineRequestBatch { get; set; }
	}
}
