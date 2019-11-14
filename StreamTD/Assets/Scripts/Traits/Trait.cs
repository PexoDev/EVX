using Assets.Scripts.Units;

namespace Assets.Scripts.Traits
{
    public class Trait
    {
        private readonly UnitParameters _up;

        public Trait(UnitParameters up)
        {
            _up = up;
        }

        public void ApplyParameters<TEntity, TTarget>(AttackingEntity<TEntity, TTarget> entity)
            where TEntity : LivingEntity
            where TTarget : LivingEntity
        {
            ApplyParameters(entity, _up);
        }

        public static void ApplyParameters<TEntity, TTarget>(AttackingEntity<TEntity, TTarget> entity,
            UnitParameters up)
            where TEntity : LivingEntity
            where TTarget : LivingEntity
        {
            var paramsToApply = UnitParameters.SetNewestOrDefault(entity.UnitParams, up);
            entity.UnitParams = paramsToApply;
        }
    }
}