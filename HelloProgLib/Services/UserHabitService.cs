using HelloProgLib.Interfaces;
using HelloProgLib.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloProgLib.Services
{
    public class UserHabitService : IUserHabitService
    {
        private SqliteConnection connection;
        private UserService userService;
        private HabbitService habbitService;
        public UserHabitService()
        {
            connection = DBProvider .GetConnection();
            userService = new UserService();
            habbitService = new HabbitService();
        }
        public void AddUserHabit(string telegramId, Habit habit)
        {
            using (var transaction = connection.BeginTransaction())
            {
                var insertCmd = connection.CreateCommand();
                insertCmd.CommandText = $"INSERT INTO HABIT_USER (user_id, habit_id) VALUES ('{telegramId}', '{habit.id}')";
                insertCmd.ExecuteNonQuery();

                transaction.Commit();
            }
        }

        public List<User> AllUsersHasHabit()
        {
            List<User> users = new List<User>();

            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = $"SELECT * FROM HABIT_USER group by user_id";
            using (var reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = Int32.Parse(reader.GetString(0));
                    string userId = reader.GetString(1).ToString();
                    int HabitId = Int32.Parse(reader.GetString(2));

                    User user = userService.GetUser(userId);
                    users.Add(user);
                }
            }
            return users;
        }

        public void DeleteUserHabit(string telegramId, Habit habit)
        {
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE FROM HABIT_USER WHERE user_id='{telegramId}' AND habit_id = '{habit.id}'";
            tableCmd.ExecuteNonQuery();
        }

        public List<User> GetHabitUsers(int habitId)
        {
            List<User> users = new List<User>();
            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = $"SELECT * FROM HABIT_USER WHERE habit_id = '{habitId}'";
            using (var reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = Int32.Parse(reader.GetString(0));
                    string userId = reader.GetString(1).ToString();
                    string HabitId = reader.GetString(2).ToString();

                    User user = userService.GetUser(userId);

                    users.Add(user);
                }
            }
            return users;
        }

        public List<Habit> GetUserHabits(string telegramId)
        {
            List<Habit> habits = new List<Habit>();
            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = $"SELECT * FROM HABIT_USER WHERE user_id = '{telegramId}'";
            using (var reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = Int32.Parse(reader.GetString(0));
                    string userId = reader.GetString(1).ToString();
                    int HabitId = Int32.Parse(reader.GetString(2));

                    Habit habit = habbitService.GetHabbit(HabitId);

                    habits.Add(habit);
                }
            }
            return habits;
        }

        public bool HasUserHabit(string telegramId, Habit habit)
        {
            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = $"SELECT * FROM HABIT_USER WHERE user_id='{telegramId}' AND habit_id = '{habit.id}'";
            return selectCmd.ExecuteReader().HasRows;
        }
    }
}
