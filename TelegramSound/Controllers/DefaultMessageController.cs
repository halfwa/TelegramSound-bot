using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramSound.Controllers
{
    public class DefaultMessageController
    {
        private readonly ITelegramBotClient _botClient;
        public DefaultMessageController(ITelegramBotClient botClient)
        {
            _botClient = botClient; 
        }
        public async Task Handle(Message message, CancellationToken ct)
        {
            var infoMessage = "Получено сообщение не поддерживаемого формата 😕";
            await _botClient.SendTextMessageAsync(message.Chat.Id, infoMessage, cancellationToken: ct);
        }
    }
}   