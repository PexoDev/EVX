using System;
using System.Collections.Generic;
using Assets.Scripts.Attacks;
using Assets.Scripts.Units;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Scripts.ECS_TEst
{
    public class EntityTest : MonoBehaviour
    {
        [SerializeField] private Mesh _mesh;
        [SerializeField] private Material _material;

        private void Awake()
        {
            var em = World.Active.EntityManager;
            var archetype = em.CreateArchetype(typeof(Projectile), typeof(Translation), typeof(Rotation), typeof(RenderMesh), typeof(LocalToWorld));
            var entity = em.CreateEntity(archetype);

            Projectile p = new Projectile
            {
                Target = new float3(100, 100, 100),
                Speed = 1 * Time.deltaTime,
                Id = 1337
            };
            em.SetComponentData(entity, p);

            em.SetSharedComponentData(entity, new RenderMesh
            {
                mesh = _mesh,
                material = _material
            });

            ProjectileObject po = new ProjectileObject
            {
                ProjectileECSObject = p,
                HitAction = () => { Debug.Log("XD");}
            };

            ProcessProjectiles.HitActions.Add(po.ProjectileECSObject.Id, po.HitAction);
        }
    }

    public struct Projectile : IComponentData
    {
        public float3 Target { get; set; }
        public float Speed { get; set; }
        public int Id { get; set; }

        public int Damage { get; }
        public DamageType DamageType { get; }
        //public Action HitMethod { get; set; }
        //public LivingEntity Attacker { get; set; }
    }

    public class ProjectileObject
    {
        public Projectile ProjectileECSObject { get; set; }
        public Action HitAction { get; set; }
    }

    public class ProcessProjectiles : ComponentSystem
    {
        public static Dictionary<int,Action> HitActions = new Dictionary<int, Action>();

        protected override void OnUpdate()
        {
            Entities.ForEach((ref Projectile ent, ref Translation trans) =>
                {
                    trans.Value = math.lerp(trans.Value, ent.Target, ent.Speed);

                    if (!(math.distance(trans.Value, ent.Target) < 1f)) return;
                    if(!HitActions.ContainsKey(ent.Id)) return;

                    HitActions[ent.Id].Invoke();
                    HitActions.Remove(ent.Id);
                    ent.Id = -1;
                });
        }
    }
}