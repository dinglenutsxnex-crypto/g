using System;
using Google.Protobuf.WellKnownTypes;
using System.Collections.Generic;
using Godot;
using Network.core.data;

namespace Network.core.events
{
	public class NetworkEvent
	{
		public bool success;
		public string command;
		public object data;
		public SFSProtocolResponse response;
		public Dictionary<string, object> parameters = new Dictionary<string, object>();
		public Action<NetworkEvent> callback;
		public float timestamp;

		public NetworkEvent()
		{
			timestamp = (float)Engine.GetProcessTime();
		}

		public NetworkEvent(string cmd, object data = null)
		{
			command = cmd;
			this.data = data;
			timestamp = (float)Engine.GetProcessTime();
		}

		public T GetData<T>()
		{
			if (data is T)
				return (T)data;
			return default(T);
		}

		public bool HasParameter(string key)
		{
			return parameters.ContainsKey(key);
		}

		public object GetParameter(string key)
		{
			if (parameters.ContainsKey(key))
				return parameters[key];
			return null;
		}

		public void SetParameter(string key, object value)
		{
			parameters[key] = value;
		}
	}
}
