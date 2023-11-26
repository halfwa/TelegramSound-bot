using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using TelegramSound.Controllers;

namespace TelegramSound
{
    public class Bot : BackgroundService
    {
        private readonly ITelegramBotClient _botClient;

        private readonly InlineKeyboardController _inlineKeyboardController;
        private readonly TextMessageController _textMessageController;
        private readonly DefaultMessageController _defaultMessageController;

        public Bot(
            ITelegramBotClient botClient,
            InlineKeyboardController inlineKeyboardController,
            TextMessageController textMessageController,
            DefaultMessageController defaultMessageController
            )
        {
            _botClient = botClient;
            _inlineKeyboardController = inlineKeyboardController;
            _textMessageController = textMessageController;
            _defaultMessageController = defaultMessageController;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _botClient.StartReceiving(
                HandleUpdatesAsync,
                HandleErrorAsync,
                new ReceiverOptions() { AllowedUpdates = { } },
                cancellationToken: stoppingToken);

            var me = await _botClient.GetMeAsync();
            Console.WriteLine($"Bot: \"{me.Username}\" is running.\n");
            Console.ReadLine();
        }
        private async Task HandleUpdatesAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {

            // Проверка на то, что пользователь нажал на inline-кнопку
            if (update!.Type == UpdateType.CallbackQuery)
            {
                await  _inlineKeyboardController.Handle(update!.CallbackQuery!, cancellationToken);
                return;
            }

            // Проверка на то, что пользователь отправил сообщение
            if (update.Type == UpdateType.Message && update?.Message?.Text != null)
            { 
                await _textMessageController.Handle(update.Message, cancellationToken);
                return;
            }
            else
            {
                await _defaultMessageController.Handle(update!.Message!, cancellationToken);
                return;
            }
        }

        Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
        {
            // Задаем сообщение об ошибке в зависимости от того, какая именно ошибка произошла
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            // Выводим в консоль информацию об ошибке
            Console.WriteLine(errorMessage);

            // Задержка перед повторным подключением
            Console.WriteLine("Ожидаем 10 секунд перед повторным подключением.");
            Thread.Sleep(10000);

            return Task.CompletedTask;
        }
    }

}
