namespace common
{
	public enum RequestErrorCode
	{
		Ok = 0,
		Error = 1,
		InvalidSessionStatus = 100,
		SessionAlreadyStartLogin = 200,
		PlayerAlreadyStartLogin = 201,
		InvalidClientServerProtocolVersion = 202,
		InvalidLoginOrPassword = 203,
		InvalidSecurityInfo = 204,
		MaxValue = 9999
	}
}
