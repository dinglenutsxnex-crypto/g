using Godot;
using SF3.TutorialAnimations;

[Tool]
public class TutorialPanel : Control
{
    [Export] private LocalizationText _description;
    [Export] private Control _panel;
    private SF3.TutorialAnimations.Animation _currentAnimation;

    public override void _Ready()
    {
        PauseWindow.OnPauseDisabled += ShowPanel;
        PauseWindow.OnPauseEnabled += HidePanel;
    }

    public void SetAnimation(string animationName, Vector2 offset)
    {
        switch (animationName)
        {
            case "BattleAnimation":
                _currentAnimation = new BattleAnimation(_panel, offset);
                break;
            case "MovingFromCenterAnimation":
                _currentAnimation = new MovingFromCenterAnimation(_panel, offset);
                break;
            default:
                _currentAnimation = new SF3.TutorialAnimations.Animation(_panel, offset);
                break;
        }
    }

    public void PlayInAnimation()
    {
        _currentAnimation?.InAnimation();
    }

    private void ShowPanel()
    {
        _description.Visible = true;
        _panel.Visible = true;
    }

    private void HidePanel()
    {
        _description.Visible = false;
        _panel.Visible = false;
    }

    public void PlayOutAnimation(Action callback)
    {
        _currentAnimation?.OutAnimation(callback);
    }

    public void SetMessage(string alias)
    {
        _description.SetAlias(alias);
    }

    public void SetMessage(string alias, string[] args)
    {
        _description.SetAlias(alias, args);
    }

    public void ToogleDescription(Vector3 viewportPosition, Vector2 offset)
    {
        if (viewportPosition.Y > 0.75f)
            DescriptionSetToBottom();
        else
            DescriptionSetToUp();
        _panel.Position = offset;
    }

    private void DescriptionSetToBottom()
    {
        _panel.AnchorLeft = 0f;
        _panel.AnchorRight = 1f;
        _panel.AnchorBottom = 0f;
    }

    private void DescriptionSetToUp()
    {
        _panel.AnchorLeft = 0f;
        _panel.AnchorRight = 1f;
        _panel.AnchorTop = 1f;
    }

    public override void _ExitTree()
    {
        PauseWindow.OnPauseDisabled -= ShowPanel;
        PauseWindow.OnPauseEnabled -= HidePanel;
    }
}
