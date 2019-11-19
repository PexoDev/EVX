using Assets.Scripts.Attacks;
using Assets.Scripts.Controllers;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
// ReSharper disable UnusedMember.Global

namespace Assets.Scripts
{
    public class ProcessProjectiles : ComponentSystem
    {
        protected override void OnUpdate()
        {
            if (GameController.Mode != GameMode.Play) return;

            Entities.ForEach((ref ProjectileBody ent, ref Translation trans) =>
            {
                trans.Value = math.lerp(trans.Value, ent.Target, ent.Speed);

                if (!(math.distance(trans.Value, ent.Target) < 0.1f)) return;
                if (!ProjectilesController.HitActions.ContainsKey(ent.Id)) return;

                ProjectilesController.HitActions[ent.Id].Invoke();
                ProjectilesController.HitActions.Remove(ent.Id);
            });
        }
    }
}