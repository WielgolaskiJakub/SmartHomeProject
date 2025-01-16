using System.Data.SQLite;
using System.Transactions;

class DataBaseInitializer
{
    public static void CreateDatabase()
    {
        Console.WriteLine($"Ścieżka do pliku bazy danych: {System.IO.Path.GetFullPath("SmartHomeDB.db")}");

        // Ścieżka do pliku bazy danych
        string dbPath = "Data Source=SmartHomeDB.db;Version=3;";

        // Połączenie z bazą danych
        using (var connection = new SQLiteConnection(dbPath))
        {
            connection.Open();
            Console.WriteLine("Połączono z bazą danych.");

            //  Odczytywanie danych z tabeli
            string query = "SELECT * FROM SmartHomeSettings;";
            using (var command = new SQLiteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Temperature: {reader["Temperature"]}, Lights: {reader["Lights"]}");

                    }
                }
            }
        }
    }

    public static void UpdateColumn(string columnName, object newValue)
    {

        string dbPath = "Data Source=SmartHomeDB.db;Version=3;";
        using (var connection = new SQLiteConnection(dbPath))
        {
            connection.Open();
            string updateQuery = $"UPDATE SmartHomeSettings SET {columnName} = @newValue WHERE ID = 1;";
            using (var command = new SQLiteCommand(updateQuery, connection))
            {

                command.Parameters.AddWithValue(columnName, newValue);
                command.Parameters.AddWithValue("@newValue", newValue);
                command.ExecuteNonQuery();
            }

        }

    }




    public static object GetColumnValue(string columnName)
    {
        string dbPath = "Data Source=SmartHomeDB.db;Version=3;";
        using (var connection = new SQLiteConnection(dbPath))
        {
            connection.Open();
            string query = $"SELECT {columnName} FROM SmartHomeSettings WHERE ID = 1;";
            using (var command = new SQLiteCommand(query, connection))
            {
                object result = command.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }
    }
}