using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapperService.Services;

namespace WebScrapperTests.Services
{
    public class WebDriverFactoryTests
    {

        [Fact]
        public void Factory_Should_ReturnChromeDriver_WhenSeleniumIsNotOnDocker()
        {
            Environment.SetEnvironmentVariable("IsSeleniumOnDocker" , "false");

            var factory = new WebDriverFactory();

            var driver = factory.GetWebDriver();

            Assert.IsType<ChromeDriver>(driver);
        }
    }
}
