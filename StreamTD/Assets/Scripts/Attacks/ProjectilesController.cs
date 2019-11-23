using System;
using System.Collections.Generic;
using Assets.Scripts.Units;
using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts.Attacks
{
    public class ProjectilesController
    {
        public static ProjectilesController Instance;
        private readonly EntityManager _em;
        private readonly Mesh _mesh;
        private readonly Material _plasma;
        private readonly Material _laser;
        private readonly Material _ballistic;
        public static Dictionary<int, Action> HitActions = new Dictionary<int, Action>();
        private static int _newestProjectileIndex = int.MinValue;

        public ProjectilesController(Material plasma, Material laser, Material ballistic, Mesh mesh)
        {
            _em = World.Active.EntityManager;
            _mesh = mesh;
            _plasma = plasma;
            _laser = laser;
            _ballistic = ballistic;

            if (Instance == null) Instance = this;
        }

        public Projectile InitializeProjectile(IAttack attackData, LivingEntity attacker, LivingEntity target, Action hitAction)
        {
            var material = attackData.PlasmaDamage > 0 ? _plasma :
                attackData.LaserDamage > 0 ? _laser : _ballistic;
            var newProjectile = new Projectile(_em, attackData, attacker, target, _newestProjectileIndex, _mesh, material);
            if (_newestProjectileIndex == int.MaxValue - 1) _newestProjectileIndex = int.MinValue;
            _newestProjectileIndex++;

            HitActions[newProjectile.Body.Id] = () =>
            {
                hitAction?.Invoke();
                _em.DestroyEntity(newProjectile.Entity);
                newProjectile = null;
            };

            return newProjectile;
        }
    }

}