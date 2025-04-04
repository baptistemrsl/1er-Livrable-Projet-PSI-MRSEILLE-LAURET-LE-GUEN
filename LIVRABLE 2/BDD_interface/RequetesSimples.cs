using MySql.Data.MySqlClient;
using System;

/// <summary>
/// Requêtes simples utilitaires (non modifiantes).
/// </summary>
public static class RequetesSimples
{
    public static void AfficherPlatsPerimes(MySqlConnection conn)
    {
        Console.WriteLine("=== Plats périmés ===");
        string query = "SELECT * FROM plat WHERE date_peremption < CURDATE();";
        Executer(conn, query);
    }

    public static void AfficherUtilisateursParNom(MySqlConnection conn)
    {
        Console.WriteLine("=== Utilisateurs par ordre alphabétique ===");
        string query = "SELECT * FROM utilisateur ORDER BY nom;";
        Executer(conn, query);
    }

    public static void AfficherNbCommandesParClient(MySqlConnection conn)
    {
        Console.WriteLine("=== Nombre de commandes par client ===");
        string query = "SELECT id_client, COUNT(*) AS nb_commandes FROM commande GROUP BY id_client;";
        Executer(conn, query);
    }

    private static void Executer(MySqlConnection conn, string query)
    {
        try
        {
            var cmd = new MySqlCommand(query, conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                    Console.Write($"{reader.GetName(i)}: {reader.GetValue(i)}\t");
                Console.WriteLine();
            }
            reader.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Erreur : " + e.Message);
        }
    }
}