using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Common.Contracts;
using ProjectManager.Common.Exceptions;

namespace ProjectManager.Common.Extensions;

/// <summary>
/// Static class that provides pagination extension methods.
/// </summary>
public static class PaginationExtensions
{
    /// <summary>
    /// Asynchronously converts a query to a paged list.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the query.</typeparam>
    /// <param name="query">The source query.</param>
    /// <param name="request">The pagination request parameters.</param>
    /// <param name="count">The total count of elements in the original query.</param>
    /// <returns>A task representing the asynchronous operation that returns a paged list of items.</returns>
    public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> query, IPaginated request, int count)
    {
        return new PagedList<T>()
        {
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = count,
            Items = await query.ToListAsync()
        };
    }

    /// <summary>
    /// Applies pagination to an ordered enumerable collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="query">The collection to paginat.</param>
    /// <param name="parameters">The pagination parameters.</param>
    /// <param name="count">Returns the total number of elements in the original collection.</param>
    /// <returns>A queryable collection with the pagination applied.</returns>
    public static IQueryable<T> Page<T>(this IOrderedEnumerable<T> query, IPaginated parameters, out int count)
    {
        return query.AsQueryable().Page(parameters, out count);
    }

    /// <summary>
    /// Applies pagination to an enumerable collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="query">The collection to paginate.</param>
    /// <param name="parameters">The pagination parameters.</param>
    /// <param name="count">Returns the total number of elements in the original collection.</param>
    /// <returns>A queryable collection with the pagination applied.</returns>
    public static IEnumerable<T> Page<T>(this IEnumerable<T> query, IPaginated parameters, out int count)
        => query.AsQueryable().Page(parameters, out count);

    /// <summary>
    /// Applies pagination to a queryable collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="query">The collection to paginate.</param>
    /// <param name="parameters">The pagination parameters.</param>
    /// <param name="count">Returns the total number of elements in the original collection.</param>
    /// <returns>A queryable collection with the pagination applied.</returns>
    public static IQueryable<T> Page<T>(this IQueryable<T> query, IPaginated parameters, out int count)
    {
        if (parameters.Page < 0)
        {
            throw new ProjectManagerBadRequestException("Invalid page. Please specify a page greater than or equal to zero.");
        }

        if (parameters.PageSize <= 0)
        {
            throw new ProjectManagerBadRequestException("Invalid page size. Please specify a page size greater than zero.");
        }

        var size = Math.Min(parameters.PageSize, int.MaxValue);
        count = query.Count();
        return query
            .Skip(Math.Max(parameters.Page, 0) * size)
            .Take(size);
    }
}