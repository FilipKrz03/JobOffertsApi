using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersApiCore.Common
{
    public class ResourceParamethers
    {
        private const int maxPageSize = 50;
        private int pageSize = 30;

        public int PageSize
        {
            get { return pageSize; }
            set => pageSize = value > maxPageSize ? maxPageSize : value;
        }

        public int PageNumber {  get; set; }  = 1;  

        public string? SearchQuery {  get; set; }

        public string? SortColumn { get; set; }
        public string SortOrder { get; set; } = "asc";
    }
}
