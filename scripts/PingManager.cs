// SKELETON auto-generated from Unity source: PingManager.cs
// DO NOT rename classes/methods/fields below without updating this file everywhere it's used.
// FLAGGED: original used coroutines (IEnumerator/StartCoroutine) - needs manual async/signal redesign.
using Godot;
using System;

public class PingManager
{
	public PingManager()
	{
		// TODO: port constructor logic
	}

	public event Action<bool> OnPingFinished;
	public string LAST_SERVER_TIME_KEY;
	public string LAST_UPDATE_TIME_KEY;
	public long lastServerTime;
	public long lastUpdateTime;
	public bool running;
	public int failedPings;
	public Coroutine delayPingCor;
	public int lastPing;
	public PingResponse extensible;
	public Timestamp timestamp;

	public long num;

	public bool PingInProcess { get; private set; }
}
