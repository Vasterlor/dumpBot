using dumpBot.Users;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace dumpBot;

internal class Program
{
    private static ChatUsers chatUsers = new();
    private static UserNameDumpTest userNameDumpTest = new();
    private static UserNameNaPivch userNameNaPivch = new();
    private static readonly Chats Chats = new();


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

    private static async Task Main(string[] args)
    {
        var Client = new TelegramBotClient("6628402318:AAGVuvBaCQZxxR5MlK7arNzzSgB3uFBu9yc");
        var chatIds = new List<long>
        {
            //Chats.dampTest,
            //Chats.naPivch
        };
        //DumpTestUsers
        chatUsers.AddDumpTestUser(userNameDumpTest.sashakuzo);
        chatUsers.AddDumpTestUser(userNameDumpTest.fourten);

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
        await SendToMultipleChatsAsync(Client, chatIds,
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
        if (message != null && IsChatAllowed(message.Chat.Id))
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

                if (message.Text.ToLower().Contains("/roll"))
                {
                    Random rand = new Random();
                    int number = rand.Next(1, 101);
                    if (number < 10)
                    {
                        await botClient.SendTextMessageAsync(
                            message.Chat.Id,
                            text: "😂 " + Convert.ToString(number),
                            replyToMessageId: messageToReplyTo // Вказуємо ID повідомлення, на яке відповідаємо
                        );
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(
                          message.Chat.Id,
                          text: Convert.ToString(number),
                          replyToMessageId: messageToReplyTo // Вказуємо ID повідомлення, на яке відповідаємо
                        );
                    }

                }

                if (message.Text.ToLower().Contains("/duel"))
                {
                    string stone = "🪨";
                    string scissors = "✂️";
                    string paper = "🧻";
                    Random rand = new Random();
                    int number = rand.Next(0, 3);
                    if (number == 0)
                    {
                        await botClient.SendTextMessageAsync(
                            message.Chat.Id,
                            text: stone,
                            replyToMessageId: messageToReplyTo // Вказуємо ID повідомлення, на яке відповідаємо
                        );
                    }
                    else if (number == 1)
                    {
                        await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        text: scissors,
                        replyToMessageId: messageToReplyTo // Вказуємо ID повідомлення, на яке відповідаємо
                        );
                    }
                    else if (number == 2)
                    {
                        await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        text: paper,
                        replyToMessageId: messageToReplyTo // Вказуємо ID повідомлення, на яке відповідаємо
                        );
                    }
                }

