using ScheduleTelegramBot.Core;
using ScheduleTelegramBot.ResponseUtils;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace ScheduleTelegramBot.Commands
{
    public class TodayScheduleCommand : IBaseCommand
    {
        public string Name => "Today";

        public async void Execute(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var connection = new PSUWebSiteConnection(update.Message.Chat.Id);
            var scheduleFilter = new ScheduleFilter(connection);
            List<string> schudleList = scheduleFilter.GetTodaySchedule();
            int tableSize = 0;
            string htmlTable = HtmlStringEditor.ListToHtmlTable(schudleList, ref tableSize);
            string outputFilePath = "Result.jpeg";
            HtmlStringEditor.HtmlToJpeg(htmlTable, tableSize, outputFilePath);

            using (FileStream stream = new(outputFilePath, FileMode.Open))
            {
                Message sentMessage = await botClient.SendPhotoAsync(
                chatId: update.Message.Chat.Id,
                photo: new InputOnlineFile(stream, outputFilePath),
                cancellationToken: cancellationToken);
            }
        }
    }
}
