using System.Collections.Generic;
using Godot;

public class SpriteSlicer2DSliceInfo
{
    private List<Node> m_ChildObjects = new List<Node>();

    public Node SlicedObject { get; set; }
    public Vector2 SliceEnterWorldPosition { get; set; }
    public Vector2 SliceExitWorldPosition { get; set; }

    public List<Node> ChildObjects
    {
        get { return m_ChildObjects; }
        set { m_ChildObjects = value; }
    }
}
