using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramSound.Configuration;
using TelegramSound.Services;

namespace TelegramSound.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IFileHandler _fileHandler;

        public TextMessageController(ITelegramBotClient botClient, IFileHandler fileHandler)
        {
             _botClient = botClient;
            _fileHandler = fileHandler;
        }
        public async Task Handle(Message message, CancellationToken ct)
        {
            switch (message.Text)
            {
                case "/start":
                    //Сообщение при начале работы бота - показывает кнопки
                    ReplyKeyboardMarkup keyboard = new(new[] { new KeyboardButton[] { "YouTube ✅" } })
                    {
                        ResizeKeyboard = true
                    };
                    await _botClient.SendTextMessageAsync(message.Chat.Id, text: "Выберите источник", replyMarkup: keyboard);
                    break;

                case "YouTube ✅":
                    InlineKeyboardMarkup button = new(new[] // реализация inline клавиатура под сообщением 
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData(text: "Video (MP4) 🎬", callbackData: "Ссылка на видео + \"видео\"")
                        }
                    });

                    await _botClient.SendTextMessageAsync(message.Chat.Id, text: "Скиньте ссылку на видео для конвертации в аудио формат (MP3) 🎧");
                    await _botClient.SendTextMessageAsync(message.Chat.Id, text: "Что бы скачать видео следуйте инструкции\nПример с получением видео: https://youtu.be/videoId видео", replyMarkup: button);
                    break;

                case string s when s!.StartsWith("https://www.youtube.com/"):

                    await _fileHandler.ProcessAudio(message, ct);

                    // Получение ссылки на видео от пользователя
                    // проверка, что пользователь запросил видео
                    if (message.Text.EndsWith("видео"))
                    {
                        await _fileHandler.ProcessVideo(message, ct);
                    }
                    break;

                default:
                    await _botClient.SendTextMessageAsync(message.Chat.Id, text: "Неизвестная команда 😐 ");
                    break;
            }

        }
    }
}
