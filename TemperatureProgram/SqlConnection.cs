using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TemperatureProgram
{
    internal class SqlConnection
    {
        public static string ConnectionString;
        public const string MainTableName = "temperature";

        public static void Initialize()
        {
            SQLitePCL.Batteries.Init();
            ConnectionString = new SqliteConnectionStringBuilder("Data Source=ApplicationData.db")
            {
                Mode = SqliteOpenMode.ReadWriteCreate
            }.ToString();
        }

        public static void OpenConnection()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            new SqliteCommand("CREATE TABLE IF NOT EXISTS " + MainTableName + " (date DATETIME, temperature DOUBLE);", connection).ExecuteNonQuery();
        }
    }
}
