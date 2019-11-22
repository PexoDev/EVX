using System.Collections.Concurrent;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public static class CooldownController
    {
        private static readonly ConcurrentDictionary<string, float> Cooldowns = new ConcurrentDictionary<string, float>();

        public static bool GetCooldown(string id, float cooldownDuration)
        {
            if (!Cooldowns.ContainsKey(id) || Cooldowns[id] <= 0)
            {
                Cooldowns[id] = cooldownDuration;
                return true;
            }

            return false;
        }

        public static void UpdateCooldowns(int speedMultiplier)
        {
            foreach (string key in Cooldowns.Keys)
                if(Cooldowns[key] > 0)
                    Cooldowns[key] -= Time.deltaTime * speedMultiplier;
        }
    }
}