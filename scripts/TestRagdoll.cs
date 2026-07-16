using Godot;

public partial class TestRagdoll : Node
{
    private Button Kill;

    private Button Restart;

    private Node3D foe;

    private AnimationPlayer foeAnimator;

    private CollisionShape3D foeCollider;

    private Vector3 foeBasePosition;

    public override void _Ready()
    {
        foeAnimator = foe.GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
        foeCollider = foe.GetNodeOrNull<CollisionShape3D>("CollisionShape3D");
        Kill.Pressed += DoKill;
        Restart.Pressed += DoRestart;
        foeBasePosition = foe.Position;
        DoRestart();
    }

    private void DoKill()
    {
        Kill.Visible = false;
        Restart.Visible = true;
        foeCollider.Disabled = true;
        foeAnimator.Stop();
        foeAnimator.Pause();
    }

    private void DoRestart()
    {
        Kill.Visible = true;
        Restart.Visible = false;
        foeCollider.Disabled = false;
        foeAnimator.Play();
        foe.Position = foeBasePosition;
    }

    public override void _Process(double delta)
    {
    }
}
