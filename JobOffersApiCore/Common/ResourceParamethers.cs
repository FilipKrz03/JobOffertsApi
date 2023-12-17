using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersApiCore.Common
{
    public class ResourceParamethers
    {
        public int PageSize { get; set; } = 30;
        public int PageNumber {  get; set; }  = 1;  

        public string? SearchQuery {  get; set; }

        public string? SortColumn { get; set; }
        public string SortOrder { get; set; } = "asc";
    }
}
