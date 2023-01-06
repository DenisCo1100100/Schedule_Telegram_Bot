using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ScheduleTelegramBot.Extensions;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ScheduleTelegramBot.ResponseUtils
{
    public class ScheduleFilter
    {
        private const string XPATH_TO_TABLE = "//body/section/div[@class='container']/table/tbody";
        private const string XPATH_TO_WEEKS_DROPDOWN_BUTTON = "//body/section/div/div/div[@class='col-xs-3']/div/button";
        private const string XPATH_TO_WEEKS_DROPDOWN_CONTENT = "//body/section/div/div/div[@class='col-xs-3']/div/ul";
        private const string EXPRESSION_WEEK_NUMBERS = @"\s(?<week>\d+)\s\(((?<firstTime>\d{2}.\d{2})[.]?)-((?<secondTime>\d{2}.\d{2})[.]?)\)";

        private Dictionary<List<DateTime>, int> _weeksNumbers = new();

        IWebDriver _driver;

        public ScheduleFilter(PSUWebSiteConnection connection)
        {
            string pathToFile = AppDomain.CurrentDomain.BaseDirectory + '\\';
            _driver = new ChromeDriver(pathToFile);
            _driver.Navigate().GoToUrl(connection.Url);
            Thread.Sleep(1000);

            FillWeeksDictinary();
        }

        public List<string> GetThisWeekSchudle() => GetSchudleByWeekNumber(GetCurrentWeekNumber());

        public List<string> GetNextWeekSchudle() => GetSchudleByWeekNumber(GetCurrentWeekNumber() + 1);

        public List<string> GetTodaySchedule() => GetScheduleByDayOfWeek(DateTime.Now);

        public List<string> GetTomorrowSchedule() => GetScheduleByDayOfWeek(DateTime.Now.AddDays(1));

        private List<string> GetScheduleByDayOfWeek(DateTime dayOfWeek)
        {
            string curentDayOfWeek = dayOfWeek.ToString("dddd", new CultureInfo("ru-RU"));
            string nextDayOfWeek = dayOfWeek.AddDays(1).ToString("dddd", new CultureInfo("ru-RU"));
            List<string> schedule = GetSchudleByWeekNumber(GetCurrentWeekNumber());
            List<string> todaySchedule = new();
            bool isToday = false;
            foreach (string day in schedule)
            {
                if (day.Split(" ")[0].ToLower().Equals(curentDayOfWeek.ToLower()))
                    isToday = true;
                else if (day.Split(" ")[0].ToLower().Equals(nextDayOfWeek.ToLower()))
                    return todaySchedule;

                if (isToday)
                    todaySchedule.Add(day);
            }
            return todaySchedule;
        }

        private void FillWeeksDictinary()
        {
            IWebElement dropDownButton = _driver.FindElement(By.XPath(XPATH_TO_WEEKS_DROPDOWN_BUTTON));
            dropDownButton.Click();

            IWebElement element = _driver.FindElement(By.XPath(XPATH_TO_WEEKS_DROPDOWN_CONTENT));
            foreach (var item in element.FindElements(By.TagName("li")))
            {
                Match match = Regex.Match(item.Text, EXPRESSION_WEEK_NUMBERS);
                if (match.Success)
                {
                    List<DateTime> weekDates = new();
                    int weekNumber = Convert.ToInt32(match.Groups["week"].Value);
                    string firstDateStr = match.Groups["firstTime"].Value;
                    DateTime firstDate = DateTime.Parse(firstDateStr);
                    weekDates.Add(firstDate);
                    string secondDateStr = match.Groups["secondTime"].Value;
                    DateTime secondDate = DateTime.Parse(secondDateStr);
                    weekDates.Add(secondDate);
                    _weeksNumbers.Add(weekDates, weekNumber);
                }
            }
            dropDownButton.Click();
        }

        private List<string> GetSchudleByWeekNumber(int weekNumber)
        {
            List<string> schudle = new();
            _driver.FindElement(By.XPath(XPATH_TO_WEEKS_DROPDOWN_BUTTON)).Click();
            _driver.FindElement(By.XPath($"//body/section/div/div/div[@class='col-xs-3']/div/ul/li/a[@href=\"#w{weekNumber - 1}\"]")).Click();

            IWebElement element = _driver.FindElement(By.XPath(XPATH_TO_TABLE));
            foreach (var row in element.FindElements(By.TagName("tr")))
            {
                if (row.Text != String.Empty)
                    schudle.Add(row.Text);
            }

            return schudle;
        }

        private int GetCurrentWeekNumber()
        {
            DateTime currentDate = DateTime.Now.Date;
            foreach (var week in _weeksNumbers)
            {
                if (currentDate.IsInRange(week.Key[0], week.Key[1]))
                    return week.Value;
            }

            return -1;
        }
    }
}
