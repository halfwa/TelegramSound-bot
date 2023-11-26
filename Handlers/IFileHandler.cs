using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramSound.Services
{
    public interface IFileHandler
    {
        Task ProcessAudio(Message message, CancellationToken ct);
        Task ProcessVideo(Message message, CancellationToken ct);
    }
}
