using System.Text.RegularExpressions;
using dumpBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace dumpBot;

internal class Program
{
    private static ChatUsers chatUsers = new();
    private static UserNameDumpTest userNameDumpTest = new();
    private static UserNameNaPivch userNameNaPivch = new();

    //мультічат і обробка помилок при відправленні повідомлень в різні чати
    private static async Task SendToMultipleChatsAsync(ITelegramBotClient botClient, List<long> chatIds, string message)
    {
        foreach (var chatId in chatIds)
            try
            {
                await botClient.SendTextMessageAsync(chatId, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при відправленні в чат {chatId}: {ex.Message}");
            }
    }

    private static void Main(string[] args)
    {
        var Client = new TelegramBotClient("6628402318:AAGVuvBaCQZxxR5MlK7arNzzSgB3uFBu9yc");
        var chatIds = new List<long>
        {
            -1001765136934, // naPivch
            -1001902063585 // dampTest
        };
        //DumpTestUsers
        chatUsers.AddDumpTestUser(userNameDumpTest.sashakuzo);
        chatUsers.AddDumpTestUser(userNameDumpTest.tod993);

        //NaPivchUsers
        chatUsers.AddNaPivchUser(userNameNaPivch.sashakuzo);
        chatUsers.AddNaPivchUser(userNameNaPivch.hroshko_p);
        chatUsers.AddNaPivchUser(userNameNaPivch.roonua1);
        chatUsers.AddNaPivchUser(userNameNaPivch.Healermanrober);
        chatUsers.AddNaPivchUser(userNameNaPivch.Kostya);
        chatUsers.AddNaPivchUser(userNameNaPivch.Рузана);
        chatUsers.AddNaPivchUser(userNameNaPivch.iamfuss);


        Client.StartReceiving(Update, Error);
        var startDataTime = DateTime.Now;
        SendToMultipleChatsAsync(Client, chatIds,
            "\ud83d\ude43Привіт пупсики! \n\ud83c\udfc3dumpBot увірвався в чат! \nЗапускаю івент банка матюків.\ud83e\udd2c За кожну лайку я повідомлятиму, що потрібно заплатити. \nЗібрані кошти в кінці тижня підуть на потреби ЗСУ.\ud83e\udee1 \nКому донатити оприділимо разом. \n/help - довідник команд. \nСлава Україні!");

        Console.WriteLine(startDataTime + " - dumpBot запущено");
        Console.ReadLine();
    }

    private static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
    {
        var bankaUrl = "https://send.monobank.ua/jar/6jstPnFA7M";
        if (update == null)
            // Перевіряємо, чи update не є null.
            return;

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

                if (message.Text.ToLower().Contains("/banka"))
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "Посилання на банку: " + bankaUrl,
                        replyToMessageId: messageToReplyTo // Вказуємо ID повідомлення, на яке відповідаємо
                    );

                // /traitor рандомний пошук юзера із 
                if (message.Text.ToLower().Contains("/traitor"))
                {
                    var random = new Random();

                    if (message.Chat.Id == -1001902063585) // dampTest
                    {
                        var userCount = chatUsers.DumpTestUsers.Count;
                        if (userCount > 0)
                        {
                            var randomUserId = random.Next(1, userCount + 1);
                            if (chatUsers.DumpTestUsers.TryGetValue(randomUserId, out var randomUser))
                                await botClient.SendTextMessageAsync(
                                    message.Chat.Id,
                                    "Зрадник: " + randomUser,
                                    replyToMessageId: messageToReplyTo // Вказуємо ID повідомлення, на яке відповідаємо
                                );
                        }
                    }
                    else if (message.Chat.Id == -1001765136934) // naPivch
                    {
                        var userCount = chatUsers.NaPivchUsers.Count;
                        if (userCount > 0)
                        {
                            var randomUserId = random.Next(1, userCount + 1);
                            if (chatUsers.NaPivchUsers.TryGetValue(randomUserId, out var randomUser))
                                await botClient.SendTextMessageAsync(
                                    message.Chat.Id,
                                    "Зрадник: " + randomUser,
                                    replyToMessageId: messageToReplyTo // Вказуємо ID повідомлення, на яке відповідаємо
                                );
                        }
                    }
                }

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

            if (message != null && message.Text != null && message.Chat != null)
            {
                string[] ignoredWords = { "Конча-" };

                // Перевірте, чи повідомлення містить ігноровані слова або фрази
                var shouldIgnoreMessage =
                    ignoredWords.Any(word => message.Text.Contains(word, StringComparison.OrdinalIgnoreCase));

                if (!shouldIgnoreMessage)
                {
                    // Оголошення ініціалізованого словника поза циклом
                    var wordsAndPositions = new Dictionary<string, List<int>>();
                    var totalDirtyWordsCount = 0;
                    string[] dirtyWords =
                    {
                        "блять", "блядь", "бля", "блядина", "блядіна", "ублюдок", "хуй", "хуйот", "хер", "ніхера",
                        "нихера",
                        "ахує", "охуї", "охуїв", "охуїваю",
                        "хуйня",
                        "хуйло", "нахуй", "хуєсос", "підор", "підар", "підарам", "йобаний", "хуєфікатор", "йобана",
                        "єбана", "єбати", "єбаний",
                        "їбати", "їбаний", "єбанько", "їбанько",
                        "пизда", "пізда", "піздабол", "пиздабол", "пиздець", "піздєц", "пздц", "пиздеж", "пиздежа",
                        "мудло", "мудак", "сука",
                        "сучка", "сучара", "конча", "кончений", "кончена", "мудило", "мудак", "мудло", "курва",
                        "курвище",
                        "курвий", "курви"
                    };
                    var price = 10;
                    var numberPushUps = 20;

                    foreach (var word in dirtyWords)
                    {
                        var pattern =
                            $@"\b{Regex.Escape(word)}\b"; // Створення регулярного виразу для пошуку цілого слова
                        var matches = Regex.Matches(message.Text, pattern, RegexOptions.IgnoreCase);

                        if (matches.Count > 0)
                        {
                            if (!wordsAndPositions.ContainsKey(word)) wordsAndPositions[word] = new List<int>();

                            foreach (Match match in
                                     matches)
                                wordsAndPositions[word].Add(match.Index); // Збережіть позиції знайдених слів
                            totalDirtyWordsCount += matches.Count;
                        }
                    }

                    if (wordsAndPositions.Count > 0)
                    {
                        // Побудова текстового рядка на основі словника
                        var responseText =
                            "За лайку плати в копілку.💰\nВ данмоу повідомленні використали брудні слова:";

                        foreach (var kvp in wordsAndPositions) responseText += $"\n{kvp.Key}: {kvp.Value.Count} шт.";
                        responseText += $"\n\nЗагальна кількість брудних слів: {totalDirtyWordsCount}\ud83d\ude33\n";
                        responseText +=
                            "\n1 брудне слово = 10 грн.\ud83d\udcb5\nАбо 20 віджимань.🏋️‍♀️\n\nСловом ти попав на: " +
                            totalDirtyWordsCount * price + " грн, або " + totalDirtyWordsCount * numberPushUps +
                            " віджимань\ud83d\ude05" + "\nПосилання на \ud83e\uded9: " + bankaUrl;

                        await botClient.SendTextMessageAsync(
                            message.Chat.Id,
                            responseText,
                            replyToMessageId: messageToReplyTo);
                    }


                    if (message.Text != null && message.Text.ToLower().Contains("/dirtywords"))
                    {
                        var dirtyWordsList = string.Join(", ", dirtyWords);
                        await botClient.SendTextMessageAsync(
                            message.Chat.Id,
                            "Словник брудних слів (НЕ ЧИТАТИ В ГОЛОС): \n" + dirtyWordsList,
                            replyToMessageId: messageToReplyTo
                        );
                    }

                    //команда /help - відправляє повідомлення із списком команд 
                    if (message.Text != null && message.Text.ToLower().Contains("/help"))
                        await botClient.SendTextMessageAsync(
                            message.Chat.Id,
                            "Я dumpBot і я знаю команди:" +
                            "\n/chatid - показую id даного чату" +
                            "\n/ping - перевіряю чи бот активний в чаті" +
                            "\n/dirtyWords - показую словник брудних слів" +
                            "\n/traitor - знаходжу зрадника" +
                            "\n/banka - повідомляю посилання на банку",
                            replyToMessageId: messageToReplyTo // Вказуємо ID повідомлення, на яке відповідаємо
                        );
                }
            }
        }
    }

    private static async Task Error(ITelegramBotClient botClient, Exception error, CancellationToken arg3)
    {
        Console.WriteLine($"Помилка: {error.Message}");

        // Отримати chat_id чату, в який ви хочете відправити повідомлення про помилку
        var chatIds = new List<long>
        {
            -1001765136934, // naPivch
            -1001902063585 // dampTest
        };

        foreach (var chatId in chatIds) await botClient.SendTextMessageAsync(chatId, $"Помилка: {error.Message}");
    }
}