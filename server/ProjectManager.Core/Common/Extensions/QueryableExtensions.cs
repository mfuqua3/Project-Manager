using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Common.Contracts;
using ProjectManager.Common.Exceptions;

namespace ProjectManager.Common.Extensions;

public static class QueryableExtensions
{
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

    public static IQueryable<T> ByNameIgnoreCase<T>(this IQueryable<T> query, string name)
        where T : INamed => query.Where(x => name != null && x.NameNormalized == name.ToUpper());

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