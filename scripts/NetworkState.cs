using System;

public static class NetworkState
{
	public static Type Active => typeof(ActiveNetworkState);

	public static Type Balancer => typeof(BalancerNetworkState);

	public static Type Config => typeof(ConfigNetworkState);

	public static Type Connection => typeof(ConnectionNetworkState);

	public static Type Login => typeof(LoginNetworkState);

	public static Type CreatePlayer => typeof(CreatePlayerNetworkState);

	public static Type GetPlayer => typeof(GetPlayerNetworkState);

	public static Type JoinZone => typeof(JoinZoneNetworkState);

	public static Type RefreshBattles => typeof(RefreshBattlesNetworkState);

	public static Type Timeout => typeof(TimeoutNetworkState);

	public static Type WaitEndOfTutorial => typeof(WaitEndOfTutorialNetworkState);
}
