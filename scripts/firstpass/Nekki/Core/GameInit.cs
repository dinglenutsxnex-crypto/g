using System;
using Godot;

namespace Nekki.Core
{
	public abstract class GameInit
	{
		public delegate void OnInitializeDoneEventHandler();

		private static bool canContinueInit = true;

		public static bool CanContinueInit
		{
			get { return canContinueInit; }
			set
			{
				canContinueInit = value;
				GD.Print("[" + Time.GetUnscaledTime() + "] canContinueInit == " + value);
			}
		}

		public event OnInitializeDoneEventHandler InitializeDone;

		private void OnInitializeDone()
		{
			InitializeDone?.Invoke();
		}

		protected void OnNekkiAssetDownloaderInitDone()
		{
			OnInitializeDone();
		}

		public virtual void Subscribe(params Action[] actions)
		{
			foreach (var action in actions)
			{
				InitializeDone += () => action();
			}
		}

		public virtual void Initialize()
		{
			CanContinueInit = true;
			Engine.MaxFps = 60;
			OnNekkiAssetDownloaderInitDone();
		}

		public abstract void Init(params Action[] actions);
	}
}
