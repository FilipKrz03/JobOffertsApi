using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersApiCore.Common
{
    public class PagedListConverter<TSource, TDestination> :
        ITypeConverter<PagedList<TSource>, PagedList<TDestination>>
        where TSource : class
        where TDestination : class
    {
        public PagedList<TDestination> Convert
            (PagedList<TSource> source, PagedList<TDestination> destination, ResolutionContext context)
        {
            var collection =
                context.Mapper.Map<List<TSource>, List<TDestination>>(source);

            return new
                PagedList<TDestination>(collection, source.PageNumber, source.PageSize, source.TotalCount);
        }
    }
}
