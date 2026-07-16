using Godot;
using Nekki.UI;
using Nekki.UI;
using SF3.GameModels;

namespace SF3.Effects
{
	public class GUIEffect : GameEffectBase
	{
		public string side;

		public Vector3 localPos;

		public string leftAnchor = "TopLeft";

		public string rightAnchor = "TopRight";

		public float leftSidePos;

		public float rightSidePos;

		public bool offsetEffectByAspect;

		public bool usesStack;

		public int priorityInStack;

		public bool changePivot;

		public string[] hideComponentIds;

		private Godot.Collections.Array _tweeners;

		protected Tween _tweenPosition;

		private bool _leftSide;

		public bool LeftSide
		{
			get { return _leftSide; }
		}

		public override void _Ready()
		{
			base._Ready();
			transf.SetParent(NekkiUIRoot.Instance.GetAnchor(side));
			transf.Scale = Vector3.One;
			transf.Position = localPos;
			_tweeners = GetTweeners();
			GodotObject longestTweener = GetLongestTweener();
			if (longestTweener != null)
			{
				// TODO: attach tween finished callback
			}
		}

		private Godot.Collections.Array GetTweeners()
		{
			// STUB: replace UITweener[] with Godot Tween collection
			return new Godot.Collections.Array();
		}

		private GodotObject GetLongestTweener()
		{
			// STUB: NGUI UITweener replaced with Godot Tween
			return null;
		}

		public void MoveTo(float y)
		{
			Vector3 localPosition = transf.Position;
			localPosition.Y = y;
			// STUB: TweenPosition.Begin -> Godot CreateTween
			_tweenPosition = CreateTween();
			_tweenPosition.TweenProperty(transf, "position", localPosition, 0.2f);
		}

		public override void Play(Model model)
		{
			base.model = model;
			Visible = true;
			// STUB: UITweener reset
			TutorialManager.Instance.SetVisibleComponents(hideComponentIds, false);
		}

		public override void Play(Model model, bool leftSide)
		{
			Play(model, leftSide, localPos.Y);
		}

		public override void Play(Model model, bool leftSide, string alias, string[] values)
		{
			Play(model, leftSide, localPos.Y, alias, values);
		}

		public override void Play(Model model, bool leftSide, float yPos)
		{
			_leftSide = leftSide;
			Vector3 localPosition = localPos;
			localPosition.Y = yPos;
			if (changePivot)
			{
				if (_leftSide)
				{
					// STUB: UIWidget pivot -> Control anchor
					transf.SetParent(NekkiUIRoot.Instance.GetAnchor(leftAnchor));
					localPosition.X = leftSidePos;
				}
				else
				{
					transf.SetParent(NekkiUIRoot.Instance.GetAnchor(rightAnchor));
					localPosition.X = rightSidePos;
				}
			}
			if (offsetEffectByAspect)
			{
				localPosition.X *= ConstantsSF3.GetCurrentAspect;
			}
			transf.Position = localPosition;
			Play(model);
		}

		public override void Play(Model model, bool leftSide, float yPos, string alias, string[] values)
		{
			SetLabelText(alias, values);
			Play(model, leftSide, yPos);
		}

		protected void SetLabelText(string alias, string[] values)
		{
			if (alias != null)
			{
				string text = Localization.Get(alias);
				if (values != null && values.Length > 0)
				{
					text = string.Format(text, values);
				}
				// STUB: GetComponent<NekkiUILabel>().text -> Label.Text
				Label label = this as Label;
				if (label != null) label.Text = text;
			}
		}

		public override void Disable()
		{
			base.Disable();
			TutorialManager.Instance.SetVisibleComponents(hideComponentIds, true);
		}

		private void OnDisable()
		{
			if (_tweenPosition != null)
			{
				_tweenPosition.Stop();
			}
			if (usesStack)
			{
				EffectsManager.DeteleEffectFromStack(this);
			}
		}
	}
}
