using FileUploadService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUploadService.DAL.Interceptors
{
    public class DateInterceptor: SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var dbContext = eventData.Context;
            if(dbContext == null)
            {
                return base.SavingChangesAsync(eventData, result, cancellationToken);
            }

            var entries = dbContext.ChangeTracker.Entries<IAuditable>()
                .Where(x=>x.State == EntityState.Added)
                .ToList();

            foreach( var entry in entries )
            {
                entry.Property(x=>x.CreatedAt).CurrentValue = DateTime.UtcNow;
            }
            return base.SavingChangesAsync(eventData, result,cancellationToken);
        }
    }
}
