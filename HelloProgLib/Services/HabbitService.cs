using HelloProgLib.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloProgLib.Services
{
    public class HabbitService : IHabitService
    {
        private SqliteConnection connection;

        public HabbitService()
        {
            connection = DBProvider.GetConnection();
        }

        public void AddHabbit(String title, String description)
        {
            using (var transaction = connection.BeginTransaction())
            {
                var insertCmd = connection.CreateCommand();
                insertCmd.CommandText = $"INSERT INTO HABBIT (title, description) VALUES ('{title}', '{description}')";
                insertCmd.ExecuteNonQuery();

                transaction.Commit();
            }
        }

        public List<Habit> AllHabits()
        {
            List<Habit> habits = new List<Habit>();
            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = $"SELECT * FROM HABBIT";
            using (var reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = Int32.Parse(reader.GetString(0));
                    string title = reader.GetString(1).ToString();
                    string description = reader.GetString(2).ToString();

                    Habit habit = new Habit(id, title, description);

                    habits.Add(habit);
                }
            }
            return habits;
        }

        public void DeleteHabbit(int id)
        {
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE FROM HABBIT WHERE id='{id}'";
            tableCmd.ExecuteNonQuery();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Habit FindHabbit(string title)
        {
            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = $"SELECT * FROM HABBIT WHERE title = '{title}'";

            using (var reader = selectCmd.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    return null;
                }
                reader.Read();

                int id = Int32.Parse(reader.GetString(0).ToString());
                string description = reader.GetString(2).ToString();

                Habit habit = new Habit(id, title, description);
                return habit;
            }

        }
        public Habit GetHabbit(int id)
        {
            string title = "";
            string description = "";

            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = $"SELECT * FROM HABBIT WHERE id = '{id}'";
            using (var reader = selectCmd.ExecuteReader())
            {
                reader.Read();
                title = reader.GetString(1).ToString();
                description = reader.GetString(2).ToString();
            }

            Habit habit = new Habit(id, title, description);

            return habit;
        }
    }
}
