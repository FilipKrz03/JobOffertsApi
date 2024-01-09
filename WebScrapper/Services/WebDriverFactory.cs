using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapperService.Interfaces;

namespace WebScrapperService.Services
{
    internal class WebDriverFactory : IWebDriverFactory
    {
        public IWebDriver GetWebDriver()
        {
            string isSeleniumOnDocker = Environment.GetEnvironmentVariable("IsSeleniumOnDocker")!;

            if (isSeleniumOnDocker == "true")
            {
                string remoteDriverUri = Environment.GetEnvironmentVariable("RemoteDriverUri")!;

                var options = new ChromeOptions();
                options.AddArgument("--ignore-ssl-errors=yes");
                options.AddArgument("-ignore-certificate-errors");

                return new RemoteWebDriver(new Uri(remoteDriverUri), options);
            }
            else
            {
                return new ChromeDriver();
            }
        }
    }
}
