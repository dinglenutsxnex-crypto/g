using System;
using System.Collections.Generic;
using SF3;
using SF3.Audio;
using SF3.Items;
using SF3.UserData;
using Godot;

public class ReelDriver : Node
{
	public delegate void ItemSelectedEventHandler(BaseItem item);
	public delegate void ItemInsertedEventHadler(BaseItem item, PerkSlot slot);

	public class SplineObject
	{
		public Node Obj;
		public BaseItem Item;
		private float _lastPosition;
		private ReelDriver _reel;

		public int ID { get { return Item.id; } }

		public float PositionOnCurve
		{
			get
			{
				if ((_reel._inDrag || _reel._dragBack) && !Dragged)
				{
					float num = ((Node3D)_reel._dragedItem.Obj).Position.Length();
					Vector2 screenSize = DisplayServer.WindowGetSize();
					float num2 = Mathf.Sqrt(screenSize.X * screenSize.X + screenSize.Y * screenSize.Y);
					float num3 = (num > num2 / 10f) ? 1f : (num / (num2 / 10f));
					if (_reel._dragedItem.Index == 0)
					{
						return (float)Index * _reel._cellSize + _reel._ajust - num3 * _reel._cellSize;
					}
					if (_reel._dragedItem.Index == _reel.Count - 1)
					{
						return (float)Index * _reel._cellSize + _reel._ajust + num3 * _reel._cellSize;
					}
					if (Index < _reel._dragedItem.Index)
					{
						return (float)Index * _reel._cellSize + _reel._ajust + num3 * _reel._cellSize;
					}
				}
				return (float)Index * _reel._cellSize + _reel._ajust;
			}
		}

		public CardItem ReelItem { get; private set; }
		public int Index { get; private set; }
		public bool Dragged { get; private set; }

		public SplineObject(ReelDriver reel, Node go, int index, BaseItem item)
		{
			Obj = go;
			Item = item;
			_reel = reel;
			go.Name = ID + "_" + index;
			Index = index;
			ReelItem = Obj.GetNode<CardItem>(".");
			ReelItem.Init(item);
			UpdatePosition();
		}

		public bool UpdatePosition()
		{
			bool result = false;
			if (Dragged) return result;
			float num = Mathf.Abs(_lastPosition - PositionOnCurve);
			if (((_lastPosition > 0.5f && PositionOnCurve <= 0.5f) || (_lastPosition < 0.5f && PositionOnCurve >= 0.5f)) && num < _reel._cellSize)
			{
				result = true;
			}
			_lastPosition = PositionOnCurve;
			Node3D n = Obj as Node3D;
			n.Position = _reel.PositionAt(PositionOnCurve) * _reel._positionScale;
			n.Rotation = _reel.RotationAt(PositionOnCurve) * _reel._rotationScale;
			n.Scale = Vector3.One * _reel._scale.Sample(PositionOnCurve);
			ReelItem.UpdateDepth((int)(_reel._posZ.Sample(PositionOnCurve) * -100f));
			ReelItem.UpdateShade(_reel._shade.Sample(PositionOnCurve));
			ReelItem.UpdateItem();
			return result;
		}

		public void StartDrag()
		{
			Dragged = true;
			ReelItem.Drag(true);
			Obj.GetParent()?.RemoveChild(Obj);
			_reel._dragPivot.AddChild(Obj);
			((Node3D)Obj).Position = Vector3.Zero;
			_reel._startDragScale = ((Node3D)Obj).Scale;
			((Node3D)Obj).Scale = Vector3.One * _reel._dragScale;
			Obj.Visible = false;
			Obj.Visible = true;
		}

		public void EndDrag()
		{
			Dragged = false;
			ReelItem.Drag(false);
			UpdatePosition();
		}

		public void SetIndex(int index)
		{
			Index = index;
		}
	}

