using SF3.GameModels;
using Godot;
namespace SF3.Effects
{
	public partial class SpriteAnimationEffect : GameEffectBase
	{
		private AnimationPlayer AnimationPlayer;
		private bool checkToDisable;
		public Animation animationClip;
		public float playTime;
		private float timer;
		public string stateName;
		private Vector3 worldPos;
		private Vector3 curPos;
		public override void _Ready()
		{
			base.Awake();
			AnimationPlayer = GetComponent<AnimationPlayer>();
		}
		public override void Play(Model model, Vector3 atPos)
		{
			worldPos = atPos;
			FixPosition();
			base.Play(model, curPos);
			AnimationPlayer.Play(stateName);
			playTime = animationClip.Length;
			checkToDisable = true;
			timer = 0f;
		}
		private void FixPosition()
		{
			curPos = GetViewport().GetCamera3D().WorldToViewportPoint(worldPos);
			curPos = UICamera3D.currentCamera3D.ViewportToWorldPoint(curPos);
			curPos.z = 1f;
		}
		private void _Process(double delta)
		{
			if (checkToDisable)
			{
				FixPosition();
				transf.position = curPos;
				timer += GameTimeController.deltaTime;
				if (timer >= playTime)
				{
					checkToDisable = false;
					obj.SetActive(false);
				}
			}
		}
	}
}

