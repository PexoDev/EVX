using System;
using Assets.Scripts.Units;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Assets.Scripts.Attacks
{
    public class Projectile
    {
        public Entity Entity;
        public ProjectileBody Body => _em.GetComponentData<ProjectileBody>(Entity);
        public Translation Position => _em.GetComponentData<Translation>(Entity);

        public bool InUse { get; set; }
        public IAttack Attack { get; set; }
        public LivingEntity Attacker { get; set; }
        public LivingEntity Target { get; set; }

        private EntityManager _em;

        public Projectile(EntityManager em, IAttack attackData, LivingEntity attacker, LivingEntity target, int poolId, Mesh meshTexture, Material mat)
        {
            Target = target;
            Attack = attackData;
            Attacker = attacker;
            _em = em;

            Entity = em.CreateEntity(typeof(ProjectileBody), typeof(Translation), typeof(Rotation), typeof(Scale), typeof(LocalToWorld),
                typeof(RenderMesh));
            em.SetComponentData(Entity, new ProjectileBody
            {
                Target = new float3(target.Position.x, target.Position.y, 0),
                Speed = 5f * Time.fixedDeltaTime,
                Id = poolId
            });
            em.SetComponentData(Entity, new Scale
            {
                Value = Attack.Damage < 100 ? Attack.Damage > 25 ? Attack.Damage * .04f : .66f : 4f 
            });
            em.SetComponentData(Entity, new Translation
            {
                Value = new float3(attacker.Position.x, attacker.Position.y, 0)
            });
            em.SetComponentData(Entity, new Rotation
            {
                Value = Quaternion.Euler(0, 0,
                    Mathf.Atan2(Target.Position.y - Position.Value.y,
                        Target.Position.x - Position.Value.x) * Mathf.Rad2Deg)
            });
            em.SetSharedComponentData(Entity, new RenderMesh
            {
                mesh = meshTexture,
                material = mat
            });
        }

        //public void Move()
        //{
        //    Move(Vector2.MoveTowards(Position, Target.Position, Speed * Time.fixedDeltaTime));
        //    Rotation = new Vector3(0, 0, Mathf.Atan2(Target.Position.y - Position.y, Target.Position.x - Position.x) * Mathf.Rad2Deg);
        //    if (IsInRange(Target.Position, 0.01f))
        //        DisposeProjectile();
        //}
    }

    public struct ProjectileBody : IComponentData
    {
        public float3 Target { get; set; }
        public float Speed { get; set; }
        public int Id { get; set; }
        public bool InUse { get; set; }
    }
}