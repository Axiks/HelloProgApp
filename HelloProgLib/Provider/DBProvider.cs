using System;
using Microsoft.Data.Sqlite;

// Занесення інформації "про себе" і технології які використовує

namespace HelloProgLib
{
    public class DBProvider
    {
        private static SqliteConnectionStringBuilder sqliteConnectionStringBuilder;
        private DBProvider()
        {}

        private static SqliteConnectionStringBuilder GetBuilder()
        {
            if(sqliteConnectionStringBuilder == null)
            {
                sqliteConnectionStringBuilder = new SqliteConnectionStringBuilder();
                sqliteConnectionStringBuilder.DataSource = "helloProg.db";
                // DropTable();
                InitTable();
            }
            return sqliteConnectionStringBuilder;
        }

        public static SqliteConnection GetConnection()
        {
            SqliteConnection connection = new SqliteConnection(GetBuilder().ConnectionString);
            connection.Open();
            return connection;
        }

        private static void InitTable()
        {
            using (var connection = new SqliteConnection(sqliteConnectionStringBuilder.ConnectionString))
            {
                connection.Open();

                //Create Table User, Habbit
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "CREATE TABLE IF NOT EXISTS USER(telegram_id TEXT NOT NULL, username TEXT, about TEXT, accession_time INTEGER NOT NULL, UNIQUE(telegram_id))";
                tableCmd.ExecuteNonQuery();

                tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "CREATE TABLE IF NOT EXISTS HABBIT(id INTEGER PRIMARY KEY, title TEXT NOT NULL, description TEXT, UNIQUE(title))";
                tableCmd.ExecuteNonQuery();

                tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "CREATE TABLE IF NOT EXISTS HABIT_USER(id INTEGER PRIMARY KEY, user_id INTEGER NOT NULL, habit_id INTEGER  NOT NULL)";
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        private static void DropTable()
        {
            using (var connection = new SqliteConnection(sqliteConnectionStringBuilder.ConnectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"DROP TABLE IF EXISTS USER";
                tableCmd.ExecuteNonQuery();

                tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"DROP TABLE IF EXISTS HABBIT";
                tableCmd.ExecuteNonQuery();

                tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"DROP TABLE IF EXISTS HABIT_USER";
                tableCmd.ExecuteNonQuery();

                connection.Close();

            }
        }
    }
}
