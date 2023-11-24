using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Common.Contracts;
using ProjectManager.Common.Exceptions;

namespace ProjectManager.Common.Extensions;

public static class PaginationExtensions
{
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
    
    public static IQueryable<T> Page<T>(this IOrderedEnumerable<T> query, IPaginated parameters, out int count)
    {
        return query.AsQueryable().Page(parameters, out count);
    }

    public static IEnumerable<T> Page<T>(this IEnumerable<T> query, IPaginated parameters, out int count)
        => query.AsQueryable().Page(parameters, out count);
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