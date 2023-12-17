using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersApiCore.Common
{
    public class PagedList<T> : List<T>
    {

        private const int maxPageSize = 50;
        private int pageSize = 30;

        public int PageNumber {  get; set; } 
        public int PageSize
        {
            get { return pageSize; }
            set => pageSize = value > maxPageSize ? maxPageSize : value;
        }
        public int TotalCount {  get; set; }
        public int TotalPages { get; set; }

        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageSize < TotalCount;

        public PagedList(List<T>items , int pageSize , int pageNumber ,  int totalCount)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            AddRange(items);
        }

        public static async Task<PagedList<T>>
            CreateAsync(IQueryable<T> source , int pageSize , int pageNumber)
        {
            var count = await source.CountAsync();

            var items = await source.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            return new(items, pageSize, pageNumber, count);
        }
    }
}
