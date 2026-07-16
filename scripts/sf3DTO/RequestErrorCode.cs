namespace sf3DTO
{
	public enum RequestErrorCode
	{
		DeprecatedRequestErrorCode = 0,
		PlayerNotFound = 10000,
		InvalidConfigVersion = 10001,
		InvalidDisplayName = 20000,
		RecoverableOfflineRequestError = 30000,
		UnrecoverableOfflineRequestError = 30001,
		InvalidLogEvent = 40000,
		BrawlerCannotFindEnemy = 50000,
		BrawlerFightMissed = 50001,
		BrawlerInvalidEnemy = 50002,
		BrawlerAlreadyStarted = 50003,
		BrawlerInvalidTotalRounds = 50004,
		CbtInvalidEmail = 60000
	}
}
