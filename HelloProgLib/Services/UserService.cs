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
    public class UserService : IUserService
    {
        private SqliteConnection connection;
        public UserService()
        {
            connection = DBProvider.GetConnection();
        }

        public void AddUser(string telegramId, string username)
        {
            using (var transaction = connection.BeginTransaction())
            {
                DateTime foo = DateTime.Now;
                long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();

                var insertCmd = connection.CreateCommand();
                insertCmd.CommandText = $"INSERT INTO USER (telegram_id, username, accession_time) VALUES ('{telegramId}', '{username}', '{unixTime}')";
                insertCmd.ExecuteNonQuery();

                transaction.Commit();
            }
        }

        public void DeleteUser(User user)
        {
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE FROM USER WHERE telegram_id='{user.TelegramId}'";
            tableCmd.ExecuteNonQuery();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public User GetUser(string telegramId)
        {
            string username = "";
            string about = "";
            int unixtime = 0;

            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = $"SELECT * FROM USER WHERE telegram_id = '{telegramId}'";
            using (var reader = selectCmd.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    return null;
                }
                // 
                // ?
                if (reader.FieldCount == 0)
                {
                    return null;
                }

                reader.Read();
                if (!reader.IsDBNull(1))
                {
                    username = reader.GetString(1).ToString();
                }
                else return null;

                if (!reader.IsDBNull(2))
                {
                    about = reader.GetString(2);
                }

                unixtime = Int32.Parse(reader.GetString(3));
            }

            DateTime date = UnixTimeToDateTime(unixtime);

            User user = new User(telegramId, username, about, date);
            return user;
        }

        public void SetAbout(User user, string body)
        {
            var updateCmd = connection.CreateCommand();
            updateCmd.CommandText = $"UPDATE USER SET about = '{body}' WHERE telegram_id = '{user.TelegramId}'";
            updateCmd.ExecuteNonQuery();
        }

        private DateTime UnixTimeToDateTime(long unixtime)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixtime).ToLocalTime();
            return dtDateTime;
        }
    }
}
