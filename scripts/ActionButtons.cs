using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Nekki;
using SF3;

public partial class ActionButtons : UIModuleHolder
{
    [Serializable]
    public partial class ActionUnit
    {
        [Export] public string Name;
        [Export] public EQuadrants Quadrant;
        [Export] public ActionButton AButton;
        [Export] public TextureRect Sprite;
        [Export] public TextureRect TutorTween;
        private float _baseAlpha;

        public bool Tutor { get; private set; }

        public bool Active
        {
            get => AButton.Visible;
            set
            {
                AButton.Visible = value;
                TutorTween.Visible = value;
            }
        }

        public void Init()
        {
            _baseAlpha = Sprite.Modulate.A;
        }

        public void SetTutor(EQuadrants[] array, bool activate)
        {
            if (array == null)
            {
                SetState(false);
                return;
            }
            foreach (EQuadrants q in array)
            {
                if (q == Quadrant)
                {
                    SetState(activate);
                    break;
                }
            }
        }

        private void SetState(bool state)
        {
            Tutor = state;
            Sprite.ZIndex = state ? 100 : 2;
            var c = Sprite.Modulate;
            c.A = state ? 0.9f : _baseAlpha;
            Sprite.Modulate = c;
            TutorTween.Visible = state;
        }
    }

    public const int OnButtonPress = 0;
    public const int OnButtonRelease = 1;

    private bool _active;
    [Export] public TextureRect shadowEnergyActiveEffect;
    [Export] public Node3D shadowEnergyRotator;
    public ActionUnit[] ActionUnits;

    private static Vector3 _sheduledScale = Vector3.One;
    private bool _tutorActive;

    public static ActionButtons Instance { get; private set; }

    public void InitializeButtons()
    {
        StopShadowFull();
    }

    public static void SetScale(float scale)
    {
        if (Instance != null)
            Instance.Scale = new Vector3(scale, scale, 1f);
        else
            _sheduledScale = new Vector3(scale, scale, 1f);
    }

    public List<TextureRect> SetTutor(EQuadrants[] buttons, bool active = true)
    {
        foreach (var unit in ActionUnits)
            unit.SetTutor(buttons, active);
        List<TextureRect> list = new List<TextureRect>();
        foreach (var unit in ActionUnits)
            list.Add(unit.TutorTween);
        return list;
    }

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
    }

    internal void Start()
    {
        foreach (var unit in ActionUnits)
            unit.Init();
        SetTutor(null);
        Scale = _sheduledScale;
        shadowEnergyActiveEffect.Visible = false;
        shadowEnergyRotator.Visible = false;
        GamepadController.Instance.SubscribeUIElement(this);
        if (NekkiUtils.IsPhone())
            SetScale(1.3f);
    }

    public ActionUnit GetActionButtonByName(string name)
    {
        var unit = ActionUnits.FirstOrDefault(b => b.Name.Equals(name));
        if (unit == null)
            GD.PrintErr($"No button by name [{name}] found.");
        return unit;
    }

    public void ActionButtonHide(EQuadrants key, bool hide)
    {
        switch (key)
        {
            case EQuadrants.Punch: PunchButtonEnable(!hide); break;
            case EQuadrants.Kick: KickButtonEnable(!hide); break;
            case EQuadrants.Missile: MissileButtonEnable(!hide); break;
            case EQuadrants.Magic: ShadowButtonEnable(!hide); break;
        }
    }

    public void MissileButtonEnable(bool isEnable) => ActionButtonsEnable(isEnable, "Missile");
    public void ShadowButtonEnable(bool isEnable) => ActionButtonsEnable(isEnable, "Magic");
    public void KickButtonEnable(bool isEnable) => ActionButtonsEnable(isEnable, "Kick");
    public void PunchButtonEnable(bool isEnable) => ActionButtonsEnable(isEnable, "Punch");

    public void ActionButtonsEnable(bool isEnable) => ActionButtonsEnable(isEnable, "Missile", "Magic", "Kick", "Punch");

    private void ActionButtonsEnable(bool isEnable, params string[] buttonNames)
    {
        foreach (string name in buttonNames)
        {
            var unit = ActionUnits.FirstOrDefault(u => u.Name.Equals(name));
            if (unit != null)
                unit.AButton.Visible = isEnable;
        }
    }

    public static void PlayShadowFull()
    {
        if (Instance != null)
            Instance.shadowEnergyActiveEffect.Visible = true;
    }

    public void StopShadowFull()
    {
        shadowEnergyActiveEffect.Visible = false;
    }

    public static void SetActive(bool active)
    {
        Instance._active = active;
        Instance.Visible = active;
        foreach (var unit in Instance.ActionUnits)
            unit.Active = active;
    }

    public static bool QuadrantPressed(EQuadrants quadrant)
    {
        foreach (var unit in Instance.ActionUnits)
        {
            if (unit.Tutor && unit.Quadrant == quadrant)
                return true;
        }
        return false;
    }
}
