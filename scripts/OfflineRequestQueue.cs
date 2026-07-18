using Godot;
using System.Collections.Generic;
using clientDTO;
using SF3.UserData;

public class OfflineRequestQueue
{
	public class QueueItem
	{
		public OfflineRequestData data;

		public byte[] state;
	}

	public class QueueData
	{
		public List<QueueItem> queue = new List<QueueItem>();

		private long _currentID;

		public long currentID
		{
			get
			{
				return _currentID;
			}
			private set
			{
				_currentID = value;
			}
		}

		public QueueData(long stateID)
		{
			currentID = stateID;
		}

		public long GetCurrentStateID()
		{
			return currentID;
		}

		public long GetNextStateID()
		{
			if (currentID == long.MaxValue)
			{
				GD.PrintErr("Too many requests were created - int64 wasn't enough...");
			}
			return ++currentID;
		}

		public bool SetIDFromServer(long id)
		{
			if (currentID > id)
			{
				currentID = id;
				return false;
			}
			currentID = id;
			return true;
		}

		public void Add(QueueItem o)
		{
			queue.Add(o);
		}

		public void RemoveAcceptedRequests(long lastAcceptedID)
		{
			if (queue.Count == 0)
			{
				return;
			}
			int num = -1;
			for (int i = 0; i < queue.Count; i++)
			{
				QueueItem queueItem = queue[i];
				if (queueItem.data.RequestID <= lastAcceptedID)
				{
					num = i;
					continue;
				}
				break;
			}
			if (num != -1)
			{
				queue.RemoveRange(0, num + 1);
			}
		}

		public bool RemoveInvalidRequests(List<string> validVersions)
		{
			for (int i = 0; i < queue.Count; i++)
			{
				if (!validVersions.Contains(queue[i].data.VersionFull))
				{
					queue.RemoveRange(i, queue.Count - i);
					return true;
				}
			}
			return false;
		}
	}

	private QueueData queueData;

	private List<QueueItem> queue
	{
		get
		{
			return queueData.queue;
		}
	}

	public OfflineRequestQueue()
	{
		Load();
	}

	public void Start()
	{
		SendFromQueue();
	}

	private void SendFromQueue()
	{
		if (queue.Count > 0)
		{
		}
	}

	public void Stop()
	{
		NetworkConnection.current.RemoveCallbacks(this);
	}

	public long GetCurrentStateID()
	{
		return queueData.GetCurrentStateID();
	}

	internal void RemoveAcceptedRequests(long lastAcceptedID)
	{
		queueData.RemoveAcceptedRequests(lastAcceptedID);
	}

	public bool SetIDFromServer(long id)
	{
		GD.Print("Switched offline_state_id to: " + id);
		bool flag = queueData.SetIDFromServer(id);
		if (!flag)
		{
			GD.PrintErr("Client added new requests while we did syncing with the server - need fix ASAP");
		}
		return flag;
	}

	public void AddRequest(string cmd, object e)
	{
		QueueItem queueItem = new QueueItem();
		queueItem.data = new OfflineRequestData();
		queueItem.data.RequestID = queueData.GetNextStateID();
		queueItem.data.Cmd = cmd;
		queueItem.data.VersionFull = NetworkConnection.current.CurrentConfigVersion.full;
		AddToQueue(queueItem);
	}

	private void AddToQueue(QueueItem request)
	{
		queueData.Add(request);
	}

	public void Clear()
	{
		queueData = new QueueData(0L);
	}

	private void Load()
	{
		queueData = new QueueData(0L);
	}
}
