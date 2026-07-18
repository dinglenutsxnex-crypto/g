using Godot;
using System.Collections;
using System.Collections.Generic;

public partial class VisualDebugUI : UIModuleHolder
{
    public partial class Unit
    {
        private bool expand;
        public int iteration;
        public Node3D VisualObject;
        public Node3D transform;
        public List<Script> scripts = new List<Script>();
        public List<Unit> childs = new List<Unit>();

        public bool isRoot => transform != null && transform.GetParent() == null;

        public Unit(Node3D root)
        {
            transform = root;
        }

        public void Show(ScrollContainer scrollView, VisualDebugUiUnit baseUnit, string parentName, int iteretion, int order)
        {
            if (VisualObject != null)
                VisualObject.QueueFree();
            iteration = iteretion;
            VisualObject = baseUnit.Duplicate() as Node3D;
            VisualObject.Visible = true;
            if (childs.Count == 0) { }
            else if (expand) { }
            else { }
            if (expand)
            {
                for (int i = 0; i < childs.Count; i++)
                    childs[i].Show(scrollView, baseUnit, VisualObject.Name, iteretion + 1, i);
            }
        }

        public void Expand()
        {
            expand = !expand;
        }

        public void Init(Dictionary<int, Unit> unitsDict)
        {
            childs.Clear();
            foreach (var item in unitsDict)
            {
                if (item.Value.transform.GetParent() == transform)
                    childs.Add(item.Value);
            }
        }
    }

    private List<Unit> _units = new List<Unit>();
    private Dictionary<int, Unit> _unitsDict = new Dictionary<int, Unit>();
    private static Unit _current;
    [Export] private ScrollContainer _scrollViewObjects;
    [Export] private ScrollContainer _scrollViewScripts;
    [Export] private VisualDebugUiUnit _baseUnitObjects;
    [Export] private VisualDebugUiScript _baseUnitScripts;

    public static VisualDebugUI Instance { get; private set; }

    private void RefreshUnits()
    {
        _units.Clear();
        if (_current != null && _current.transform == null)
            _current = null;
        List<int> list = new List<int>();
        foreach (var item in _unitsDict)
        {
            if (!item.Value.transform.IsInsideTree())
                list.Add(item.Key);
        }
        foreach (int id in list)
            _unitsDict.Remove(id);
        var objects = GetObjects();
        foreach (var obj in objects)
        {
            if (!_unitsDict.ContainsKey(obj.GetInstanceId()) && obj != this)
                _unitsDict.Add((int)obj.GetInstanceId(), new Unit(obj));
        }
        foreach (var item in _unitsDict)
            item.Value.Init(_unitsDict);
        foreach (var value in _unitsDict.Values)
        {
            if (value.isRoot)
                _units.Add(value);
        }
        Redraw();
    }

    private List<Node3D> GetObjects()
    {
        var list = new List<Node3D>();
        foreach (Node child in GetTree().Root.GetChildren())
        {
            if (child is Node3D n3d)
                list.Add(n3d);
        }
        return list;
    }

    public override void _Ready()
    {
        Instance = this;
        RefreshUnits();
        _baseUnitObjects.Visible = false;
        _baseUnitScripts.Visible = false;
    }

    public static void Open()
    {
        if (Instance != null && Instance.IsInsideTree())
            Instance.Visible = true;
        else
            NekkiUIRootModules.Instance.MountNativeModule("VisualDebug");
    }

    public static void Close()
    {
        if (Instance != null)
            Instance.Visible = false;
    }

    public void OnSelect(Unit unit)
    {
        _current = unit;
    }

    public void OnExpand(Unit unit)
    {
        unit.Expand();
        Redraw();
    }

    public void Redraw()
    {
        for (int i = 0; i < _units.Count; i++)
            _units[i].Show(_scrollViewObjects, _baseUnitObjects, string.Empty, 0, i);
    }
}
