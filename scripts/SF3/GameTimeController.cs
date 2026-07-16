using Godot;

namespace SF3
{
    public static class GameTimeController
    {
        private const float DEFAULT_TIME_SCALE = 1f;

        public static float FIXED_DELTA_TIME;

        private static Vector3 _gravity;

        private static float _lastTimeScale;

        private static float _lastSystemTimeScale;

        private static bool _gamePaused;

        private static bool _systemPaused;

        private static bool _enableFlag;

        private static bool _disableFlag;

        public static float gameTimeDelta { get; private set; }

        public static float battleTime { get; private set; }

        public static float unscaledBattleTime { get; private set; }

        public static float deltaTime { get; private set; }

        public static float deltaTimePaused
        {
            get
            {
                return (!gamePaused) ? Engine.GetProcessDeltaTime() : 0f;
            }
        }

        public static int frameCount { get; private set; }

        public static float time { get; private set; }

        public static float timeScale { get; private set; }

        public static float unscaledTime { get; private set; }

        public static float unscaledDeltaTime { get; private set; }

        public static bool gamePaused
        {
            get
            {
                return _gamePaused || _systemPaused;
            }
        }

        public static bool systemPaused
        {
            get
            {
                return _systemPaused;
            }
        }

        static GameTimeController()
        {
            _gravity = PhysicsServer3D.Singleton.GetGravity();
            FIXED_DELTA_TIME = 1.0f / Engine.PhysicsTicksPerSecond;
            Reset();
        }

        public static void Reset()
        {
            _gamePaused = false;
            _systemPaused = false;
            ResetTimeScale();
            UpdateCache();
        }

        public static void SetPhysicTimeStamp(float stampValue)
        {
            FIXED_DELTA_TIME = stampValue;
            Engine.PhysicsTicksPerSecond = (uint)(1.0 / stampValue);
        }

        public static void UpdateBattleTime()
        {
            if (!_systemPaused)
            {
                UpdateCache();
            }
            if (!gamePaused)
            {
                battleTime += gameTimeDelta;
            }
        }

        private static void UpdateCache()
        {
            timeScale = Engine.TimeScale;
            deltaTime = Engine.GetProcessDeltaTime();
            frameCount++;
            time = Engine.GetProcessTime();
            unscaledTime += unscaledDeltaTime;
            float scale = Engine.TimeScale;
            unscaledDeltaTime = (scale > 0.001f) ? (Engine.GetProcessDeltaTime() / scale) : Engine.GetProcessDeltaTime();
        }

        public static void ResetTimeScale()
        {
            ChangeTimeScale(1f);
            _lastTimeScale = 1f;
            _lastSystemTimeScale = 1f;
        }

        public static void GameTimePause(bool pauseSounds = false)
        {
            if (!gamePaused)
            {
                _lastTimeScale = Engine.TimeScale;
                _gamePaused = true;
                ChangeGameTime(0f);
            }
        }

        public static void GameTimeResume()
        {
            if (gamePaused)
            {
                _gamePaused = false;
                ChangeGameTime(_lastTimeScale);
            }
        }

        public static void SystemTimePause()
        {
            if (!_systemPaused)
            {
                _systemPaused = true;
                _lastSystemTimeScale = Engine.TimeScale;
                Engine.TimeScale = 0f;
                timeScale = 0f;
            }
        }

        public static void SystemTimeResume()
        {
            if (_systemPaused)
            {
                _systemPaused = false;
                timeScale = _lastSystemTimeScale;
                Engine.TimeScale = _lastSystemTimeScale;
            }
        }

        public static void ChangeGameTime(float newTimeDelta)
        {
            ChangeTimeScale(newTimeDelta);
        }

        private static void ChangeTimeScale(float newTimeDelta)
        {
            float num4 = (gameTimeDelta = (Engine.TimeScale = (timeScale = Mathf.Clamp(newTimeDelta, 0f, 1f))));
            if (Engine.TimeScale > 0.01)
            {
                Engine.PhysicsTicksPerSecond = (uint)(1.0 / (FIXED_DELTA_TIME * num4));
                PhysicsServer3D.Singleton.SetGravity(_gravity / num4);
            }
            else
            {
                Engine.PhysicsTicksPerSecond = (uint)(1.0 / (FIXED_DELTA_TIME * 0.01f));
                PhysicsServer3D.Singleton.SetGravity(_gravity / 0.01f);
            }
        }
    }
}
