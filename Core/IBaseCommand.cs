using Telegram.Bot;
using Telegram.Bot.Types;

namespace ScheduleTelegramBot.Core
{
    public interface IBaseCommand
    {
        public string Name { get; }
        public void Execute(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);

    }
}
