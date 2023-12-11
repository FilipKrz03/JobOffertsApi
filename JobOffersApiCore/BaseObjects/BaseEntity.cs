using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersApiCore.BaseObjects
{
    public class BaseEntity
    {
        [Key]
        public Guid Id {  get; set; }   
    }
}
