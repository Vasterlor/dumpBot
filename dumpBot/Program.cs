using Telegram.Bot;
using Telegram.Bot.Types;

namespace dumpBot;

internal class Program
{
    private static async Task SendToMultipleChatsAsync(ITelegramBotClient botClient, List<long> chatIds, string message)
    {
        foreach (var chatId in chatIds) await botClient.SendTextMessageAsync(chatId, message);
    }

    private static void Main(string[] args)
    {
        var Client = new TelegramBotClient("6628402318:AAGVuvBaCQZxxR5MlK7arNzzSgB3uFBu9yc");
        var chatIds = new List<long>
        {
            -1001765136934,
            -4037376004
        };

        Client.StartReceiving(Update, Error);
        var dataTime = DateTime.Now;
        SendToMultipleChatsAsync(Client, chatIds,
            "\ud83d\ude43Привіт пупсики! \n\ud83c\udfc3dumpBot увірвався в чат! \nЗапускаю івент банка матюків.\ud83e\udd2c За кожну лайку я повідомлятиму, що потрібно заплатити. \nЗібрані кошти в кінці тижня підуть на потреби ЗСУ.\ud83e\udee1 \nКому донатити оприділимо разом. Слава Україні!");

        Console.WriteLine("dumpBot запущено");
        Console.ReadLine();
    }

    private static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
    {
        var message = update.Message;
        if (message != null)
        {
            var messageToReplyTo = message?.MessageId; //Отримуємо ID поточного повідомлення, на яке ми відповідаємо
            if (message != null && message.Text != null && message.Chat != null)
            {
                // /chatid - команда для отримання Id групи
                if (message.Text != null && message.Text.StartsWith("/chatid"))
                {
                    var chatId = message.Chat.Id;
                    await botClient.SendTextMessageAsync(chatId, $"ID цієї групи: {chatId}");
                }

                if (message.Text.ToLower().Contains("/ping"))
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "Я все ще тут",
                        replyToMessageId: messageToReplyTo // Вказуємо ID повідомлення, на яке відповідаємо
                    );

                // Привітання
                if (message.Text.ToLower().Contains("привіт"))
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "Привіт пупсик",
                        replyToMessageId: messageToReplyTo // Вказуємо ID повідомлення, на яке відповідаємо
                    );

                if (message.Text.ToLower().Contains("путін"))
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "Безспорно хуйло",
                        replyToMessageId: messageToReplyTo // Вказуємо ID повідомлення, на яке відповідаємо
                    );
            }

            string[] dirtyWords =
                { "блять", "сука", "бля", "хуйня", "підор", "хуйло", "йобана", "пздц", "єбана", "єбаний", "нахуй" };
            if (message != null && message.Text != null && message.Chat != null)
                foreach (var Word in dirtyWords)
                    if (message.Text.ToLower().Contains(Word))
                    {
                        await botClient.SendTextMessageAsync(
                            message.Chat.Id,
                            "За лайку плати в копілку.\ud83d\udcb0 \n\ud83e\udd2c1 брудне слово = 10 грн. \nАбо 20 віджимань.\ud83c\udfcb\ufe0f\u200d\u2640\ufe0f \nПосилання на банку: https://send.monobank.ua/jar/6jstPnFA7M",
                            replyToMessageId: messageToReplyTo); // Вказуємо ID повідомлення, на яке відповідаємо
                        break; // Вийти з циклу, коли знайдено співпадіння
                    }
        }
    }

    private static async Task Error(ITelegramBotClient botClient, Exception error, CancellationToken arg3)
    {
        Console.WriteLine($"Помилка: {error.Message}");

        // Отримати chat_id чату, в який ви хочете відправити повідомлення про помилку
        var chatIds = new List<long>
        {
            -1001765136934,
            -4037376004
        };

        foreach (var chatId in chatIds) await botClient.SendTextMessageAsync(chatId, $"Помилка: {error.Message}");
    }
}