	[Export]
	private Curve _posX;
	[Export]
	private Curve _posY;
	[Export]
	private Curve _posZ;
	[Export]
	private Curve _rotationX;
	[Export]
	private Curve _rotationY;
	[Export]
	private Curve _rotationZ;
	[Export]
	private Curve _shade;
	[Export]
	private Curve _scale;
	[Export]
	private Vector2 _cameraPositionOffset;
	[Export]
	private Vector2 _camera4x3PositionOffset;
	[Export]
	private float _positionScale = 10f;
	[Export]
	private float _rotationScale = 360f;
	[Export]
	private float _swipeScale = 5f;
	[Export]
	private float _centerOnSpeed = 1f;
	[Export]
	private float _cellSize = 0.2f;
	[Export]
	private Node _prototype;
	[Export]
	private Camera3D _reelCamera;
	[Export]
	private Camera3D _uiCamera;
	[Export]
	private string _reelLayer;
	[Export]
	private string _uiLayer;
	[Export]
	private float _dragDellay;
	private float _velocity;
	private bool _isAnim;
	[Export]
	private Node3D _dragPivot;
	[Export]
	private float _dragScale;
	[Export]
	private float _velocityTolerance;
	[Export]
	private string _cardSwipeSound;
	[Export]
	private string _cardClickSound;
	[Export]
	private Node _sellPivot;
	[Export]
	private Node3D _controlContainer;
	[Export]
	private Node _progressbarPrf;
	[Export]
	private float progressYOffset = -200f;
	[Export]
	private Node _leftBtnPrf;
	[Export]
	private Node _rightBtnPrf;
	[Export]
	private float _xOffset = -200f;
	[Export]
	private float _yOffset = -200f;

	private Button _leftBtn;
	private Button _rightBtn;
	private ProgressBar _progressbar;
	private List<SplineObject> _pool = new List<SplineObject>();
	private int _centerOnIndex = -1;
	private float _ajust;
	private bool _inSwipe;
	private bool _inDrag;
	private bool _dragBack;
	private bool _canStartDrag;
	private bool _allowDrag;
	private Vector3 _startDragScale;
	private Vector3 _lastSwipePosition;
	private Vector3 _firstSwipePosition;
	private SplineObject _dragedItem;

	public BaseItem Selected { get; private set; }
	public bool Moving { get { return _centerOnIndex >= 0; } }
	public int Count { get { return _pool.Count; } }

	public event ItemSelectedEventHandler ItemSelected;
	public event ItemSelectedEventHandler ItemStartChanging;
	public event ItemSelectedEventHandler ItemSwiped;
	public event ItemInsertedEventHadler ItemInserted;
	public event Action Cleared;

	public override void _Ready()
	{
		_prototype.Visible = false;
		_sellPivot.Visible = false;
		Vector2 screenSize = DisplayServer.WindowGetSize();
		if (_progressbarPrf != null && _progressbar == null)
		{
			CreateProgress();
			UpdateProgress();
		}
		if (_leftBtnPrf != null && _rightBtnPrf != null && _leftBtn == null && _rightBtn == null)
		{
			CreateButtons();
		}
	}

	public override void _Process(double delta)
	{
		SwipeUpdate();
		if (!VelocityMove())
		{
			FocusOnClose();
		}
	}

	public void FocusOnItem(int itemId, bool instant)
	{
		FocusOn(GetIndexOf(itemId), instant);
	}

	public void FocusOn(int id, bool instant)
	{
		_centerOnIndex = id;
		if (instant)
		{
			float amount = 0.5f - _pool[id].PositionOnCurve;
			OnItemSelected(_pool[id].Item);
			Move(amount);
			_centerOnIndex = -1;
		}
		Visible = true;
	}

	private int GetIndexOf(int itemId)
	{
		for (int i = 0; i < _pool.Count; i++)
		{
			if (_pool[i].ID == itemId) return i;
		}
		return -1;
	}

	public bool CheckTutorialDragable(ReelItem item)
	{
		if (UIBlocker.Instance.IsLocked)
		{
			TutorialComponent component = item.GetNode<TutorialComponent>(".");
			if (!component.isDragable) return false;
		}
		return true;
	}

