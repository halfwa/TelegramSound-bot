using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using YoutubeExplode.Videos.Streams;
using YoutubeExplode;
using TelegramSound.Configuration;
using File = System.IO.File;
using Telegram.Bot;

namespace TelegramSound.Services
{
    public class FileHandler : IFileHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly AppSettings _settings;
        public FileHandler(ITelegramBotClient botClient, AppSettings appSettings)
        {
            _settings = appSettings;
            _botClient = botClient;
        }
        public async Task ProcessAudio(Message message, CancellationToken ct)
        {
            var dirPath = _settings.DownloadsFolder;
            var filePath = $@"{dirPath}\audio.mp3";
            var link = message.Text.Split()[0];

            // сохранение этого видео на диск с заменой существующего и отправка его в телеграм
            var client = new YoutubeClient();

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
                if (File.Exists(filePath)) File.Delete(filePath);
            }

            try
            {
                var sourceInfo = client.Videos.GetAsync(link).Result;

                var streamManifest = await client.Videos.Streams.GetManifestAsync(link);
                var streamInfo = (AudioOnlyStreamInfo)streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

                await client.Videos.Streams.DownloadAsync(streamInfo, filePath);
                using (FileStream fileStream = new(filePath, FileMode.Open))
                {
                    var fileName = sourceInfo.Title;

                    await _botClient.SendAudioAsync(
                        chatId: message.Chat.Id,
                        audio: InputFile.FromStream(fileStream, fileName),
                        title: fileName);
                }
            }
            catch
            {
                await _botClient.SendTextMessageAsync(message.Chat.Id, text: "Произошла ошибка 😕. Возможные причины: \n1. Объем файла слишком велик ❌ \n2. Отправленная ссылка некорректна ❌ ");
            }
        }

        public async Task ProcessVideo(Message message, CancellationToken ct)
        {
            var dirPath = _settings.DownloadsFolder;
            var filePath = $@"{dirPath}\video.mp4";
            var link = message.Text.Split()[0];

            // сохранение этого видео на диск с заменой существующего и отправка его в телеграме
            var client = new YoutubeClient();

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
                if (File.Exists(filePath)) File.Delete(filePath);
            }

            try
            {
                var sourceInfo = client.Videos.GetAsync(link).Result;

                var streamManifest = await client.Videos.Streams.GetManifestAsync(link);
                var streamInfo = (MuxedStreamInfo)streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();

                await client.Videos.Streams.DownloadAsync(streamInfo, filePath);
                using (FileStream fileStream = new(filePath, FileMode.Open))
                {
                    var fileName = sourceInfo.Title;
                    await _botClient.SendVideoAsync(
                        chatId: message.Chat.Id,
                        video: InputFile.FromStream(fileStream, fileName));
                }
            }
            catch
            {
                await _botClient.SendTextMessageAsync(message.Chat.Id, text: "Произошла ошибка 😕. Возможные причины: \n1. Объем файла слишком велик ❌ \n2. Отправленная ссылка некорректна ❌ ");
            }

        }

    }
}
