using System;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;
using ProjectManager.Common.Contracts;

namespace ProjectManager.Common.Extensions;

/// <summary>
/// A static class containing extension methods for EF Core's IMutableEntityType interface.
/// </summary>
public static class MutableEntityTypeExtensions
{
    /// <summary>
    /// Adds a global query filter for soft delete to an entity type.
    /// </summary>
    /// <param name="entityType">The entity type to which the filter will be added.</param>
    public static void AddSoftDeleteQueryFilter(this IMutableEntityType entityType)
    {
        var methodToCall = typeof(MutableEntityTypeExtensions)
            .GetMethod(nameof(GetSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)
            ?.MakeGenericMethod(entityType.ClrType);

        var filter = methodToCall?.Invoke(null, new object[] { });
        entityType.SetQueryFilter(filter as LambdaExpression);
        entityType.AddIndex(entityType.FindProperty(nameof(ISoftDelete.IsDeleted)) ?? 
                            throw new MissingMemberException("Unexpected error. Property not found."));
    }

    /// <summary>
    /// Constructs a soft delete filter expression for a specific entity type.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <returns>The lambda expression representing the soft delete filter.</returns>
    internal static LambdaExpression GetSoftDeleteFilter<TEntity>() where TEntity : class, ISoftDelete
    {
        Expression<Func<TEntity, bool>> filter = x => !x.IsDeleted;
        return filter;
    }
}