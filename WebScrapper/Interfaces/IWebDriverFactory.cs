using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapperService.Interfaces
{
    internal interface IWebDriverFactory
    {
        public IWebDriver GetWebDriver();
    }
}