	private void SwipeUpdate()
	{
		if ((GameTimeController.gamePaused && !UIBlocker.Instance.IsLocked) || _dragBack) return;

		float num = 0f;
		Vector2 mousePos = GetViewport().GetMousePosition();

		if (Input.IsMouseButtonPressed(MouseButton.Left) && !_isAnim)
		{
			if (!_inSwipe)
			{
				_centerOnIndex = -1;
				_inSwipe = true;
				_lastSwipePosition = mousePos;
				_firstSwipePosition = _lastSwipePosition;
				_canStartDrag = true;
			}
		}
		else if (!Input.IsMouseButtonPressed(MouseButton.Left))
		{
			_inSwipe = false;
			if (_inDrag)
			{
				ProcessSlotOrDistruction();
			}
			else if (_canStartDrag)
			{
				FindCloseTarget();
				FocusOnClose();
			}
			else if (Mathf.Abs(_velocity) > 1E-06f)
			{
				if (_velocity > 0f)
				{
					for (int num2 = _pool.Count - 1; num2 >= 0; num2--)
					{
						if (_pool[num2].PositionOnCurve + _velocity <= 0.5f)
						{
							_velocity += 0.5f - (_pool[num2].PositionOnCurve + _velocity);
							break;
						}
					}
				}
				else
				{
					for (int i = 0; i < _pool.Count; i++)
					{
						if (_pool[i].PositionOnCurve + _velocity >= 0.5f)
						{
							_velocity += 0.5f - (_pool[i].PositionOnCurve + _velocity);
							break;
						}
					}
				}
			}
			else
			{
				FocusOnClose();
			}
		}

		if (_inSwipe || _inDrag)
		{
			if (_canStartDrag && mousePos.DistanceTo(_firstSwipePosition) > (float)DisplayServer.WindowGetSize().X / 100f)
			{
				_canStartDrag = false;
				ItemStartChanging?.Invoke(Selected);
			}
			Vector2 vector = new Vector2((mousePos.X - _lastSwipePosition.X) / (float)DisplayServer.WindowGetSize().X, (mousePos.Y - _lastSwipePosition.Y) / (float)DisplayServer.WindowGetSize().Y);
			if (_inDrag)
			{
				if (_dragedItem != null)
				{
					((Node3D)_dragedItem.Obj).Position = new Vector3(mousePos.X, mousePos.Y, 0f);
				}
				_lastSwipePosition = mousePos;
				return;
			}
			num = vector.X * _swipeScale;
			_lastSwipePosition = mousePos;
		}
		if (!UIBlocker.Instance.IsLocked)
		{
			_velocity += num;
		}
	}

	private void ProcessSlotOrDistruction()
	{
		StopDragItem(false);
	}

	private SplineObject GetObjectFor(ReelItem item)
	{
		foreach (SplineObject item2 in _pool)
		{
			if (item2.ReelItem == item) return item2;
		}
		return null;
	}

	public void ChangeType(EquipmentType type, List<BaseItem> items = null, List<BaseItem> sloted = null)
	{
		ChangeType(_allowDrag, type, items, sloted);
	}

	public void ChangeType(bool allowDrag, EquipmentType type, List<BaseItem> items = null, List<BaseItem> sloted = null, bool refreshImageSack = true)
	{
		_allowDrag = allowDrag;
		Clear();
		if (items == null || items.Count == 0)
		{
			items = UserManager.UserModelInfo.GetItemsOfType(type);
		}
		items.Sort(ItemComparators.GetComparator(ComparerPurpose.ReelDriverDesc));
		if (refreshImageSack)
		{
			InventoryManager.Instance.UpdateSubtypesIcons();
		}
		for (int i = 0; i < items.Count; i++)
		{
			_pool.Add(CreateCard(items[i], i));
		}
		UpdateButtonState((Selected != null) ? GetIndexOf(Selected.id) : 0);
		UpdateProgress();
	}

