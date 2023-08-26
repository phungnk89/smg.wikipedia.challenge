using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using SMG.Wikipedia.Challenge.Support;
using SMG.Wikipedia.Elements;
using TechTalk.SpecFlow;

namespace SMG.Wikipedia.Challenge.StepDefinitions
{
    [Binding]
    public class SearchStepDefinitions
    {
        private IWebDriver driver;
        private IConfiguration configuration;
        private WebDriverWait wait;
        private WebHelper helper;
        private string keyword = string.Empty;

        public SearchStepDefinitions(ScenarioContext context)
        {
            driver = (EdgeDriver)context["driver"];
            wait = (WebDriverWait)context["wait"];
            configuration = (IConfiguration)context["configuration"];
            helper = (WebHelper)context["webhelper"];
        }

        [When(@"I input keywords '([^']*)' in Search field")]
        public void WhenIInputKeywordsInSearchField(string input)
        {
            By element = By.XPath(MainPage.txtSearch);

            driver.FindElement(element).SendKeys(input);

            keyword = input;
        }

        [Then(@"it should pop out suggestion that match the keywords")]
        public void ThenItShouldPopOutSuggestionThatMatchTheKeywords()
        {
            By element = By.XPath(MainPage.liSuggestion);

            wait.Until(drv => drv.FindElement(element));

            Assert.That(helper.GetElementText(element), Contains.Substring(keyword), $"Suggestion does not contain {keyword}");
        }

        [Then(@"I click Search button")]
        public void ThenIClickSearchButton()
        {
            By element = By.XPath(MainPage.btnSearch);

            driver.FindElement(element).Click();
        }

        [Then(@"the relevant result page should display")]
        public void ThenTheRelevantResultPageShouldDisplay()
        {
            By element = By.XPath(MainPage.headerTitle);

            Assert.That(helper.GetElementText(element).ToLower(), Contains.Substring(keyword.ToLower()), $"Page result is not relevant with keyword {keyword}");
        }
    }
}
