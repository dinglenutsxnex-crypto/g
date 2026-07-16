using Godot;

namespace SF3.TutorialAnimations
{
    public class Animation
    {
        protected Control Owner;

        protected Vector2 Offset;

        public Animation(Control owner, Vector2 offset)
        {
            Owner = owner;
            Offset = offset;
        }

        public virtual void InAnimation()
        {
            Owner.Position = new Vector2(Owner.Position.X + Offset.X, Owner.Position.Y + Offset.Y);
        }

        public virtual void OutAnimation(Callable callback)
        {
            callback.Call();
        }
    }
}