	private void Clear()
	{
		foreach (SplineObject item in _pool)
		{
			item.Obj.QueueFree();
		}
		_pool.Clear();
		Cleared?.Invoke();
	}

	private bool Move(float amount)
	{
		if (!_inDrag && !_dragBack)
		{
			if (Mathf.Abs(amount) < 1E-05f) return false;
			_ajust = Mathf.Clamp(_ajust + amount, (0f - _cellSize) * (float)_pool.Count + 0.5f + _cellSize, 0.5f);
		}
		foreach (SplineObject item in _pool)
		{
			if (item.UpdatePosition())
			{
				AudioManager.Instance.PlaySound(_cardSwipeSound);
				ItemSwiped?.Invoke(item.Item);
			}
		}
		return true;
	}

	private bool VelocityMove()
	{
		if (Mathf.Approximately(_velocity, 0f)) return false;
		float num = Mathf.Clamp(_ajust + _velocity * 0.2f, (0f - _cellSize) * (float)_pool.Count + 0.5f + _cellSize, 0.5f);
		_velocity -= _velocity * 0.2f;
		if (Math.Abs(num - ((0f - _cellSize) * (float)_pool.Count + 0.5f + _cellSize)) < 1E-05f || Math.Abs(num - 0.5f) < _velocityTolerance)
		{
			_velocity = 0f;
		}
		_ajust = num;
		if (Mathf.Abs(_velocity) < 0.0001f || (!_inSwipe && !_inDrag && Mathf.Abs(_velocity) < 0.1f))
		{
			_velocity = 0f;
			FindCloseTarget();
		}
		foreach (SplineObject item in _pool)
		{
			if (item.UpdatePosition())
			{
				AudioManager.Instance.PlaySound(_cardSwipeSound);
				ItemSwiped?.Invoke(item.Item);
			}
		}
		return true;
	}

	private void FindCloseTarget(float close = 0f)
	{
		float num = 2f;
		_centerOnIndex = -1;
		for (int i = 0; i < _pool.Count; i++)
		{
			float num2 = Mathf.Abs(_pool[i].PositionOnCurve - 0.5f);
			if (num2 < num)
			{
				_centerOnIndex = i;
				num = num2;
			}
		}
	}

	private void FocusOnClose()
	{
		if (_centerOnIndex != -1)
		{
			if (_canStartDrag)
			{
				_canStartDrag = false;
				ItemStartChanging?.Invoke(Selected);
			}
			_isAnim = true;
			float max = Mathf.Abs(_pool[_centerOnIndex].PositionOnCurve - 0.5f);
			float num = Mathf.Clamp(_centerOnSpeed * (float)Engine.GetProcessDeltaTime(), 0f, max);
			if (_pool[_centerOnIndex].PositionOnCurve > 0.5f)
			{
				Move(num * -1f);
			}
			else if (_pool[_centerOnIndex].PositionOnCurve < 0.5f)
			{
				Move(num);
			}
			if (Math.Abs(_pool[_centerOnIndex].PositionOnCurve - 0.5f) < 0.001f)
			{
				OnItemSelected(_pool[_centerOnIndex].Item);
				_centerOnIndex = -1;
			}
		}
		else
		{
			_isAnim = false;
		}
	}

	private Vector3 PositionAt(float splinePosition)
	{
		splinePosition = Mathf.Clamp(splinePosition, 0f, 1f);
		return new Vector3(_posX.Sample(splinePosition), _posY.Sample(splinePosition), _posZ.Sample(splinePosition));
	}

	private Vector3 RotationAt(float splinePosition)
	{
		splinePosition = Mathf.Clamp(splinePosition, 0f, 1f);
		return new Vector3(_rotationX.Sample(splinePosition), _rotationY.Sample(splinePosition), _rotationZ.Sample(splinePosition));
	}

