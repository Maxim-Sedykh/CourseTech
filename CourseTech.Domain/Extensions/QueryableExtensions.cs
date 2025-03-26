using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace CourseTech.Domain.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TDestination> AsProjected<TSource, TDestination>(this IQueryable<TSource> source, IMapper mapper)
    {
        return source.ProjectTo<TDestination>(mapper.ConfigurationProvider);
    }
}
