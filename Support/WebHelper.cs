using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using SMG.Wikipedia.Elements;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;

namespace SMG.Wikipedia.Challenge.Support
{
    public class WebHelper
    {
        public IWebDriver driver { get; set; }

        /// <summary>
        /// IsElementPresent
        /// </summary>
        /// <param name="by"></param>
        /// <returns>true or false</returns>
        public bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ClickByJavascript
        /// </summary>
        /// <param name="by"></param>
        public void ClickByJavascript(By by)
        {
            var element = driver.FindElement(by);

            IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;

            executor.ExecuteScript("arguments[0].click();", element);
        }

        /// <summary>
        /// CaptureScreenshotAndSave
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="Exception"></exception>
        public string CaptureScreenshotAndSave(ScenarioContext context)
        {
            var baseDirectoryPath = AppContext.BaseDirectory;
            var path = $"{baseDirectoryPath}{context.ScenarioInfo.Title}.png";

            try
            {
                driver.TakeScreenshot().SaveAsFile(path, ScreenshotImageFormat.Png);

                Console.WriteLine("Capture and save screenshot successfully!");

                return path;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// GetElementText
        /// </summary>
        /// <param name="by"></param>
        /// <returns>element text</returns>
        public string GetElementText(By by)
        {
            var element = driver.FindElement(by);

            if (!string.IsNullOrEmpty(element.Text)) return element.Text;

            if (!string.IsNullOrEmpty(element.GetAttribute("value"))) return element.GetAttribute("value");

            return string.Empty;
        }

        /// <summary>
        /// ClearMailbox
        /// </summary>
        public void ClearMailbox()
        {
            By btnDeleteAll = By.CssSelector(YopMailPage.btnDeleteAll);
            By messageDelete = By.XPath(YopMailPage.messageDeleted);

            driver.SwitchTo().DefaultContent();
            ClickByJavascript(btnDeleteAll);
            var alert = driver.SwitchTo().Alert();
            alert.Accept();

            driver.SwitchTo().DefaultContent();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(drv => drv.FindElement(messageDelete));
        }

        /// <summary>
        /// GetTemporaryPassword
        /// </summary>
        /// <returns>temporary password</returns>
        public string GetTemporaryPassword()
        {
            By txtMailContent = By.XPath(YopMailPage.txtMailContent);

            var content = GetElementText(txtMailContent);
            var pattern = @"Temporary password:\s*(\w+)";

            Match match = Regex.Match(content, pattern);

            return match.Success ? match.Groups[1].Value : string.Empty;
        }
    }
}
