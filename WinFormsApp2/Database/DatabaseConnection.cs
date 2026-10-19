using MySql.Data.MySqlClient;

namespace QRAttendanceSystem.Database
{
    public static class DatabaseConnection
    {
        private static readonly string connectionString =
            "Server=localhost;" +
            "Database=attendance_system;" +
            "Uid=root;" +
            "Pwd=;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}