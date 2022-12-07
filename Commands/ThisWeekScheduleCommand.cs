using ScheduleTelegramBot.Core;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ScheduleTelegramBot.Commands
{
    public class ThisWeekScheduleCommand : IBaseCommand
    {
        public string Name => "This week";

        public async void Execute(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: $"{update.Message.Chat.FirstName}, this function is not working now!",
                cancellationToken: cancellationToken);
        }
    }
}