	private void OnItemSelected(BaseItem item)
	{
		if (item != Selected)
		{
			Selected = item;
		}
		UpdateProgress();
		UpdateButtonState(GetIndexOf(Selected.id));
		_isAnim = false;
		_canStartDrag = false;
		ItemSelected?.Invoke(item);
	}

	public void FocusOnCenter()
	{
		if (Count > 0)
		{
			int index = Count / 2;
			FocusOnItem(_pool[index].ID, true);
		}
		else
		{
			Selected = null;
			_centerOnIndex = -1;
		}
	}

	public void FocusOnMostSuitableThing()
	{
		if (Count <= 0) return;
		int num = -1;
		int num2 = -1;
		for (int i = 0; i < _pool.Count; i++)
		{
			IInformable informable = _pool[i].Item as IInformable;
			if (informable != null && informable.IsNew())
			{
				num = _pool[i].ID;
			}
			if (_pool[i].Item is Equipment && ((Equipment)_pool[i].Item).IsEquipped())
			{
				num2 = _pool[i].ID;
			}
		}
		if (num == -1)
		{
			num = (num2 != -1) ? num2 : _pool[0].ID;
		}
		else
		{
			foreach (SplineObject item in _pool)
			{
				IInformable informable2 = item.Item as IInformable;
				if (informable2 != null && informable2.IsNew())
				{
					informable2.SetNew(false);
					UserManager.UpdateEquipmentData(item.Item.id);
				}
			}
			Equipment equipment = _pool[0].Item as Equipment;
			if (equipment != null)
			{
				InventoryManager.Instance.UpdateSlot(equipment.GetEquipmentType());
			}
		}
		FocusOnItem(num, true);
	}

	private void StartDragItem(SplineObject item)
	{
		_dragedItem = item;
		_dragedItem.StartDrag();
		_inDrag = true;
	}

	private void StopDragItem(bool removeDragged, Action onTweenFinish = null)
	{
		_sellPivot.Visible = false;
		if (removeDragged)
		{
			if (_dragedItem != null)
			{
				_dragedItem.ReelItem.Destruct(onTweenFinish);
				_pool.Remove(_dragedItem);
				for (int i = 0; i < _pool.Count; i++)
				{
					_pool[i].SetIndex(i);
				}
				_centerOnIndex = _dragedItem.Index;
				if (_centerOnIndex >= _pool.Count - 1)
				{
					_centerOnIndex = _pool.Count - 1;
				}
			}
			_inDrag = false;
		}
		else if (_dragedItem != null)
		{
			ReturnItemToTheReel();
		}
		else
		{
			_inDrag = false;
		}
	}

	private SplineObject CreateCard(BaseItem item, int index = -1)
	{
		if (index == -1)
		{
			for (int i = 0; i < _pool.Count; i++)
			{
				if (index != -1)
				{
					_pool[i].SetIndex(_pool[i].Index + 1);
				}
			}
			if (index == -1)
			{
				index = _pool.Count;
			}
		}
		Node gameObject = _prototype.Duplicate();
		gameObject.GetParent()?.RemoveChild(gameObject);
		AddChild(gameObject);
		((Node3D)gameObject).Scale = ((Node3D)_prototype).Scale;
		((Node3D)gameObject).Position = ((Node3D)_prototype).Position;
		gameObject.Visible = true;
		return new SplineObject(this, gameObject, index, item);
	}

	private void InsertItemAndDrag(BaseItem item, Action onAppear)
	{
		if (item != null)
		{
			SplineObject splineObject = CreateCard(item);
			_pool.Insert(splineObject.Index, splineObject);
			OnItemSelected(item);
			FocusOnItem(item.id, true);
			splineObject.Obj.Visible = false;
			InsertAndDragRoutine(splineObject, onAppear);
		}
	}

	private async void InsertAndDragRoutine(SplineObject so, Action onAppear)
	{
		for (int i = 0; i < 3; i++)
		{
			await ToSignal(Engine.GetMainLoop(), "process_frame");
		}
		_dragedItem = so;
		_inDrag = true;
		_dragedItem.StartDrag();
		_dragedItem.ReelItem.UpdateShade(0f);
		_dragedItem.ReelItem.UpdateDepth(10);
		so.Obj.Visible = true;
		_dragedItem.ReelItem.Appear(onAppear);
	}

