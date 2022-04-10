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

            new SqliteCommand("CREATE TABLE IF NOT EXISTS " + MainTableName + " (date DATE, hour INT, temperature DOUBLE);", connection)
                .ExecuteNonQuery();
        }

        public static void AddOrUpdateElement(DateTime date, double temperature)
        {
            string temperatureString = string.Format("{0:0.0}", temperature).Replace(",", ".");
            string dateString = date.ToString("yyyy-MM-dd");

            string values = "('" + dateString + "', '" + date.Hour + "', '" + temperatureString + "')";
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                string command1 = "SELECT * FROM " + MainTableName + " WHERE date='" + dateString + "' AND hour=" + date.Hour + ";";
                SqliteDataReader reader = new SqliteCommand(command1, connection).ExecuteReader();
                if(!((IDataReader)reader).Read())
                {
                    new SqliteCommand("INSERT INTO " + MainTableName + " (date, hour, temperature) VALUES " + values + ";", connection)
                        .ExecuteNonQuery();
                    return;
                }

                string command2 = "UPDATE " + MainTableName + " SET temperature=" + temperature + " WHERE date = '" + dateString + "' AND hour = " + date.Hour + ";";
                new SqliteCommand(command2, connection).ExecuteNonQuery();
            }
        }

        public static DayTemperature QueryDay(DateTime date)
        {
            DayTemperature dayTemperature = new DayTemperature(date);
            string datePattern = date.ToString("yyyy-MM-dd");

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                string command = "SELECT * FROM " + MainTableName + " WHERE date='" + datePattern + "';";
                SqliteDataReader reader = new SqliteCommand(command, connection).ExecuteReader();

                while (reader.Read())
                {
                    IDataRecord record = (IDataReader)reader;
                    try
                    {
                        int hour = record.GetInt32(1);
                        double temperature = record.GetDouble(2);

                        dayTemperature.Temperature.Add(hour, temperature);
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

        public static void ClearDatabase()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                string command = "DELETE FROM " + MainTableName + ";";
                new SqliteCommand(command, connection).ExecuteNonQuery();
            }
        }
    }
}
