using ScheduleTelegramBot.Commands;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ScheduleTelegramBot.Core
{
    internal class Bot
    {
        private List<IBaseCommand> Commands { get; set; }

        public Bot()
        {
            string token = System.IO.File.ReadAllText("BotToken.txt");
            var botClient = new TelegramBotClient(token);
            using var cts = new CancellationTokenSource();
            
            Commands = new List<IBaseCommand>()
            {
                new StartCommand(),
                new ThisWeekScheduleCommand(),
                new NextWeekScheduleCommand(),
                new TodayScheduleCommand(),
                new TomorrowScheduleCommand()
            };

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );
        }

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;
            if (message.Text is not { } messageText)
                return;

            Console.WriteLine($"[REQUEST]-> ChatID = {message.Chat.Id}, FirstName = {message.Chat.FirstName}: '{messageText}'");

            var findCommand = Commands.Find(command => command.Name == messageText);
            if(findCommand != null)
            {
                findCommand.Execute(botClient, update, cancellationToken);
            }
            else
            {
                Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"I do not understand. I don't have an answer for this command",
                cancellationToken: cancellationToken);
            }
        }

        Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

    }
}