	private async void ReturnItemToTheReel()
	{
		_dragBack = true;
		for (int i = 0; i < 20; i++)
		{
			await ToSignal(Engine.GetMainLoop(), "process_frame");
			((Node3D)_dragedItem.Obj).Position = ((Node3D)_dragedItem.Obj).Position.Lerp(_dragPivot.Position, (float)i / 20f);
			((Node3D)_dragedItem.Obj).Scale = ((Node3D)_dragedItem.Obj).Scale.Lerp(_startDragScale, (float)i / 20f);
		}
		_inDrag = false;
		_dragBack = false;
		_dragedItem.Obj.GetParent()?.RemoveChild(_dragedItem.Obj);
		AddChild(_dragedItem.Obj);
		_dragedItem.EndDrag();
	}

	private void CreateProgress()
	{
		Node gameObject = _progressbarPrf.Duplicate();
		_progressbar = gameObject.GetNode<ProgressBar>(".");
		gameObject.GetParent()?.RemoveChild(gameObject);
		_controlContainer.AddChild(gameObject);
		((Node3D)gameObject).Scale = Vector3.One;
		gameObject.Visible = false;
	}

	private void UpdateProgress()
	{
		if (_progressbarPrf == null || _progressbar == null) return;
		if (_pool.Count == 0)
		{
			_progressbar.Visible = false;
		}
		else
		{
			if (Selected == null) return;
			_progressbar.Visible = true;
			if (Selected is Equipment)
			{
				Equipment equipment = Selected as Equipment;
				_progressbar.Value = equipment.GetBarValue() * 100f;
			}
			else if (Selected is Perk)
			{
				Perk perk = Selected as Perk;
				if (perk != null)
				{
					_progressbar.Value = perk.GetBarValue() * 100f;
				}
			}
		}
	}

	private void CreateButtons()
	{
		Node gameObject = _leftBtnPrf.Duplicate();
		_leftBtn = gameObject.GetNode<Button>(".");
		gameObject.GetParent()?.RemoveChild(gameObject);
		_controlContainer.AddChild(gameObject);
		((Node3D)gameObject).Scale = Vector3.One;
		gameObject.Visible = false;
		_leftBtn.Pressed += leftBtnOnClick;

		gameObject = _rightBtnPrf.Duplicate();
		_rightBtn = gameObject.GetNode<Button>(".");
		gameObject.GetParent()?.RemoveChild(gameObject);
		_controlContainer.AddChild(gameObject);
		((Node3D)gameObject).Scale = Vector3.One;
		gameObject.Visible = false;
		_rightBtn.Pressed += rightBtnOnClick;
	}

	public void leftBtnOnClick()
	{
		int indexOf = GetIndexOf(Selected.id);
		if (indexOf != 0)
		{
			indexOf--;
			UpdateButtonState(indexOf);
			FocusOn(_pool[indexOf].Index, false);
		}
	}

	public void rightBtnOnClick()
	{
		int indexOf = GetIndexOf(Selected.id);
		if (indexOf != _pool.Count - 1)
		{
			indexOf++;
			UpdateButtonState(indexOf);
			FocusOn(indexOf, false);
		}
	}

	private void UpdateButtonState(int current)
	{
		if ((_leftBtnPrf == null && _rightBtnPrf == null) || (_leftBtn == null && _rightBtn == null)) return;
		if (_pool.Count == 0)
		{
			_leftBtn.Visible = false;
			_rightBtn.Visible = false;
			return;
		}
		_leftBtn.Visible = current != 0;
		_rightBtn.Visible = current != _pool.Count - 1;
	}

	public void DisableReelCamera()
	{
		_reelCamera.Visible = false;
	}

	public void EnableReelCamera()
	{
		_reelCamera.Visible = true;
	}
}
