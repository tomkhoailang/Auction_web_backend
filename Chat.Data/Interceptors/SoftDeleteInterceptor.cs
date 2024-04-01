using Chat.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Chat.Data.Interceptors
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context is null) return base.SavingChangesAsync(eventData, result, cancellationToken); ;
            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry is { State: EntityState.Deleted, Entity: ISoftDelete delete })
                {
                    entry.State = EntityState.Modified;
                    delete.IsDeleted = true;
                    delete.DeletedAt = DateTimeOffset.UtcNow;
                }
            }
            return base.SavingChangesAsync(eventData, result, cancellationToken); ;
        }
    }
}
