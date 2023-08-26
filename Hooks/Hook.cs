using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;
using System.IO;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using OpenQA.Selenium.Edge;
using System.Security.Principal;
using OpenQA.Selenium.Support.UI;
using SMG.Wikipedia.Challenge.Support;

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
                string userName = WindowsIdentity.GetCurrent().Name.Split('\\')[1];
                string dir = string.Format(@"c:\Users\{0}\Downloads", userName);

                options.AddArguments("--headless");
                options.AddUserProfilePreference("download.default_directory", dir);
                options.AddUserProfilePreference("download.prompt_for_download", true);
                options.AddUserProfilePreference("download.directory_upgrade", true);
                options.AddUserProfilePreference("download.prompt_for_download", false);
                options.AddUserProfilePreference("safebrowsing.enabled", false);
                options.AddUserProfilePreference("safebrowsing.disable_download_protection", true);
                options.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", true);
            }

            _driver = new EdgeDriver(EdgeDriverService.CreateDefaultService(), options, TimeSpan.FromSeconds(30));

            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            _webHelper = new WebHelper() { driver = _driver };

            context["driver"] = _driver;
            context["wait"] = _wait;
            context["configuration"] = _configuration;
            context["webhelper"] = _webHelper;
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