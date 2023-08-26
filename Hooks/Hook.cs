using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using SMG.Wikipedia.Challenge.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace SMG.Wikipedia.Challenge.Hooks
{
    [Binding]
    public sealed class Hook
    {
        private readonly ISpecFlowOutputHelper helper;
        private IWebDriver _driver;
        private IConfiguration _configuration;
        private WebDriverWait _wait;
        private WebHelper _webHelper;
        private DataHelper _dataHelper;
        private string imagePath = string.Empty;

        public Hook(ISpecFlowOutputHelper helper)
        {
            this.helper = helper;
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext context)
        {
            new DriverManager().SetUpDriver(new EdgeConfig());

            EdgeOptions options = new EdgeOptions { PageLoadStrategy = PageLoadStrategy.Normal };

            options.AddArguments("--window-size=1920,1080");
            options.AddArguments("--no-sandbox");
            options.AddArguments("--disable-web-security");
            options.AddArguments("--disable-gpu");
            options.AddArguments("--inprivate");
            options.AddArguments("--disable-blink-features=AutomationControlled");
            options.AddArguments("disable-infobars");
            options.AddArguments("start-maximized");

            if (bool.Parse(_configuration["headless"]))
            {
                options.AddArguments("--headless");
            }

            _driver = new EdgeDriver(EdgeDriverService.CreateDefaultService(), options, TimeSpan.FromSeconds(30));

            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            _webHelper = new WebHelper() { driver = _driver };

            _dataHelper = new DataHelper();

            context["driver"] = _driver;
            context["wait"] = _wait;
            context["configuration"] = _configuration;
            context["webhelper"] = _webHelper;
            context["datahelper"] = _dataHelper;
        }

        [AfterScenario]
        public void AfterScenario(ScenarioContext context)
        {
            imagePath = string.Empty;
            _driver.Quit();
            context.Remove("driver");
            context.Remove("wait");
            context.Remove("configuration");
            context.Remove("webhelper");
            context.Remove("datahelper");
        }

        [BeforeStep]
        public void BeforeStep()
        {
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [AfterStep]
        public void AfterStep(ScenarioContext context)
        {
            _webHelper = new WebHelper() { driver = _driver };

            if (context.TestError != null)
            {
                imagePath = _webHelper.CaptureScreenshotAndSave(context);

                if (!string.IsNullOrEmpty(imagePath))
                {
                    helper.AddAttachment(imagePath);
                }
            }
        }
    }
}