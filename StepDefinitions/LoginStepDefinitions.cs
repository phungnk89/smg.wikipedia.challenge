using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using SMG.Wikipedia.Challenge.Support;
using SMG.Wikipedia.Elements;
using System;
using System.Data;
using TechTalk.SpecFlow;

namespace SMG.Wikipedia.Challenge.StepDefinitions
{
    [Binding]
    public class LoginStepDefinitions
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private WebHelper helper;
        private DataHelper dataHelper;
        private DataTable userData;
        private IConfiguration configuration;
        private string tempPassword = string.Empty;

        public LoginStepDefinitions(ScenarioContext context)
        {
            driver = (EdgeDriver)context["driver"];
            wait = (WebDriverWait)context["wait"];
            configuration = (IConfiguration)context["configuration"];
            helper = (WebHelper)context["webhelper"];
            dataHelper = (DataHelper)context["datahelper"];
        }

        [When(@"I navigate back to Wikipedia Main Page")]
        [Given(@"I have accessed to Wikipedia Main Page")]
        public void GivenIHaveAccessedToWikipediaMainPage()
        {
            var url = configuration["appurl"];

            driver.Navigate().GoToUrl(url);
        }

        [Then(@"the Wikipedia Main Page should display")]
        public void ThenTheWikipediaMainPageShouldDisplay()
        {
            By element = By.XPath(MainPage.txtSearch);

            Assert.IsTrue(helper.IsElementPresent(element), "Main Page does not show!");
        }

        [When(@"I click on the Login link")]
        public void WhenIClickOnTheLoginLink()
        {
            By element = By.CssSelector(MainPage.linkLogin);

            driver.FindElement(element).Click();
        }

        [Then(@"the Login screen should display")]
        public void ThenTheLoginScreenShouldDisplay()
        {
            By txtUsername = By.CssSelector(LoginPage.txtUsername);
            By txtPassword = By.CssSelector(LoginPage.txtPassword);
            By btnLogin = By.CssSelector(LoginPage.btnLogin);

            Assert.IsTrue(helper.IsElementPresent(txtUsername), "Username field does not show!");
            Assert.IsTrue(helper.IsElementPresent(txtPassword), "Password field does not show!");
            Assert.IsTrue(helper.IsElementPresent(btnLogin), "Button login does not show!");
        }

        [When(@"I input valid account")]
        public void WhenIInputValidAccount()
        {
            userData = dataHelper.LoadCSV("user-data.csv");

            By txtUsername = By.CssSelector(LoginPage.txtUsername);
            By txtPassword = By.CssSelector(LoginPage.txtPassword);

            driver.FindElement(txtUsername).SendKeys(userData.Rows[0][0].ToString());
            driver.FindElement(txtPassword).SendKeys(userData.Rows[0][1].ToString());
        }

        [When(@"I click Login button")]
        public void WhenIClickLoginButton()
        {
            By element = By.CssSelector(LoginPage.btnLogin);

            driver.FindElement(element).Click();
        }

        [Then(@"the Main Page should display with my username on top")]
        public void ThenTheMainPageShouldDisplayWithMyUsernameOnTop()
        {
            By element = By.XPath(MainPage.labelUsername);

            Assert.That(helper.GetElementText(element), Contains.Substring(userData.Rows[0][0].ToString()), "Current user does not show on top!");
        }

        [When(@"I click Forgot Password link")]
        public void WhenIClickForgotPasswordLink()
        {
            By element = By.XPath(LoginPage.linkForforPassword);

            driver.FindElement(element).Click();
        }

        [Then(@"the Reset Password screen should display")]
        public void ThenTheResetPasswordScreenShouldDisplay()
        {
            By txtUsername = By.XPath(ForgotPasswordPage.txtUsername);
            By txtEmail = By.XPath(ForgotPasswordPage.txtEmail);

            Assert.IsTrue(helper.IsElementPresent(txtUsername), "Username field does not show!");
            Assert.IsTrue(helper.IsElementPresent(txtEmail), "Email field does not show!");
        }

