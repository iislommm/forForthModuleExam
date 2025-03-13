using ExamBot.Dal;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ChatBot.Api
{
    internal class Program
    {
        private static string BotToken = "7653083369:AAE6Zz1A-Pk3YfBdjr5lHhhNLM9rpLhw2KE";
        private static TelegramBotClient BotClient = new TelegramBotClient(BotToken);
        private static string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "userIds");
        //private static List<long> Ids = new List<long>();
        private static HashSet<string> Datas = new HashSet<string>();
        private static HashSet<long> Ids = new HashSet<long>();


        static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<MainContext>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            //Console.WriteLine("Hello, World!");
            if (!File.Exists(FilePath))
            {
                File.WriteAllText(FilePath, "[]");
            }
            var recieverOptions = new ReceiverOptions { AllowedUpdates = new[] { UpdateType.Message, UpdateType.InlineQuery } };
            Console.WriteLine("Done");
            BotClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                recieverOptions);
            Console.ReadKey();
        }
        static async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message)
            {
                Ids = await GetAllIds();
                var message = update.Message;
                var user = message.Chat;
                Console.WriteLine(user.Id);
                if (message.Text == "/start")
                {
                    await SaveUserId();
                    Ids.Add(user.Id);



                    var menu = new ReplyKeyboardMarkup(new[]
                    {
                   new KeyboardButton[] { "Fill Data", "Get Data", "Delete Data" },
                })

                    {
                        ResizeKeyboard = true,
                        OneTimeKeyboard = true
                    };

                    await bot.SendTextMessageAsync(user.Id, "Assalomu alaikum, welcome", replyMarkup: menu);
                    return;
                }
                if (message.Text == "Fill Data")
                {
                    await bot.SendTextMessageAsync(user.Id, "Please enter datass about yourself on this queue👇", cancellationToken: cancellationToken);
                    await bot.SendTextMessageAsync(user.Id, "Name - your first name\nLast Name  - your last name\nPhoneNumber - your personal phone number\nAddress - Enter an region you are living" +
                        "\nEmail - your active email address", cancellationToken: cancellationToken);
                    await bot.SendTextMessageAsync(user.Id, "FOR EXAMPLE 🫲🏻😐🫱🏻", cancellationToken: cancellationToken);
                    await bot.SendTextMessageAsync(user.Id, "Name - Islom\nLast Name - O'ktamaliyev\nPhoneNumber - 93 912 96 06\nAddress - Tashkent" +
                        "\nEmail - islomoktamaliyev06@gmail.com", cancellationToken: cancellationToken);
                    await bot.SendTextMessageAsync(user.Id, "Now your turn👀", cancellationToken: cancellationToken);
                   // Console.ReadKey();
                   // Console.ReadLine();
                      if (message.Text.Contains("Name - "))
                      {
                          await SaveUserData();
                        await bot.SendTextMessageAsync(user.Id, "Your datas successfully added 😃", cancellationToken: cancellationToken);                    }
                      else
                      {
                          await bot.SendTextMessageAsync(user.Id, "Mmm something went wrong 😐\n" +
                          "recheck and try again later 🫠", cancellationToken: cancellationToken);
                          await bot.SendTextMessageAsync(user.Id, "Don't forget to restart the bot to try agian 🫠", cancellationToken: cancellationToken);
                          await bot.SendTextMessageAsync(user.Id, "/start", cancellationToken: cancellationToken);
                      }
                }else if (message.Text == "Get Data")
                {
                    await bot.SendTextMessageAsync(user.Id, $"Username : {user.Username}\nFirstName : {user.FirstName}\nUserId : {user.Id}", cancellationToken: cancellationToken);
                    await bot.SendTextMessageAsync(user.Id, "/start", cancellationToken: cancellationToken);
                }
                else if (message.Text == "Delete Data")

                {
                    await bot.SendTextMessageAsync(user.Id, "Beacuse of repairing you can't delete your data right now ", cancellationToken: cancellationToken);
                    await bot.SendTextMessageAsync(user.Id, "/start", cancellationToken: cancellationToken);
                }



            }

            else if (update.Type == UpdateType.CallbackQuery)
            {
                var message = update.CallbackQuery.Message;
                var id = update.CallbackQuery.From.Id;
                await bot.SendTextMessageAsync(id, $"Your option is {update.CallbackQuery.Message}");
            }
        }

        static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {

        }
        public static async Task SaveUserData()
        {
            var serializedIds = JsonSerializer.Serialize(Datas);
            await File.WriteAllTextAsync(FilePath, serializedIds);
        }
        public static async Task SaveUserId()
        {
            var serializedIds = JsonSerializer.Serialize(Ids);
            await File.WriteAllTextAsync(FilePath, serializedIds);
        }
        public static async Task<HashSet<string>> GetAllDatas()
        {
            var DataString = File.ReadAllText(FilePath);
            var Datas = JsonSerializer.Deserialize<HashSet<string>>(DataString);
            return Datas;
        }
        public static async Task<HashSet<long>> GetAllIds()
        {
            var IdsString = File.ReadAllText(FilePath);
            var Ids = JsonSerializer.Deserialize<HashSet<long>>(IdsString);
            return Ids ?? new HashSet<long>();
        }


    }
}
