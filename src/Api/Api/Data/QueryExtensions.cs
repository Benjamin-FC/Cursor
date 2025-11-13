using Api.Domain;
using System.Linq.Expressions;

namespace Api.Data;

public static class QueryExtensions
{
    public static IQueryable<Contact> ApplySorting(this IQueryable<Contact> query, string sort, string dir)
    {
        return sort.ToLower() switch
        {
            "firstname" => dir == "desc" 
                ? query.OrderByDescending(c => c.FirstName) 
                : query.OrderBy(c => c.FirstName),
            "lastname" => dir == "desc" 
                ? query.OrderByDescending(c => c.LastName) 
                : query.OrderBy(c => c.LastName),
            "email" => dir == "desc" 
                ? query.OrderByDescending(c => c.Email) 
                : query.OrderBy(c => c.Email),
            "company" => dir == "desc" 
                ? query.OrderByDescending(c => c.Company ?? string.Empty) 
                : query.OrderBy(c => c.Company ?? string.Empty),
            "createdat" => dir == "desc" 
                ? query.OrderByDescending(c => c.CreatedAt) 
                : query.OrderBy(c => c.CreatedAt),
            _ => query.OrderBy(c => c.LastName).ThenBy(c => c.FirstName)
        };
    }
}

