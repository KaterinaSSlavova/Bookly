using Microsoft.EntityFrameworkCore;

namespace EFDataLayer.DBContext
{
    public static class DbContextExtension
    {
        public static void DetachIfTracked<TEntity, TKey>(this BooklyDbContext context, TEntity entity, Func<TEntity, TKey> keySelector) where TEntity : class 
        {
            var entityKey = keySelector(entity);

            var trackedEntity = context.ChangeTracker.Entries<TEntity>()
                .FirstOrDefault(e => keySelector(e.Entity).Equals(entityKey));

            if (trackedEntity != null)
            {
                trackedEntity.State = EntityState.Detached;
            }
        }
    }
}
