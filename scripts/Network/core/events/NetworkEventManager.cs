using System;
using System.Collections.Generic;
using Godot;

namespace Network.core.events
{
	public class NetworkEventManager : Node
	{
		private static NetworkEventManager _instance;
		private Dictionary<string, List<Action<NetworkEvent>>> _listeners = new Dictionary<string, List<Action<NetworkEvent>>>();
		private Queue<NetworkEvent> _eventQueue = new Queue<NetworkEvent>();

		public static NetworkEventManager Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new NetworkEventManager();
				}
				return _instance;
			}
		}

		public override void _Ready()
		{
			base._Ready();
			_instance = this;
		}

		public void AddListener(string command, Action<NetworkEvent> callback)
		{
			if (!_listeners.ContainsKey(command))
				_listeners[command] = new List<Action<NetworkEvent>>();
			_listeners[command].Add(callback);
		}

		public void RemoveListener(string command, Action<NetworkEvent> callback)
		{
			if (_listeners.ContainsKey(command))
			{
				_listeners[command].Remove(callback);
				if (_listeners[command].Count == 0)
					_listeners.Remove(command);
			}
		}

		public void DispatchEvent(NetworkEvent evt)
		{
			_eventQueue.Enqueue(evt);
		}

		private void ProcessEvents()
		{
			while (_eventQueue.Count > 0)
			{
				NetworkEvent evt = _eventQueue.Dequeue();
				if (_listeners.ContainsKey(evt.command))
				{
					foreach (var listener in _listeners[evt.command])
					{
						listener(evt);
					}
				}
				if (evt.callback != null)
				{
					evt.callback(evt);
				}
			}
		}

		public override void _Process(double delta)
		{
			ProcessEvents();
		}
	}
}
