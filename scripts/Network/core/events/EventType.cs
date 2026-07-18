using Godot;
using System;
using System.Collections.Generic;
using Google.Protobuf.WellKnownTypes;
using Type = System.Type;

namespace Network.core.events
{
	public class EventType
	{
		private List<string> tags;

		public Type sendType { get; private set; }
		public Type responseType { get; private set; }

		public bool isStateChanger
		{
			get
			{
				return tags.Contains("state_change");
			}
		}

		public List<string> PossibleErrors { get; private set; }

		public EventType(Type _responseType)
		{
			sendType = typeof(Empty);
			responseType = _responseType;
			tags = new List<string>();
			PossibleErrors = new List<string>();
		}

		public EventType(Type _sendType, Type _responseType, List<string> _tags, List<string> _possibleErrors)
		{
			sendType = _sendType;
			responseType = _responseType;
			tags = _tags;
			PossibleErrors = _possibleErrors;
		}
	}
}
