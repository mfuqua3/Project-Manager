using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Common.Contracts;
using ProjectManager.Common.Exceptions;

namespace ProjectManager.Common.Extensions;

/// <summary>
/// Provides several useful extension methods for IQueryable.
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Returns the only element of a sequence, or throws a specific exception if the 
    /// sequence is empty or contains more than one element.
    /// </summary>
    /// <returns>The single item in the query result.</returns>
    public static async Task<T> SingleOrNotFoundAsync<T>(this IQueryable<T> query)
    {
        try
        {
            return await query.SingleAsync();
        }
        catch (InvalidOperationException ex)
        {
            throw new ProjectManagerDataNotFoundException("The requested resource could not be found.", ex);
        }
    }

    /// <summary>
    /// Filters the query by a normalized name comparison ignoring the case.
    /// </summary>
    /// <returns>A query narrowed by provided name.</returns>
    public static IQueryable<T> ByNameIgnoreCase<T>(this IQueryable<T> query, string name)
        where T : INamed => query.Where(x => name != null && x.NameNormalized == name.ToUpper());

    /// <summary>
    /// Asserts a name is available in the query, if the name is not available then throws a BadRequestException.
    /// </summary>
    public static async Task AssertNameIsAvailableAsync<T>(this IQueryable<T> query, string name)
    where T: INamed
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));
        var nameIsAvailable = await query.ByNameIgnoreCase(name).AnyAsync();
        if (!nameIsAvailable)
        {
            throw new ProjectManagerBadRequestException($"Requested name {name} for {typeof(T).Name} is unavailable");
        }
    }

    /// <summary>
    /// Throws a specific exception if the query does not match the predicate.
    /// </summary>
    /// <returns>True, if the sequence contains any elements; otherwise, throws a not found exception.</returns>
    public static async Task<bool> ContainsOrNotFoundAsync<T>(this IQueryable<T> query, Expression<Func<T,bool>> predicate)
    {
        var exists =  await query.Where(predicate).AnyAsync();
        if (!exists)
        {
            throw new ProjectManagerDataNotFoundException("The requested resource could not be found.");
        }

        return true;
    }
}