                if (message.Text.ToLower().Contains("/banka"))
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "Посилання на банку: " + bankaUrl,
                        replyToMessageId: messageToReplyTo // Вказуємо ID повідомлення, на яке відповідаємо
                    );

                if (message.Text.ToLower() == "сонце")
                {
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "Вітаю! Ти отримав імунітет до матів до кінця дня!",
                        replyToMessageId: messageToReplyTo // Вказуємо ID повідомлення, на яке відповідаємо
                    );
                }

                if (message.Text.ToLower().StartsWith("/word"))
                {
                    var user = message.From;
                    // Отримуємо текст команди без самої команди
                    string commandText = message.Text.Substring("/word".Length).Trim();

                    // Перевіряємо, чи є текст команди не порожнім
                    if (!string.IsNullOrWhiteSpace(commandText))
                    {
                        // Якщо /word має текст, відправляємо "Переслав модератору"
                        await botClient.SendTextMessageAsync(
                            message.Chat.Id,
                            "Переслав модератору",
                            replyToMessageId: message.MessageId
                        );

                        // Відправляємо оригінальне повідомлення в інший чат
                        await botClient.SendTextMessageAsync(
                            Chats.dirtyWords,
                            $"Копія повідомлення від {user.FirstName} {user.LastName} \\({user.Username}\\) :\n```{commandText}```",
                            parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2
                        );
                    }
                    else
                    {
                        // Якщо /word без тексту, відправляємо "добавте брудне слово."
                        await botClient.SendTextMessageAsync(
                            message.Chat.Id,
                            "добавте брудне слово.",
                            replyToMessageId: message.MessageId
                        );
                    }
                }

                // /traitor рандомний пошук юзера із 
                if (message.Text.ToLower().Contains("/traitor"))
                {
                    var random = new Random();

                    if (message.Chat.Id == Chats.dampTest)
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
                    else if (message.Chat.Id == Chats.naPivch)
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
                    replyToMessageId: message.MessageId
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
                        "блять", "блядь", "бля", "блд", "блядина", "блядіна", "ублюдок", "ублюдку", "ублюдки",  "ублюдка", " ублюдком", "хуй", "хуйот", "хер", "ніхера", "ніхуя", "хуйні", "хуйню",
                        "нихера", "похер", "похеру", "похуе", "хуярять", "хуярить", "хуярити", "хуяритиме", "нихуяж", "ніхуяж", "хуя",
                        "ахує", "охуї", "ахуї", "охуїв", "охуїваю", "хуіта", "уйобки", "кончені", "уєбани", "уєбан", "єбля", "йобля", "єбліще", "Наволоч", "Лярва",
                        "хуйня", "хуїта", "хуїту", "хуєт", "хєр", "пох", "піхуй", "пиздос", "заєбаний", "похую", "хуєта", "хуєту", "хуєтІ", "хитровиєбаний",
                        "нах", "нахуй", "хуєсос", "хуєсоси", "підор", "підар", "підарам", "пидорі", "Пидарье", "Пидорье", "йобаний", "хуєфікатор", "йобана",
                        "єбана", "єбати", "ебаная", "Їбать", "їбуча", "єбать", "єбаний", "єбошить", "йбн", "заєбав", "заєбався", "заїбав", "доєбатись",
                        "їбати", "їбаний", "єбанько", "їбанько", "хулі", "єбуть", "пиздять", "піздить", "сєбався", "нахуя", "доєбались", "хуєрі", "пиздюк", "єбе",
                        "пизда", "пізда", "піздато", "пізди", "пизди", "піздабол", "пиздабол", "пиздець", "піздєц", "пздц", "пиздеж", "пиздежа", "напиздів",
                        "мудло", "мудак", "сука", "ссука", "пиздів", "їбані", "допиздівся", "мля", "хуях", "похуй", "ахуй", "пиздуй", "вйобаному",
                        "сучка", "сучара", "конча", "кончений", "кончена", "мудило", "курва",
                        "курвище", "xyй",
                        "курвий", "курви", "вкурвлювати", "yobana", "blyad", "найобуєте", "найобуєш", "найобує", "наєбав", "найобував", "найобщик", "наїбав", "наїбали", "наїбалово", "їбали", "заїбали", "єбали", "заєбали", "наебалово",
                        "наёбуете", "наёбуеш", "наебал", "наёбщик"
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
                            "\"За лайку плати в копілку.\"💰\n\"В даному повідомленні використали брудні слова:\"";

                        foreach (var kvp in wordsAndPositions) responseText += $"\n{"🤘🏿  " + kvp.Key}: {kvp.Value.Count} шт.";
                        responseText += $"\n\n\"Загальна кількість брудних слів:\" {totalDirtyWordsCount}\ud83d\ude33\n";
                        responseText +=
                            "\n\"1 брудне слово = 10 грн.\"\ud83d\udcb5\n\"Або 20 віджимань.\"🏋️‍♀️\n\nСловом ти попав на: " +
                            totalDirtyWordsCount * price + " грн, або " + totalDirtyWordsCount * numberPushUps +
                            " віджимань\ud83d\ude05" + "\n\n👇🏿 \"Посилання на\" \ud83e\uded9 👇🏿";

                        var escapedBankaUrl = bankaUrl.Replace(".", "\\.");
                        responseText = $"```csharp\n{responseText}\n``` \n👀 👉  {escapedBankaUrl}";


                        await botClient.SendTextMessageAsync(
                            message.Chat.Id,
                            responseText,
                            replyToMessageId: messageToReplyTo,
                            parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2);
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
                            "\n/banka - повідомляю посилання на банку" +
                            "\n/roll - у кого більший той і прав" +
                            "\n/duel - Камінь, ножникі, папір" +
                            "\n/word - після команди добавте <Брудне> слово щоб передати його модератору",
                            replyToMessageId: messageToReplyTo // Вказуємо ID повідомлення, на яке відповідаємо
                        );
                }
            }
        }
    }

    private static bool IsChatAllowed(long chatId)
    {
        // Перевіряємо, чи чат присутній у списку дозволених чатів з класу Chats
        return chatId == Chats.naPivch
            || chatId == Chats.dampTest
            || chatId == Chats.dirtyWords;
    }


    private static async Task Error(ITelegramBotClient botClient, Exception error, CancellationToken arg3)
    {
        Console.WriteLine($"{DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss")} Помилка: {error.Message}");

        // Отримати chat_id чату, в який ви хочете відправити повідомлення про помилку
        var chatIds = new List<long>
        {
            //Chats.naPivch,
            Chats.dampTest
        };

        foreach (var chatId in chatIds)
            await botClient.SendTextMessageAsync(
                chatId,
                $"УпсіДупсі! Помилка: {error.Message} " + userNameNaPivch.sashakuzo);
    }
}