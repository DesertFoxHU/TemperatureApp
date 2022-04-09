using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
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

            new SqliteCommand("CREATE TABLE IF NOT EXISTS " + MainTableName + " (date DATETIME, temperature DOUBLE);", connection)
                .ExecuteNonQuery();
        }

        public static void AddElement(DateTime date, double temperature)
        {
            string temperatureString = string.Format("{0:0.00}", temperature).Replace(",", ".");
            string dateString = date.ToString("yyyy-MM-dd HH:m:ss");
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                new SqliteCommand("INSERT INTO " + MainTableName + " (date, temperature) VALUES ('" + dateString + "', " + temperatureString + ");", connection)
                    .ExecuteNonQuery();
            }
        }

        public static DayTemperature QueryDay(DateTime date)
        {
            DayTemperature dayTemperature = new DayTemperature(date);
            string datePattern = date.ToString("yyyy-MM-dd") + "%";

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                string command = "SELECT * FROM " + MainTableName + " WHERE date LIKE '" + datePattern + "';";
                SqliteDataReader reader = new SqliteCommand(command, connection).ExecuteReader();

                while (reader.Read())
                {
                    IDataRecord record = (IDataReader)reader;
                    try
                    {
                        string rawDate = record.GetString(0); //yyyy-MM-dd HH:m:ss
                        double temperature = record.GetDouble(1);

                        string rawHour = rawDate.Split(' ')[1].Split(':')[0];
                        int dateHour = int.Parse(rawHour);

                        dayTemperature.Temperature.Add(dateHour, temperature);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show($"Váratlan hiba adatfeldolgozás közben! Hibakód: {ex.StackTrace}");
                        break;
                    }
                }
                reader.Close();
            }
            return dayTemperature;
        }
    }
}
