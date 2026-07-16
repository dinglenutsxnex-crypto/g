using System;
using Godot;
using SF3.GameModels;

namespace SF3.Effects
{
    public class CustomEffect : GameEffectBase
    {
        [Serializable]
        public class DelayedEffectActivation
        {
            public Node3D effect;
            public float delay;
            private float _nextEffectActivation;
            private bool _done;

            public void Init()
            {
                effect.Visible = false;
                _nextEffectActivation = GameTimeController.battleTime + delay;
                _done = false;
            }

            public void Update()
            {
                if (!_done && _nextEffectActivation < GameTimeController.battleTime)
                {
                    effect.Visible = true;
                    _done = true;
                }
            }
        }

        public bool takeInAccountCharacterDirection;
        [Export] private bool _flipByAccountCharacterDirection;
        private GpuParticles3D _particlesEffect;
        public float duration;
        public bool overrideDuration;
        private Vector3 _followAxis;
        private float _timer;
        private bool _checkOnDisable;
        private Bone _bone;
        private Model _model;
        private string _attachToBone;
        private bool _follow;
        private Vector3 _shift;
        private Vector3 _currentShift;
        public DelayedEffectActivation[] delayedEffects;
        private EDirectionType _currentParentModelDirection;
        private bool attachToLocal;

        public override void _Ready()
        {
            base._Ready();
            _particlesEffect = GetNode<GpuParticles3D>(".");
            var componentsInChildren = GetComponentsInChildren<GpuParticles3D>();
            if (!overrideDuration)
            {
                duration = 0f;
                foreach (var ps in componentsInChildren)
                {
                    float d = ps.Lifetime;
                    if (duration < d) duration = d;
                }
            }
        }

        private void OnDisable()
        {
            if (_model != null)
                _model.moveControl.OnDirectionChangedEvent -= OnModelDirectionChanged;
        }

        public override void Play(Model m, Vector3 atPos, Vector3 freezeAxisValue)
        {
            base.Play(m, atPos);
            _timer = 0f;
            _checkOnDisable = true;
            InitDelayedEffects();
            _followAxis = freezeAxisValue;
        }

        private void InitDelayedEffects()
        {
            if (delayedEffects != null)
            {
                foreach (var de in delayedEffects)
                    de.Init();
            }
        }

        public override void Play(Model m, Vector3 atPos)
        {
            Play(m, atPos, Vector3.Zero);
        }

        public override void Play(Model m, Vector3 angle, bool applyToEnemy, string attachToBone, bool loop, bool follow, Vector3 shift, Vector3 followAxis, bool attachLocal)
        {
            base.Play(m, Vector3.Zero);
            if (_particlesEffect != null)
            {
                _particlesEffect.Restart();
            }
            attachToLocal = attachLocal;
            _attachToBone = attachToBone;
            _follow = follow;
            _shift = shift;
            _model = (!applyToEnemy) ? m : model.enemy;
            UpdateBone();
            if (!loop)
            {
                _timer = 0f;
                _checkOnDisable = true;
            }
            InitDelayedEffects();
            _followAxis = followAxis;
            _model.moveControl.OnDirectionChangedEvent += OnModelDirectionChanged;
            OnModelDirectionChanged(_model.moveControl.moveDirection);
            Position = _bone.transform.Position + _currentShift;
            var blackFire = GetNode<BlackFireSkin>(".");
            if (blackFire != null)
                blackFire.Init(_bone.transform.GameObject, model.GetBone("pelvis").transform, m);
        }

        public override void Play(Model m, Vector3 angle, bool applyToEnemy, string attachToBone, bool loop, bool follow, Vector3 shift)
        {
            Play(m, angle, applyToEnemy, attachToBone, loop, follow, shift, Vector3.Zero, false);
        }

        private void UpdateBone()
        {
            _bone = model.GetBone(_attachToBone);
        }

        private void OnModelDirectionChanged(EDirectionType newDirection)
        {
            _currentShift = _shift;
            Vector3 localEulerAngles = defaultAngles;
            Vector3 localScale = defaultScale;
            if (newDirection != EDirectionType.RIGHT && newDirection == EDirectionType.LEFT)
            {
                if (takeInAccountCharacterDirection)
                {
                    localEulerAngles.Y = 180f - localEulerAngles.Y;
                    _currentShift.X *= -1f;
                    localScale.Z = 0f - localScale.Z;
                }
                if (_flipByAccountCharacterDirection)
                    localScale.X = 0f - localScale.X;
            }
            Rotation = Quaternion.FromEuler(localEulerAngles * Mathf.Deg2Rad);
            Scale = localScale;
        }

        protected void OnUpdate(double delta)
        {
            if (delayedEffects != null)
            {
                foreach (var de in delayedEffects)
                    de.Update();
            }
            if (_follow && _bone != null && _bone.transform != null)
            {
                Vector3 position = _bone.transform.Position + _currentShift;
                if (_followAxis.X > 1f || _followAxis.X < 1f)
                    position.X = Position.X;
                if (_followAxis.Y > 1f || _followAxis.Y < 1f)
                    position.Y = Position.Y;
                if (_followAxis.Z > 1f || _followAxis.Z < 1f)
                    position.Z = Position.Z;
                Position = position;
            }
            if (_checkOnDisable)
            {
                _timer += GameTimeController.deltaTime;
                if (_timer >= duration)
                {
                    _checkOnDisable = false;
                    Visible = false;
                }
            }
            else if (_follow && (model == null || !model.active))
            {
                _timer = 0f;
                _checkOnDisable = true;
            }
            if (attachToLocal)
            {
                Position = _bone.transform.Position + defaultPos;
                Rotation = _bone.transform.Basis.GetRotationQuaternion() * Quaternion.FromEuler(defaultAngles * Mathf.Deg2Rad);
            }
        }
    }
}
