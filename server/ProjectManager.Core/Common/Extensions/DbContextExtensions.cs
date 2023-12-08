using System;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Common.Contracts;

namespace ProjectManager.Common.Extensions;

/// <summary>
/// DbContextExtensions static class provides extension methods for DbContext.
/// </summary>
public static class DbContextExtensions
{
    /// <summary>
    /// An extension method to process entities implementing various interfaces.
    /// For entities with <see cref="ISoftDelete"/> interface, if they are newly added it sets IsDeleted to false,
    /// else if they are supposed to be deleted it sets IsDeleted to true and provides the deletion time.
    /// For entities with <see cref="ICreated"/> interface and in an Added state, it sets the Created time to the current time.
    /// For entities with <see cref="IUpdated"/> interface and in a Modified state, it sets the Updated time to the current time.
    /// For entities with <see cref="INamed"/> interface and in an Added or Modified state, it sets the NameNormalized to uppercase version of Name.
    /// </summary>
    /// <param name="dbContext">The DbContext instance on which the extension method operates.</param>
    public static void ProcessCustomerInterfaces(this DbContext dbContext)
    {
        var autoDetectChangesEnabled = dbContext.ChangeTracker.AutoDetectChangesEnabled;
        try
        {
            dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            foreach (var entry in dbContext.ChangeTracker.Entries())
            {
                #region Configure ISoftDelete Fields
                if (entry.Entity is ISoftDelete deleted)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            deleted.IsDeleted = false;
                            break;
                        case EntityState.Deleted:
                            entry.State = EntityState.Unchanged;
                            deleted.IsDeleted = true;
                            deleted.Deleted = DateTime.UtcNow;
                            break;
                    }
                }
                #endregion

                #region Configure ITracked Fields
                if (entry.Entity is ICreated created && entry.State == EntityState.Added)
                {
                    created.Created = DateTime.UtcNow;
                }
                if (entry.Entity is IUpdated updated && entry.State == EntityState.Modified)
                {
                    updated.Updated = DateTime.UtcNow;
                }
                #endregion

                #region Configure INamed Fields

                if (entry.Entity is INamed named && entry.State is EntityState.Added or EntityState.Modified)
                {
                    named.NameNormalized = named.Name?.ToUpper();
                }

                #endregion
            }
        }
        finally
        {
            dbContext.ChangeTracker.AutoDetectChangesEnabled = autoDetectChangesEnabled;
        }
    } 
}