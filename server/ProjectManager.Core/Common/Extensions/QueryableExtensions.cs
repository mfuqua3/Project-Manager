using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
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