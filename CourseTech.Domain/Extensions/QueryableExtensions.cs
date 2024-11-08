using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<TDestination> AsProjected<TSource, TDestination>(this IQueryable<TSource> source, IMapper mapper)
        {
            return source.ProjectTo<TDestination>(mapper.ConfigurationProvider);
        }
    }
}
