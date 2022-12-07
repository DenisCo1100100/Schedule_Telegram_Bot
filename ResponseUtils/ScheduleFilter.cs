using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ScheduleTelegramBot.ResponseUtils
{
    public class ScheduleFilter
    {
        private const string XPATH_TO_TABLE = "//body/section/div[@class='container']/table/tbody";
        private IWebDriver _driver;
        private List<string> _allSemester = new List<string>();

        public ScheduleFilter(PSUWebSiteConnection connection)
        {
            string pathToFile = AppDomain.CurrentDomain.BaseDirectory + '\\';
            _driver = new ChromeDriver(pathToFile);
            _driver.Navigate().GoToUrl(connection.Url);
            Thread.Sleep(3000);
            IWebElement element = _driver.FindElement(By.XPath(XPATH_TO_TABLE));
            foreach (var row in element.FindElements(By.TagName("tr")))
            {
                _allSemester.Add(row.Text);
            }
        }
    }
}
