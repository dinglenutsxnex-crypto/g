using System;
using Godot;
using Network.core.events;
using SF3.UserData;
using sf3DTO;

namespace SF3
{
	public static class BrawlerHelper
	{
		private static BrawlerEnemy currentEnemy;

		public static void GetNewBrawlerFightAsync(Action<BrawlerFight> OnFightSearchFinished)
		{
			if (currentEnemy != null)
			{
				GD.PrintErr("Error: Requesting a new fight while we still have a currentEnemy");
				currentEnemy = null;
			}
			NetworkConnection.Send("brawler_start", new object(), delegate(NetworkEvent e)
			{
				BrawlerFight brawlerFight = null;
				if (e.success)
				{
					OnFightSearchFinished?.Invoke(brawlerFight);
				}
				OnFightSearchFinished?.Invoke(brawlerFight);
			});
		}

		public static void SendBrawlerFight(FightResult fightResult)
		{
			BrawlerFinishRequest brawlerFinishRequest = new BrawlerFinishRequest();
			brawlerFinishRequest.Enemy = currentEnemy;
			brawlerFinishRequest.TotalRounds = fightResult.roundsPlayed;
			if (NetworkConnection.current.IsNetworkEstablished())
			{
				NetworkConnection.Send("brawler_finish", brawlerFinishRequest, OnBrawlerSent);
			}
			currentEnemy = null;
		}

		private static void OnBrawlerSent(NetworkEvent e)
		{
			if (!e.success)
			{
				return;
			}
		}
	}
}
