using ScheduleTelegramBot.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ScheduleTelegramBot.Commands
{
    public class StartCommand : IBaseCommand
    {
        public string Name => "/start";

        public async void Execute(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                new KeyboardButton[] { "This week", "Next week" },
                new KeyboardButton[] { "Today", "Tomorrow"},
                new KeyboardButton[] { "All semester" }
            });

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Choose a response",
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: cancellationToken);
        }
    }
}