        [When(@"I input valid username and email")]
        public void WhenIInputValidUsernameAndEmail()
        {
            By txtUsername = By.XPath(ForgotPasswordPage.txtUsername);
            By txtEmail = By.XPath(ForgotPasswordPage.txtEmail);

            driver.FindElement(txtUsername).SendKeys(userData.Rows[0][0].ToString());
            driver.FindElement(txtEmail).SendKeys(userData.Rows[0][2].ToString());
        }

        [When(@"I click Reset Password button")]
        public void WhenIClickResetPasswordButton()
        {
            By element = By.XPath(ForgotPasswordPage.btnResetPassword);

            driver.FindElement(element).Click();
        }

        [Then(@"the Reset Instruction text should display")]
        public void ThenTheResetInstructionTextShouldDisplay()
        {
            By element = By.XPath(ForgotPasswordPage.labelResetInstruction);

            Assert.IsTrue(helper.IsElementPresent(element), "Instruction text does not show!");
        }

        [When(@"I check my mailbox")]
        public void WhenICheckMyMailbox()
        {
            var url = "https://yopmail.com/en/";

            By txtEmail = By.CssSelector(YopMailPage.txtEmail);
            By btnRefresh = By.CssSelector(YopMailPage.btnRefresh);

            driver.Navigate().GoToUrl(url);
            driver.FindElement(txtEmail).SendKeys(userData.Rows[0][2].ToString());
            driver.FindElement(btnRefresh).Click();
        }

        [Then(@"I should receive Reset Password email")]
        public void ThenIShouldReceiveResetPasswordEmail()
        {
            var counter = 0;

            By frameInbox = By.CssSelector(YopMailPage.frameInbox);
            By divResetEmail = By.XPath(YopMailPage.divResetPasswordMail);

            while (counter < 5)
            {
                driver.SwitchTo().Frame(driver.FindElement(frameInbox));

                if (helper.IsElementPresent(divResetEmail)) break;

                driver.Navigate().Refresh();

                counter++;
            }

            Assert.IsTrue(helper.IsElementPresent(divResetEmail), "Does not receive email!");
        }

        [When(@"I get the temporary password from the email")]
        public void WhenIGetTheTemporaryPasswordFromTheEmail()
        {
            By divResetEmail = By.XPath(YopMailPage.divResetPasswordMail);
            By frameMail = By.CssSelector(YopMailPage.frameMail);

            driver.FindElement(divResetEmail).Click();
            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame(driver.FindElement(frameMail));

            tempPassword = helper.GetTemporaryPassword();

            driver.SwitchTo().DefaultContent();

            helper.ClearMailbox();
        }

        [When(@"I login with the temporary password")]
        public void WhenILoginWithTheTemporaryPassword()
        {
            By txtUsername = By.CssSelector(LoginPage.txtUsername);
            By txtPassword = By.CssSelector(LoginPage.txtPassword);
            By btnLogin = By.CssSelector(LoginPage.btnLogin);

            driver.FindElement(txtUsername).SendKeys(userData.Rows[0][0].ToString());
            driver.FindElement(txtPassword).SendKeys(tempPassword);
            driver.FindElement(btnLogin).Click();
        }

        [Then(@"the New Password screen should display")]
        public void ThenTheNewPasswordScreenShouldDisplay()
        {
            By txtNewPassword = By.CssSelector(LoginPage.txtNewPassword);
            By txtRetypePassword = By.CssSelector(LoginPage.txtRetypePassword);

            Assert.IsTrue(helper.IsElementPresent(txtNewPassword), "New password screen does not show!");
            Assert.IsTrue(helper.IsElementPresent(txtRetypePassword), "New password screen does not show!");
        }

        [When(@"I input my new password")]
        public void WhenIInputMyNewPassword()
        {
            By txtNewPassword = By.CssSelector(LoginPage.txtNewPassword);
            By txtRetypePassword = By.CssSelector(LoginPage.txtRetypePassword);

            driver.FindElement(txtNewPassword).SendKeys(userData.Rows[0][1].ToString());
            driver.FindElement(txtRetypePassword).SendKeys(userData.Rows[0][1].ToString());
        }

        [When(@"I click Continue Login button")]
        public void WhenIClickContinueLoginButton()
        {
            By element = By.CssSelector(LoginPage.btnLogin);

            driver.FindElement(element).Click();
        }

    }
}
