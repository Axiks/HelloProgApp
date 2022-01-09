using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelloProgLib;
using HelloProgLib.Models;
using HelloProgLib.Services;
using Microsoft.ServiceBus.Messaging;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace HelloProg.TelegramBot
{
    internal class Program
    {
        static TelegramBotClient botClient;
        static UserService userService;
        static HabbitService habbitService;

        static void Main(string[] args)
        {
            // Заносимо користувача у список користувачів. Якщо він є в базі то вітаємо ще раз
            userService = new UserService();
            habbitService = new HabbitService();

            Console.WriteLine("Telegram bot start");
            InitTelegramBot();

            Console.ReadLine();
            botClient.StopReceiving();

        }

        private static void InitTelegramBot()
        {
            botClient = new TelegramBotClient("5079882233:AAETDqSx6DX1hdp1nkgOW40o009t8UbnRAE");
            botClient.StartReceiving();
            botClient.OnMessage += Bot_OnMessage;
            botClient.OnMessage += Bot_HabitDriver;
            botClient.OnInlineQuery += BotClient_OnInlineQuery;
            botClient.OnMessage += Bot_HabitAdminDriver;
        }

        private static void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            // var message = e.Message;
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                //botClient.SendTextMessageAsync(e.Message.Chat.Id, "Hello" + e.Message.Chat.Username);
                if (e.Message.Text.StartsWith("/start"))
                {
                    if(e.Message.Chat.Type == ChatType.Private)
                    {
                        // Внутрішнє спілкування
                        // botClient.SendTextMessageAsync(e.Message.Chat.Id, "Hello. You start me in the first");
                        long chatId = e.Message.Chat.Id;
                        string userId = e.Message.From.Id.ToString();
                        string username = e.Message.From.Username.ToString();
                        // botClient.SendTextMessageAsync(e.Message.Chat.Id, "Chat ID: " + chatId);
                        // botClient.SendTextMessageAsync(e.Message.Chat.Id, "User ID: " + userId);


                        if (userService.GetUser(userId) == null)
                        {
                            botClient.SendTextMessageAsync(e.Message.Chat.Id, "Приємно з тобою познайомитись )");
                            userService.AddUser(userId, username);
                        }
                        else
                        {
                            botClient.SendTextMessageAsync(e.Message.Chat.Id, "Привіт! Рад тебе бачити знову");
                        }
                    }
                    else
                    {
                        botClient.SendTextMessageAsync(e.Message.Chat.Id, "Щоб зумів з тобою познайомитись, перейди до внутрішнього діалогу з мною..");
                    }
                }

                if (e.Message.Text.StartsWith("/help"))
                {
                    if (e.Message.Chat.Type == ChatType.Private)
                    {
                        botClient.SendTextMessageAsync(e.Message.Chat.Id, "Мої команди:\n/allHabitItem - список захоплень які можеш собі добавити\n/addHabit - добавити собі захоплення\n/delHabit - видалити захоплення\n/myHabit - список моїх захоплень");
                    }
                    else
                    {
                        botClient.SendTextMessageAsync(e.Message.Chat.Id, "Щоб зумів з тобою познайомитись, перейди до внутрішнього діалогу з мною..");
                    }
                }
            }
        }

        private static void Bot_HabitAdminDriver(object sender, MessageEventArgs e)
        {
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                // Усі технології
                if (e.Message.Text.StartsWith("/allHabitItem"))
                {
                    List<Habit> habits = habbitService.AllHabits();
                    string message = "";

                    if (habits.Count != 0)
                    {
                        foreach (Habit habit in habits)
                        {
                            message += habit.title + " " + habit.description + "\n";
                        }
                    }
                    else botClient.SendTextMessageAsync(e.Message.Chat.Id, "Список порожній");
                    botClient.SendTextMessageAsync(e.Message.Chat.Id, $"Список технологій \n { message }");
                }

                // Добавити технологію (тільки адмін)
                if (e.Message.Text.StartsWith("/addHabitItem"))
                {
                    string[] subs = e.Message.Text.Split(' ');
                    if (subs.Length == 2)
                    {
                        string title = subs[1].ToLower();

                        if (habbitService.FindHabbit(title) == null)
                        {
                            string description = "";
                            habbitService.AddHabbit(title, description);
                            botClient.SendTextMessageAsync(e.Message.Chat.Id, $"Елемент { title } добавлено");
                        }
                        else
                        {
                            botClient.SendTextMessageAsync(e.Message.Chat.Id, "Такий елемент уже є");
                        }
                    }
                    else
                    {
                        botClient.SendTextMessageAsync(e.Message.Chat.Id, "Неправильні аргументи команди");
                    }
                }

                if (e.Message.Text.StartsWith("/delHabitItem"))
                {
                    string[] subs = e.Message.Text.Split(' ');
                    if (subs.Length == 2)
                    {
                        string title = subs[1].ToLower();

                        Habit findElement = habbitService.FindHabbit(title);
                        if (findElement == null)
                        {
                            botClient.SendTextMessageAsync(e.Message.Chat.Id, "Такого елементу не існує");
                        }
                        else
                        {
                            habbitService.DeleteHabbit(findElement.id);
                            botClient.SendTextMessageAsync(e.Message.Chat.Id, $"Елемент '{ findElement.title }' видалено");
                        }
                    }
                    else
                    {
                        botClient.SendTextMessageAsync(e.Message.Chat.Id, "Неправильні аргументи команди");
                    }
                }
            }
        }

        private static void Bot_HabitDriver(object sender, MessageEventArgs e)
        {
            UserHabitService userHabitService = new UserHabitService();
            string userId = e.Message.From.Id.ToString();


            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                if (e.Message.Text.StartsWith("/addHabit"))
                {
                    // 
                    // botClient.SendTextMessageAsync(e.Message.Chat.Id, "Введи назву");
                    string argument = GetArgument(e.Message.Text);
                    if (argument != null)
                    {
                        Habit habit = habbitService.FindHabbit(argument);
                        if (habit == null)
                        {
                            botClient.SendTextMessageAsync(e.Message.Chat.Id, "Такого елементу не існує");
                        }
                        else
                        {
                            // Добавити перевірку на те чи в Юзера уже є таке захоплення
                            if (userHabitService.HasUserHabit(userId, habit))
                            {
                                botClient.SendTextMessageAsync(e.Message.Chat.Id, $"Ти вже маєш елемент {habit.title}");
                            }
                            else
                            {
                                userHabitService.AddUserHabit(userId, habit);
                                botClient.SendTextMessageAsync(e.Message.Chat.Id, $"{habit.title} добавлено до вашого списку");
                            }
                        }
                    }
                    else
                    {
                        botClient.SendTextMessageAsync(e.Message.Chat.Id, "Неправильні аргументи команди");
                    }
                }

                if (e.Message.Text.StartsWith("/delHabit"))
                {
                    string argument = GetArgument(e.Message.Text);
                    if (argument != null)
                    {
                        Habit habit = habbitService.FindHabbit(argument);
                        if (habit == null)
                        {
                            botClient.SendTextMessageAsync(e.Message.Chat.Id, "Такого елементу не існує");
                        }
                        else
                        {
                            if (userHabitService.HasUserHabit(userId, habit))
                            {
                                userHabitService.DeleteUserHabit(userId, habit);
                                botClient.SendTextMessageAsync(e.Message.Chat.Id, $"{habit.title} видалено з вашого списку");
                            }
                            else
                            {
                                botClient.SendTextMessageAsync(e.Message.Chat.Id, $"В твоєму списку не має елементу {habit.title}");
                            }
                        }
                    }
                    else
                    {
                        botClient.SendTextMessageAsync(e.Message.Chat.Id, "Неправильні аргументи команди");
                    }
                }

                if (e.Message.Text.StartsWith("/myHabit"))
                {
                    List<Habit> habits = userHabitService.GetUserHabits(userId);
                    if (habits.Count > 0)
                    {
                        string habitString = "Мої захоплення: \n";
                        foreach (Habit habit in habits)
                        {
                            habitString += habit.title + " \n";
                        }
                        botClient.SendTextMessageAsync(e.Message.Chat.Id, habitString);
                    }
                    else
                    {
                        botClient.SendTextMessageAsync(e.Message.Chat.Id, "Ваш список порожній. Добавте елементи");
                    }
                }
            }
        }

        private static string GetArgument(string comandText)
        {
            string[] subs = comandText.Split(' ');
            if (subs.Length == 2)
            {
                return subs[1].ToLower();
            }
            else
            {
                return null;
            }
        }

        private static void BotClient_OnInlineQuery(object sender, InlineQueryEventArgs e)
        {
            // Some variables
            string link = "https://t.me/chat_username";
            // Your Button that you want
            InlineKeyboardButton button = InlineKeyboardButton.WithUrl("Go to chat", link);

             //e.InlineQuery.
            // Inline Results

            List<InlineQueryResultBase> results = new List<InlineQueryResultBase>();
            /*results.Add(new InlineQueryResultLocation(e.InlineQuery.Id, 41, 22, "Loccc")); // OK
            results.Add(new InlineQueryResultLocation("trollor2", 36, 28, "Loc2")); // DONT OK
            results.Add(new InlineQueryResultLocation("trollor3", 36, 28, "Loc4")); // DONT OK
            results.Add((new InlineQueryResultArticle("trollo3", "ttatlie", new InputTextMessageContent("Meessage!!"))));*/

            // Витаскуємо усіх зареганих користувачів
            UserHabitService userHabitService = new UserHabitService();
            List<HelloProgLib.Models.User> users = userHabitService.AllUsersHasHabit();

            int i = 0;
            foreach (var user in users)
            {
                // Знаходимо усі теги користувача
                string message = $"@{user.Username} цікавить: \n";
                List<Habit> userhabits = userHabitService.GetUserHabits(user.TelegramId);
                foreach (Habit habit in userhabits)
                {
                    message += habit.title + "\n";
                }
                results.Add((new InlineQueryResultArticle($"user{i}", user.Username, new InputTextMessageContent(message))));
                i++;
            }
            // Answer with results:
            botClient.AnswerInlineQueryAsync(e.InlineQuery.Id,
                                                   results,
                                                   isPersonal: true,
                                                   cacheTime: 0);
        }
    }
}
