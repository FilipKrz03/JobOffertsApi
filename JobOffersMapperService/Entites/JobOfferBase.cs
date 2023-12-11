using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersMapperService.Entites
{
    public class JobOfferBase
    {
        [Key]
        public Guid Id {  get; set; }

        [Required]
        public string OfferTitle {  get; set; }

        [Required]
        public string OfferCompany {  get; set; }

        [Required]
        public string OfferLink {  get; set; }

        public JobOfferBase(string offerTitle , string offerCompany , string offerLink)
        {
            OfferTitle = offerTitle;
            OfferCompany = offerCompany;
            OfferLink = offerLink;
        }
    }
}
