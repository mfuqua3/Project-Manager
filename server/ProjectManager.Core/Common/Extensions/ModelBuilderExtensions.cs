using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Common.Contracts;

namespace ProjectManager.Common.Extensions;

/// <summary>
/// ModelBuilder extension methods providing additional functionality for handling entity types and relationships.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Apply the soft delete query filters to each entity type in the model that implements the ISoftDelete interface.
    /// </summary>
    /// <param name="builder">The instance of the ModelBuilder to apply the soft delete filters to.</param>
    public static void ApplySoftDeleteQueryFilters(this ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                entityType.AddSoftDeleteQueryFilter();
            }

        }
    }
        
    /// <summary>
    /// Restrict the delete behavior of all foreign key relationships in the model to prevent cascading deletes.
    /// </summary>
    /// <param name="builder">The instance of the ModelBuilder to restrict delete behavior on.</param>
    public static void RestrictForeignKeyDelete(this ModelBuilder builder)
    {
        var foreignKeyRelationships = builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());
        foreach (var fkRelationship in foreignKeyRelationships)
        {
            fkRelationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}