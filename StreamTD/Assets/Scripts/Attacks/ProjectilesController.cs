using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Attacks
{
    public class ProjectilesController
    {
        public static ProjectilesController Instance;
        private readonly ObjectBodyPool<Projectile, DamageType> _pool;
        public Transform ParentTransform;

        public ProjectilesController(GameObject projectilePrefab, Transform projectilesParent, Sprite plasma, Sprite laser,
            Sprite ballistic)
        {
            ParentTransform = projectilesParent;

            _pool = new ObjectBodyPool<Projectile, DamageType>(projectilePrefab, projectilesParent,
                new Dictionary<DamageType, Sprite>
                {
                    {DamageType.Plasma, plasma},
                    {DamageType.Laser, laser},
                    {DamageType.Ballistic, ballistic}
                });

            if (Instance == null) Instance = this;
        }

        public void ProcessProjectiles()
        {
            foreach (var poolItem in _pool.Pool.Where(proj => proj.InUse).ToArray())
                poolItem.Object.Move();

            _pool.UpdateAllBodies();
        }

        public ObjectBodyPoolItem<Projectile> InitializeProjectile(Projectile projectile)
        {
            var cacheAction = projectile.HitMethod;
            projectile.HitMethod = () =>
            {
                cacheAction?.Invoke();
                _pool.ReturnToPool(projectile);
            };

            var newObj = _pool.GetObject(projectile, null, projectile.DamageType);
            newObj.Body.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(projectile.Target.Position.y - projectile.Position.y, projectile.Target.Position.x - projectile.Position.x) * Mathf.Rad2Deg);
            newObj.Body.transform.localScale = Vector3.one * ((projectile.Damage < 100) ? (projectile.Damage > 25) ? projectile.Damage * .04f : .66f : 4f);
            return newObj;
        }
    }
}