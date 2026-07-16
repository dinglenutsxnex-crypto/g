using System;
using System.Collections.Generic;
using Godot;
using Network.core.data;
using Network.core.events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.UserData;
using sf3DTO;

public class SF3EventLogger : BaseEventLogger
{
	public void Init()
	{
		NetworkConnection.current.OnNetworkEstablished += Start;
	}

	public void Cleanup()
	{
		NetworkConnection.current.RemoveCallbacks(this);
	}

	private void Start()
	{
		BaseEventLogger.eventsData.events.RemoveAll((JObject e) => !ShouldBeLogged(e));
		Send(BaseEventLogger.eventsData.events);
	}

	public override void AddEvent(string etype, JObject eventData)
	{
		base.AddEvent(etype, eventData);
		Send(eventData);
	}

	private void Send(List<JObject> events)
	{
		LogRequest logRequest = new LogRequest();
		foreach (JObject @event in events)
		{
			logRequest.Events.Add(@event.ToString(Formatting.None));
		}
		if (logRequest.Events.Count > 0)
		{
			Send(logRequest);
		}
	}

	private void Send(JObject eventData)
	{
		if (!ShouldBeLogged(eventData))
		{
			RemoveEvent(eventData);
			return;
		}
		LogRequest logRequest = new LogRequest();
		logRequest.Events.Add(eventData.ToString(Formatting.None));
		Send(logRequest);
	}

	private void Send(LogRequest request, bool next = false)
	{
		if (NetworkConnection.current.IsNetworkEstablished())
		{
			string cmd = "log";
			Action<NetworkEvent> callback = OnSendLogResponse;
			NetworkConnection.SendEvent(cmd, request, callback, request, null, next, false);
		}
	}

	private void OnSendLogResponse(NetworkEvent e)
	{
		LogRequest logRequest = e.data as LogRequest;
		if (e.HandleError(ClientRequestError.ClientTimeout))
		{
			Send(logRequest, true);
		}
		else if (e.HandleError(RequestErrorCode.InvalidLogEvent) || e.isAnyServerIssue || e.success)
		{
			RemoveEvents(logRequest);
		}
	}

	private void RemoveEvents(LogRequest finishedRequest)
	{
		for (int num = finishedRequest.Events.Count - 1; num >= 0; num--)
		{
			JObject eventToRemove = JObject.Parse(finishedRequest.Events[num]);
			RemoveEvent(eventToRemove);
		}
		Save();
	}

	protected override void AddGeneralData(JObject data)
	{
		base.AddGeneralData(data);
		try
		{
			data.Add("pl", UserManager.GetLevel());
			data.Add("cts", GetCurrentTime());
			data.Add("plf", SFSProtocol.getPlatform());
			data.Add("v", NetworkConnection.current.CurrentConfigVersion.version.GetVersionToString(4));
			data.Add("fv", NetworkConnection.current.CurrentConfigVersion.full);
			data.Add("ab", UserManager.GetABTag());
			data.Add("sid", NetworkConnection.current.CurrentPlayerStateIDForLogging);
			data.Add("onl", NetworkConnection.current.IsNetworkEstablished());
		}
		catch (Exception ex)
		{
			GD.PrintErr("AddGeneralData Error:" + ex.Message);
			data.Add("err", ex.Message);
		}
	}

	private long GetCurrentTime()
	{
		if (UserManager.IsStartingTutorialCompleted)
		{
			return NetworkConnection.current.getCurrentServerTime().Value;
		}
		return TimeManager.GetLocalMS();
	}

	protected override bool ShouldBeLogged(string etype)
	{
		return true;
	}

	private bool ShouldBeLogged(JObject e)
	{
		return ShouldBeLogged(e["etype"].ToString());
	}
}
