using System;
using System.Collections.Generic;
namespace sf3DTO
{
	public class OfflineRequestBatch
	{
		public List<OfflineRequest> Requests { get; set; } = new List<OfflineRequest>();
		public MutableOfflineState ClientState { get; set; }
	}
}
