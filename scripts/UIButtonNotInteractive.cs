public partial class UIButtonNotInteractive : Button
{
	private bool mInitDone;
	private bool mState;

	public virtual void SetState(bool state, bool immediate)
	{
		if (!mInitDone)
		{
			mInitDone = true;
			OnInit();
		}
		if (mState != state)
		{
			mState = state;
		}
	}

	protected virtual void OnInit()
	{
	}
}

