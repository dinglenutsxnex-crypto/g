using Godot;
using System;
using System.Collections.Generic;

public class NekkiUIElements : Node
{
    [Serializable]
    public class Element
    {
        [Export]
        public string Name;

        [Export]
        public Node Widget;
    }

    [Export]
    public List<Element> Elements;

    public Node this[string index]
    {
        get
        {
            if (Elements != null)
            {
                for (int i = 0; i < Elements.Count; i++)
                {
                    if (Elements[i].Name.Equals(index))
                        return Elements[i].Widget;
                }
            }
            return null;
        }
    }

    public T Get<T>(string element) where T : Node
    {
        T val = this[element] as T;
        if (val == null && this[element] != null)
            val = this[element].GetNodeOrNull<T>(".");
        return val;
    }
}
