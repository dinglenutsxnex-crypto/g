using System.Collections.Generic;
using System.Linq;
using Godot;
using Nekki;

namespace SF3.GameModels
{
    public class ModelCapsules
    {
        private readonly List<Node3D> _renderers = new List<Node3D>();
        private bool _shouldRender;
        private Dictionary<string, ShaderMaterial> _cachedMaterials = new Dictionary<string, ShaderMaterial>();
        private List<Capsule> _capsules { get; set; }
        public List<Capsule> collisionCapsules { get; private set; }
        public List<Capsule> repulsiveCapsules { get; private set; }
        public Dictionary<int, List<Capsule>> attackingCapsules { get; private set; }
        public RepulsionRect repulsionRect { get; private set; }
        public RepulsionRect floorRepulsion { get; private set; }
        public static bool ShowCapsules { get; set; }

        public ModelCapsules()
        {
            _capsules = new List<Capsule>();
            collisionCapsules = new List<Capsule>();
            repulsiveCapsules = new List<Capsule>();
            attackingCapsules = new Dictionary<int, List<Capsule>>();
            CheckShowCapsules();
        }

        public void Clear()
        {
            _capsules.Clear();
            collisionCapsules.Clear();
            repulsiveCapsules.Clear();
            attackingCapsules.Clear();
            floorRepulsion = null;
            repulsionRect = null;
        }

        public void AddCapsule(Capsule newCapsule)
        {
            _capsules.Add(newCapsule);
            InitCapsule(newCapsule);
        }

        public void AddCapsules(List<Capsule> newCapsule)
        {
            _capsules.AddRange(newCapsule);
            foreach (var c in newCapsule)
                InitCapsule(c);
        }

        public void SetRepulsionRect(Dictionary<string, Node3D> transforms, float widthSc, float heightSc, float minSize)
        {
            repulsionRect = new RepulsionRect(transforms, widthSc, heightSc, minSize);
        }

        public void SetFloorRepulsion(List<Node3D> transforms, float widthSc, float heightSc)
        {
            floorRepulsion = new RepulsionRect(transforms, widthSc, heightSc);
        }

        private void InitCapsule(Capsule capsule)
        {
            if (capsule.collisible)
                collisionCapsules.Add(capsule);
            if (capsule.repulsive)
                repulsiveCapsules.Add(capsule);
        }

        public Capsule GetCapsule(string capsuleName)
        {
            foreach (var capsule in _capsules)
            {
                if (capsule.name.Equals(capsuleName))
                    return capsule;
            }
            return null;
        }

        public void ClearAttackingCapsules()
        {
            if (attackingCapsules.Count != 0)
                attackingCapsules.Clear();
        }

        public void ClearAttackingCapsules(int intervalId)
        {
            if (attackingCapsules.ContainsKey(intervalId))
                attackingCapsules.Remove(intervalId);
        }

        public void AddAttackingCapsules(int intervalId, List<Capsule> capsules)
        {
            attackingCapsules.Add(intervalId, GetAttackingCapsules(capsules));
        }

        private List<Capsule> GetAttackingCapsules(List<Capsule> capsules)
        {
            List<Capsule> list = new List<Capsule>();
            foreach (var capsule in capsules)
            {
                foreach (var c in _capsules)
                {
                    if (capsule.name.Equals(c.name))
                    {
                        list.Add(c);
                        break;
                    }
                }
            }
            return list;
        }

        public void UpdateEquationLineCollision()
        {
            if (attackingCapsules != null)
            {
                foreach (var kvp in attackingCapsules)
                {
                    foreach (var c in kvp.Value)
                        c.RenderEquationLine();
                }
            }
            if (collisionCapsules != null)
            {
                foreach (var c in collisionCapsules)
                    c.RenderEquationLine();
            }
            if (repulsiveCapsules != null)
            {
                foreach (var c in repulsiveCapsules)
                    c.RenderEquationLine();
            }
        }

        public void RenderCapsules(bool enable)
        {
            _shouldRender = enable;
        }

        private void CheckShowCapsules()
        {
            _shouldRender = true;
        }
    }
}
