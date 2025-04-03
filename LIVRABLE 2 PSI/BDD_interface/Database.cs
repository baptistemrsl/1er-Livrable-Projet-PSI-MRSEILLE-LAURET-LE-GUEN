using MySql.Data.MySqlClient;

/// <summary>
/// Fournit une méthode de connexion centralisée à la base de données MySQL.
/// </summary>
public static class Database
{
    private static string connectionString = "Server=localhost;PORT=3306;Database=LivinParis;Uid=root;Pwd=root;";

    public static MySqlConnection GetConnection()
    {
        return new MySqlConnection(connectionString);
    }
}