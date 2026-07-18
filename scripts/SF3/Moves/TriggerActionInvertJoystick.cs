using Nekki.Yaml;
using Node = Nekki.Yaml.Node;

namespace SF3.Moves
{
    public class TriggerActionInvertJoystick : TriggerActionRoundResetable
    {
        public TriggerActionInvertJoystick(Node yamlNode)
            : base(EActionType.INVERT_JOYSTICK, yamlNode)
        {
        }

        protected override void ApplyAction(object modelData)
        {
            base.ApplyAction(modelData);
            GameController.Instance.InvertQuadrants(true);
        }

        public override void Reset()
        {
            GameController.Instance.InvertQuadrants(false);
        }
    }
}
