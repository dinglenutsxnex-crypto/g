using System;
using Google.Protobuf.WellKnownTypes;
using Godot;

namespace Nekki.Utils
{
	public partial class GlobalTimer : Node
	{
		public const int TICK = 0;
		public const int FRAME_TICK = 1;

		private static GlobalTimer _instance;
		private static double _syncTime;
		private static DateTime _serverUtc;
		private static bool _syncronized;
		private static Action _onSuccess;
		private static Action _onError;
		private static bool _requestInProgress;
		private static bool _lastRequestSuccessful;
		private static bool _externalInit;

		private double _lastTimeTick;

		public static GlobalTimer Instnce
		{
			get
			{
				if (_instance == null)
					Init();
				return _instance;
			}
		}

		public static DateTime LocalizedNow => Now.ToLocalTime();
		public static DateTime Now => _serverUtc.AddSeconds(Time.GetUnscaledTime() - _syncTime);
		public static double GetTime => ConvertToUnixTimestamp(Now);
		public static bool IsSynchronized => _syncronized;
		public static bool IsRequestInProgress => _requestInProgress;
		public static bool IsLastRequestSuccessful => _lastRequestSuccessful;

		public static long ConvertToUnixTimestamp(DateTime date)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			return (long)Math.Floor((date.ToUniversalTime() - epoch).TotalSeconds);
		}

		public static void Init(bool externalInit = false)
		{
			if (_instance != null) return;
			_instance = new GlobalTimer();
			_instance.Name = "_timer";
			_instance._syncronized = false;
			_instance._lastRequestSuccessful = false;
			_instance._externalInit = externalInit;
			if (!_externalInit)
				ServerTimeSync();
		}

		public static void ServerTimeSync(Action onSuccess = null, Action onError = null)
		{
			_requestInProgress = true;
			_syncronized = false;
			_onSuccess = onSuccess;
			_onError = onError;
			_onSuccess?.Invoke();
		}

		public static void ServerTimeExtended(long msec)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			_serverUtc = epoch + TimeSpan.FromMilliseconds(msec);
			_syncTime = Time.GetUnscaledTime();
			_syncronized = true;
			_requestInProgress = false;
			_lastRequestSuccessful = true;
			_onSuccess?.Invoke();
		}

		public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
		{
			return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimeStamp);
		}

		public static DateTime UnixTimeStampToDateTimeLocal(double unixTimeStamp)
		{
			return UnixTimeStampToDateTime(unixTimeStamp).ToLocalTime();
		}

		public override void _Process(double delta)
		{
			if (_lastTimeTick + 1.0 < Time.GetUnscaledTime())
			{
				_lastTimeTick = Time.GetUnscaledTime();
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				_instance = null;
			base.Dispose(disposing);
		}
	}
}
