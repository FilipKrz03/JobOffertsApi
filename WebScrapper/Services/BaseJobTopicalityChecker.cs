using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapperService.Interfaces;

namespace WebScrapperService.Services
{
    public abstract class BaseJobTopicalityChecker
    {

        private readonly IWebDriverFactory _webDriverFactory;


        public BaseJobTopicalityChecker(
            IWebDriverFactory webDriverFactory 
            )
        {
            _webDriverFactory = webDriverFactory;
        }
    }
}
