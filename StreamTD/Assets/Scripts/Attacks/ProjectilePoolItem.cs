using UnityEngine;

namespace Assets.Scripts.Attacks
{
    public class ProjectilePoolItem
    {
        public Projectile Projectile { get; set; }
        public GameObject Body { get; set; }
        public bool InUse { get; set; }

        public ProjectilePoolItem(GameObject projectilePrefab, Projectile projectile)
        {
            Projectile = projectile;
            Body = Object.Instantiate(projectilePrefab, ProjectilesController.Instance.ProjectilesCanvas.transform);
            Body.SetActive(false);
            Projectile.Body = Body;
        }
    }
}