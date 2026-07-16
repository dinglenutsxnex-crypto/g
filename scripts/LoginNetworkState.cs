using System;
using Godot;
using Network.core.events;
using Newtonsoft.Json.Linq;
using SF3.UserData;
using sf3DTO;

public class LoginNetworkState : TCPNetworkState
{
	private static readonly string EMAIL_TAG = "PlayerEmail";

	private ulong timeoutTimer;

	private LoginEmailDialogController dialog;

	public override void TCPStart(object data)
	{
		NetworkConnection.current.addEventListener("login", OnLoginSuccess);
		NetworkConnection.current.addEventListener("login_error", OnLoginFail);
		if (ShouldCheckEmail())
		{
			dialog = LoginEmailDialogController.ShowDialog(OnDialogClosed);
		}
		else
		{
			DoLogin();
		}
	}

	private void OnDialogClosed(string email)
	{
		// STUB: PlayerPrefs -> Godot.Save
		DoLogin();
	}

	private bool ShouldCheckEmail()
	{
		return NetworkConnection.Settings.CheckEmail && UserManager.GetPlayerID() < 0;
	}

	private void DoLogin()
	{
		dialog = null;
		JObject jObject = null;
		// STUB: PlayerPrefs.HasKey
		if (NetworkConnection.current.IsConnectionActive())
		{
			// STUB: coroutine -> timer
			timeoutTimer = (ulong)(NetworkConnection.Settings.LoginTimeout.ToSeconds() * 1000.0);
			NetworkConnection.current.RequestLogin(jObject);
		}
		else
		{
			OnFail("Connection not alive to login");
		}
	}

	public override void TCPCleanup()
	{
		NetworkConnection.current.removeEventListener("login", OnLoginSuccess);
		NetworkConnection.current.removeEventListener("login_error", OnLoginFail);
		timeoutTimer = 0;
		if (dialog != null)
		{
			dialog.CloseDialog();
			dialog = null;
		}
	}

	private void OnLoginSuccess(NetworkEvent e)
	{
		GD.Print(string.Format("OnLogin - Guid: {0} requestID: {1}", e.message, e.requestID));
		OnSuccess(typeof(JoinZoneNetworkState), null);
	}

	private void OnLoginFail(NetworkEvent e)
	{
		e.HandleErrorAsDialog(RequestErrorCode.CbtInvalidEmail);
		OnFail("Login Failed " + e.message);
	}

	public override void TCPStop()
	{
		Disconnect();
	}
}
