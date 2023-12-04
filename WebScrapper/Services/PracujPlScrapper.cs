using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapperService.Interfaces;

namespace WebScrapperService.Services
{
    public class PracujPlScrapper : BaseJobScrapper, IScrapperService
    {
        public PracujPlScrapper()
             : base("https://it.pracuj.pl/praca?pn=",
                  ".c1fljezf", "h1", "h2", ".core_n194fgoq"){ }
    }
}
