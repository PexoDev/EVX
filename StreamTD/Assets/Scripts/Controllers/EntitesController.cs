using System.Collections.Generic;
using Assets.Scripts.Units;

namespace Assets.Scripts.Controllers
{
    public abstract class EntitiesController<T> where T : LivingEntity
    {
        public readonly GameController Gc;
        
        public List<T> Entities = new List<T>();
        public abstract void ProcessActions();

        protected void ProcessLivingEntities()
        {
            foreach (T entity in Entities)
                entity.Regenerate();
        }

        protected EntitiesController(GameController gc)
        {
            Gc = gc;
        }

        public virtual void RemoveInstance(T entity)
        {
            if(Entities.Contains(entity))
                Entities.Remove(entity);
        }
    }
}