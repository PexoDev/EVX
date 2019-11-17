using UnityEngine;

namespace Assets.Scripts.Units
{
    public abstract class Entity
    {
        public Vector2 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public float Speed { get; set; }

        public float CalculateDistance(Entity target)
        {
            return Vector2.Distance(Position, target.Position);
        }

        public float CalculateDistance(Vector2 target)
        {
            return Vector2.Distance(Position, target);
        }

        public bool IsInRange(Vector2 target, float range)
        {
            return CalculateDistance(target) <= range;
        }

        public virtual void Move(Vector2 newPosition, Vector3 rotation)
        {
            Position = newPosition;
            Rotation = rotation;
        }

        public virtual void Move(Vector2 newPosition)
        {
            Position = newPosition;
        }
    }
}
