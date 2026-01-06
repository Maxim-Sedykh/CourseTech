using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace CourseTech.Domain.Extensions;

/// <summary>
/// Расширения для <see cref="IQueryable"/>
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Сделать метод Automapper.ProjectTo, проекция на уровне бдшки
    /// Удобная обёртка чтобы каждый раз не писать mapper.ConfigurationProvider
    /// </summary>
    /// <typeparam name="TSource">Тип источника</typeparam>
    /// <typeparam name="TDestination">Тип в какой хотим смаппить</typeparam>
    /// <param name="source">Источник</param>
    /// <param name="mapper">Объект маппера</param>
    /// <returns>То что хотим смаппить</returns>
    public static IQueryable<TDestination> AsProjected<TSource, TDestination>(this IQueryable<TSource> source, IMapper mapper)
    {
        return source.ProjectTo<TDestination>(mapper.ConfigurationProvider);
    }
}
