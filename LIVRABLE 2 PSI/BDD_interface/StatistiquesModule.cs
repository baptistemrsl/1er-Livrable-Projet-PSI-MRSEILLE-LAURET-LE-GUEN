using System;
using MySql.Data.MySqlClient;

/// <summary>
/// Module statistique pour les bilans et indicateurs globaux.
/// </summary>
public static class StatistiquesModule
{
    public static void Lancer()
    {
        Console.WriteLine("=== Module Statistiques ===");
        Console.WriteLine("1. Nombre de livraisons par cuisinier");
        Console.WriteLine("2. Commandes entre deux dates");
        Console.WriteLine("3. Moyenne des prix des commandes");
        Console.WriteLine("4. Moyenne des dépenses par client");
        Console.WriteLine("5. Commandes par nationalité et période");
        Console.Write("Choix : ");
        var choix = Console.ReadLine();

        switch (choix)
        {
            case "1": LivraisonsParCuisinier(); break;
            case "2": CommandesParPeriode(); break;
            case "3": MoyennePrix(); break;
            case "4": MoyenneClients(); break;
            case "5": CommandesParNationalite(); break;
        }
    }

    private static void LivraisonsParCuisinier()
    {
        using var conn = Database.GetConnection(); conn.Open();
        var cmd = new MySqlCommand(
            "SELECT p.id_cuisinier, COUNT(*) as livraisons FROM plat p " +
            "JOIN ligne_commande l ON p.id_plat = l.id_plat GROUP BY p.id_cuisinier", conn);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
            Console.WriteLine($"Cuisinier {reader["id_cuisinier"]} - {reader["livraisons"]} livraisons");
    }

    private static void CommandesParPeriode()
    {
        Console.Write("Date début (YYYY-MM-DD) : ");
        var d1 = Console.ReadLine();
        Console.Write("Date fin (YYYY-MM-DD) : ");
        var d2 = Console.ReadLine();
        using var conn = Database.GetConnection(); conn.Open();
        var cmd = new MySqlCommand("SELECT * FROM commande WHERE date_commande BETWEEN @d1 AND @d2", conn);
        cmd.Parameters.AddWithValue("@d1", d1);
        cmd.Parameters.AddWithValue("@d2", d2);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
            Console.WriteLine($"{reader["id_commande"]} - {reader["date_commande"]} - {reader["montant_total"]} €");
    }

    private static void MoyennePrix()
    {
        using var conn = Database.GetConnection(); conn.Open();
        var cmd = new MySqlCommand("SELECT AVG(montant_total) FROM commande", conn);
        var result = cmd.ExecuteScalar();
        Console.WriteLine($"Moyenne des prix : {result} €");
    }

    private static void MoyenneClients()
    {
        using var conn = Database.GetConnection(); conn.Open();
        var cmd = new MySqlCommand(
            "SELECT AVG(total) FROM (SELECT SUM(montant_total) AS total FROM commande GROUP BY id_client) t", conn);
        var result = cmd.ExecuteScalar();
        Console.WriteLine($"Dépense moyenne par client : {result} €");
    }

    private static void CommandesParNationalite()
    {
        Console.Write("ID client : ");
        var client = Console.ReadLine();
        Console.Write("Date début (YYYY-MM-DD) : ");
        var d1 = Console.ReadLine();
        Console.Write("Date fin (YYYY-MM-DD) : ");
        var d2 = Console.ReadLine();
        using var conn = Database.GetConnection(); conn.Open();
        var cmd = new MySqlCommand(
            "SELECT p.nationalite, COUNT(*) as nb FROM commande c " +
            "JOIN ligne_commande l ON c.id_commande = l.id_commande " +
            "JOIN plat p ON l.id_plat = p.id_plat " +
            "WHERE c.id_client = @client AND c.date_commande BETWEEN @d1 AND @d2 GROUP BY p.nationalite", conn);
        cmd.Parameters.AddWithValue("@client", client);
        cmd.Parameters.AddWithValue("@d1", d1);
        cmd.Parameters.AddWithValue("@d2", d2);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
            Console.WriteLine($"{reader["nationalite"]}: {reader["nb"]} commande(s)");
    }
}