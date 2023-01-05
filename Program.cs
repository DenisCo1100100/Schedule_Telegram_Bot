using ScheduleTelegramBot.Core;
using ScheduleTelegramBot.ResponseUtils;

namespace ScheduleTelegramBot
{
    public class Program
    {
        static void Main(string[] args)
        {
            new HtmlStringEditor();
            new Bot();
            Console.ReadKey();
        }
    }
}