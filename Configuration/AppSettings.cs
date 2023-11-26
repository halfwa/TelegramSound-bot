using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramSound.Configuration
{
    public class AppSettings
    {
        /// <summary>
        /// Токен Telegram API
        /// </summary>
        public string BotToken { get; set; }
        /// <summary>
        /// Папка загрузки аудио и видео файлов 
        /// </summary>
        public string DownloadsFolder { get; set; }
    }
}
