using Godot;
using System;

public partial class UIScrollViewCustom : Control
{
    public enum CellVisibility
    {
        FULL = 0,
        NONE = 1,
        TOP_PART = 2,
        BOTTOM_PART = 3
    }

    [Export]
    private Vector2 _cellSize;
    [Export]
    private Curve _scrollInertiaCurve;
    [Export]
    private float _scrollInertiaTime = 0.5f;
    [Export]
    public Vector2 padding = Vector2.Zero;

    private bool _isInertia;
    private Vector2 _lastPosition;
    private Vector2 _direction;
    private float _inertiaTimeEnd;
    private Vector2 _lastScrollLocalPoint;
    private Vector2 _nextScrollLocalPoint;
    private Vector2 _minScrollPoint;
    private Vector2 _maxScrollPoint;
    private int _visibleRowsCount;
    private int _rowsCount;
    public bool disableRecalcBounds;
    private Vector2 _currentVisibleRows;
    private Vector2 _viewSize;

    public Vector2 cellSize
    {
        get { return _cellSize; }
        set
        {
            _cellSize.X = Mathf.Abs(value.X);
            _cellSize.Y = Mathf.Abs(value.Y);
        }
    }

    public bool isScrolling
    {
        get { return _isInertia; }
    }

    public Vector2 CurrentVisibleRows
    {
        get
        {
            if (_currentVisibleRows == Vector2.Zero)
            {
                Vector2 vector = Position - _minScrollPoint;
                _currentVisibleRows = Vector2.Zero;
                if (_direction.Y < 0f)
                {
                    _currentVisibleRows.X = Mathf.Ceil(vector.Y / _cellSize.Y);
                }
                else
                {
                    _currentVisibleRows.X = Mathf.Floor(vector.Y / _cellSize.Y);
                }
                _currentVisibleRows.Y = _currentVisibleRows.X + _visibleRowsCount - 1f;
            }
            return _currentVisibleRows;
        }
    }

    public override void _Ready()
    {
        _viewSize = GetRect().Size;
        _lastPosition = Position;
    }

    public override void _Process(double delta)
    {
        if (_isInertia)
        {
            if (_inertiaTimeEnd > Engine.GetProcessTime())
            {
                float num = _inertiaTimeEnd - Engine.GetProcessTime();
                float t = _scrollInertiaCurve.Sample(num / _scrollInertiaTime);
                Position = _lastScrollLocalPoint.Lerp(_nextScrollLocalPoint, t);
            }
            else
            {
                _isInertia = false;
                Position = _nextScrollLocalPoint;
            }
            RestrictWithinBounds();
        }
        _currentVisibleRows = Vector2.Zero;
    }

    private void OnScrollDragStarted()
    {
        _lastPosition = Position;
    }

    private void OnScrollDragFinished()
    {
        Vector2 direction = Position - _lastPosition;
        if (Mathf.Abs(direction.Y) >= 1f)
        {
            _direction = direction;
            if (_direction.Y > 0f)
                ScrollDownByCell();
            else
                ScrollUpByCell();
            _isInertia = true;
        }
    }

    public void ScrollVerticalToCell(int cellRowNumber)
    {
        if (cellRowNumber >= CurrentVisibleRows.X && cellRowNumber <= CurrentVisibleRows.Y)
            return;

        int num = cellRowNumber < CurrentVisibleRows.X
            ? cellRowNumber - (int)CurrentVisibleRows.Y
            : cellRowNumber - (int)CurrentVisibleRows.X;

        _lastScrollLocalPoint = Position;
        _nextScrollLocalPoint = new Vector2(_lastScrollLocalPoint.X, _lastScrollLocalPoint.Y + num * _cellSize.Y);
        _nextScrollLocalPoint.Y = Mathf.Clamp(_nextScrollLocalPoint.Y, _minScrollPoint.Y, _maxScrollPoint.Y);

        _inertiaTimeEnd = Engine.GetProcessTime() + _scrollInertiaTime;
        _isInertia = true;
    }

    public void ScrollDownByCell()
    {
        _direction.Y = 1f;
        if (!(CurrentVisibleRows.Y >= _rowsCount - 1))
            ScrollVerticalToCell((int)CurrentVisibleRows.Y + 1);
    }

    public void ScrollUpByCell()
    {
        _direction.Y = -1f;
        if (!(CurrentVisibleRows.X <= 0f))
            ScrollVerticalToCell((int)CurrentVisibleRows.X - 1);
    }

    public void ResetPosition()
    {
        Position = Vector2.Zero;
        _isInertia = false;
    }

    public void InvalidateBounds()
    {
        _minScrollPoint = Position;
        _maxScrollPoint = new Vector2(_minScrollPoint.X, _minScrollPoint.Y);
        _maxScrollPoint.X += GetRect().Size.X - _viewSize.X + padding.X;
        _maxScrollPoint.Y += GetRect().Size.Y - _viewSize.Y + padding.Y;
        _visibleRowsCount = Mathf.FloorToInt(_viewSize.Y / _cellSize.Y);
        _rowsCount = Mathf.CeilToInt(GetRect().Size.Y / _cellSize.Y);
    }

    public void RecalculateBounds()
    {
        Vector2 localPosition = Position;
        ResetPosition();
        InvalidateBounds();
        disableRecalcBounds = true;
        Position = localPosition;
    }

    public CellVisibility GetVisibilityForCell(Control cell)
    {
        return GetVisibilityForCell(cell, _cellSize);
    }

    public CellVisibility GetVisibilityForCell(Control cell, Vector2 cellGraphicsSize)
    {
        if (cell.GetParent() != this)
            return CellVisibility.NONE;

        Vector2 vector = Position - _minScrollPoint;
        Vector2 vector2 = vector + _viewSize;
        Vector2 vector3 = (_cellSize - cellGraphicsSize) / 2f;
        Vector2 vector4 = -cell.Position;

        bool fullTop = vector4.Y + vector3.Y >= vector.Y;
        bool fullBottom = vector4.Y + _cellSize.Y - vector3.Y <= vector2.Y;

        if (fullTop)
        {
            if (fullBottom) return CellVisibility.FULL;
            return CellVisibility.TOP_PART;
        }
        if (fullBottom) return CellVisibility.BOTTOM_PART;
        return CellVisibility.NONE;
    }

    private void RestrictWithinBounds()
    {
        Position = new Vector2(
            Mathf.Clamp(Position.X, _minScrollPoint.X, _maxScrollPoint.X),
            Mathf.Clamp(Position.Y, _minScrollPoint.Y, _maxScrollPoint.Y)
        );
    }
}
