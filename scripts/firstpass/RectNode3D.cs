using Godot;

public partial class RectNode3D : Node3D
{
    private Rect2 _rect = new Rect2(0, 0, 100, 100);

    public Rect2 rect
    {
        get { return _rect; }
        set { _rect = value; }
    }

    public float Width
    {
        get { return _rect.Size.X; }
        set { _rect.Size = new Vector2(value, _rect.Size.Y); }
    }

    public float Height
    {
        get { return _rect.Size.Y; }
        set { _rect.Size = new Vector2(_rect.Size.X, value); }
    }
}
