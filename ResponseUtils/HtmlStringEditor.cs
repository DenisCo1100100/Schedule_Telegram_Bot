using GrapeCity.Documents.Html;
using System.Drawing;
using System.Text.RegularExpressions;

namespace ScheduleTelegramBot.ResponseUtils
{
    public class HtmlStringEditor
    {
        private const string EXPRESSION_ROW_DATA = @"(?<time>\b[0-2]?\d:[0-5]\d\b-\b[0-2]?\d:[0-5]\d\b)\s(?<week>[(][0-9, ]+[)])?(?<subject>[^a-zA-Z0-9_]+)?(?<hall>\d+)?(?<teacher>[^a-zA-Z0-9_]+)?(?<group>[0-9][^a-zA-Z]+)?";

        private readonly static string _htmlTablePattern = @"<table style=""height: 126px; width: 1000px;"" border=""1"">
                                                 <tbody><tr style=""height: 18px;"" >
                                                      <th style=""width: 103.25px; height: 18px; text-align: center;"">Время</th>
                                                      <th style=""width: 103.25px; height: 18px; text-align: center;"">Неделя</th>
                                                      <th style=""width: 103.25px; height: 18px; text-align: center;"">Предмет</th>
                                                      <th style=""width: 103.25px; height: 18px; text-align: center;"">Аудитория</th>
                                                      <th style=""width: 103.25px; height: 18px; text-align: center;"">Преподаватель</th>
                                                      <th style=""width: 103.25px; height: 18px; text-align: center;"">Группа</th>
                                                    </tr>
                                                    <ForContent>    
                                                 </tbody>
                                             </table>";

        private readonly static string _htmlWeekNamePattern = @"<tr><th><ForWeekName></th><th></th><th></th><th></th><th></th><th></th></tr>";

        private readonly static string _htmlRowPattern = @"<tr style=""height: 36px;"" >
                                             <td style=""width: 103.25px; height: 36px; text-align: center;""><ForTime></td>
                                             <td style=""width: 103.25px; height: 36px; text-align: center;""><ForWeekNumber></td>
                                             <td style=""width: 103.25px; height: 36px; text-align: center;""><ForSubject></td>
                                             <td style=""width: 103.25px; height: 36px; text-align: center;""><ForHall></td>
                                             <td style=""width: 103.25px; height: 36px; text-align: center;""><ForTeacher></td>
                                             <td style=""width: 103.25px; height: 36px; text-align: center;""><ForGroup></td>
                                           </tr>";

        public static string ListToHtmlTable(List<string> schudleList, ref int tableSize)
        {
            string resultContent = string.Empty;
            foreach (var row in schudleList)
            {
                Match match = Regex.Match(row, EXPRESSION_ROW_DATA);
                if (match.Success)
                {
                    resultContent += _htmlRowPattern.Replace("<ForTime>", match.Groups["time"].Value)
                                                    .Replace("<ForWeekNumber>", match.Groups["week"].Value)
                                                    .Replace("<ForSubject>", match.Groups["subject"].Value)
                                                    .Replace("<ForHall>", match.Groups["hall"].Value)
                                                    .Replace("<ForTeacher>", match.Groups["teacher"].Value)
                                                    .Replace("<ForGroup>", match.Groups["group"].Value);
                    tableSize += 45;
                }
                else
                {
                    resultContent += _htmlWeekNamePattern.Replace("<ForWeekName>", row.Split(" ")[0]);
                    tableSize += 27;
                }
            }
            tableSize += 27;
            return _htmlTablePattern.Replace("<ForContent>", resultContent);
        }

        public static void HtmlToJpeg(string htmlString, int size, string outputFilePath)
        {
            var browserPath = BrowserFetcher.GetSystemChromePath();
            using var browser = new GcHtmlBrowser(browserPath);

            using var pg = browser.NewPage(htmlString, new PageOptions
            {
                WindowSize = new Size(1020, size),
                DefaultBackgroundColor = Color.AliceBlue
            });
            pg.SaveAsJpeg(outputFilePath);
        }
    }
}
