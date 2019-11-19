using Unity.Entities;
using Unity.Mathematics;

namespace Assets.Scripts.Attacks
{
    public struct ProjectileBody : IComponentData
    {
        public float3 Target { get; set; }
        public float Speed { get; set; }
        public int Id { get; set; }
    }
}