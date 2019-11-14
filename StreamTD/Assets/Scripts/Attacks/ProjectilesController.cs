using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Attacks
{
    public class ProjectilesController
    {
        public static ProjectilesController Instance;

        public Canvas ProjectilesCanvas;
        private readonly GameObject _projectilePrefab;
        private readonly List<ProjectilePoolItem> _pool = new List<ProjectilePoolItem>();

        public ProjectilesController(GameObject projectilePrefab, Canvas projectilesCanvas)
        {
            ProjectilesCanvas = projectilesCanvas;
            _projectilePrefab = projectilePrefab;

            if(Instance == null) Instance = this;
        }

        public void ProcessProjectiles()
        {
            foreach (var poolItem in _pool.Where(proj => proj.InUse))
                poolItem.Projectile.Act();
        }

        public void InitializeProjectile(int damage, DamageType damageType, Vector2 position, LivingEntity target, Action hitAction)
        {
            var item = _pool.FirstOrDefault(p => !p.InUse);

            if (item == null)
            {
                var wrappedProjectile = GetWrappedProjectile(damage, damageType, position, target, hitAction, _pool.Count);
                _pool.Add(new ProjectilePoolItem(_projectilePrefab, wrappedProjectile ));
                item = _pool.Last();
            }
            else
            {
                item.Body.SetActive(true);
                item.Projectile = GetWrappedProjectile(damage, damageType, position, target, hitAction, _pool.IndexOf(item));
                item.Projectile.Body = item.Body;
            }

            item.Projectile.Body.GetComponent<SpriteRenderer>().color = (damageType == DamageType.Ballistic) ? Color.black :
                (damageType == DamageType.Laser) ? Color.red : Color.green;
            item.InUse = true;
        }

        private Projectile GetWrappedProjectile(int damage, DamageType damageType, Vector2 position, LivingEntity target, Action hitAction, int index)
        {
            return new Projectile(damage, damageType, position, target, () =>
            {
                hitAction();
                ReturnToPool(index);
            });
        }

        public void ReturnToPool(int index)
        {
            var proj = _pool[index];
            proj.Body.SetActive(false);
            proj.Projectile = null;
            proj.InUse = false;
        }
    }
}