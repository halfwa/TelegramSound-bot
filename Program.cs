using Telegram.Bot;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TelegramSound.Configuration;
using TelegramSound;
using Microsoft.Extensions.Configuration;
using TelegramSound.Controllers;
using TelegramSound.Services;
using TelegramSound.Extensions;



var host = new HostBuilder()
    .ConfigureServices((hostContext, services) =>
    {
        AppSettings appSettings = BuildAppSettings();

        services.AddSingleton(appSettings);

        services.AddTransient<IFileHandler, FileHandler>();

        // Подключаем контроллеры сообщений и кнопок
        services.AddTransient<TextMessageController>();
        services.AddTransient<InlineKeyboardController>();
        services.AddTransient<DefaultMessageController>();

        // Регистрируем объект TelegramBotClient c токеном подключения
        services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));

        // Регистрируем постоянно активный сервис бота
        services.AddHostedService<Bot>();
    }
    )
    .UseConsoleLifetime() 
    .Build();

Console.WriteLine("Сервис запущен");
await host.RunAsync();

static AppSettings BuildAppSettings()
{
    return new AppSettings()
    {
        BotToken = "YOUR API TOKEN",
        DownloadsFolder = Path.Combine(DirectoryExtension.GetProjectPath(), "Downloads")
    };
}   
