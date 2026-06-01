using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SoftDeletes.Api.Entites;

namespace SoftDeletes.Api.Entites
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default
        )
        {
            if(eventData.Context is  null) return ValueTask.FromResult(result) ;

            foreach ( var entry in eventData.Context.ChangeTracker.Entries<ISoftDeletable>())
            {
                   if (entry.State != EntityState.Deleted) continue;

            entry.State = EntityState.Modified;
            entry.Entity.IsDeleted = true;
            entry.Entity.DeletedAt = DateTime.UtcNow;
            }

        return ValueTask.FromResult(result);
        }
    }
}