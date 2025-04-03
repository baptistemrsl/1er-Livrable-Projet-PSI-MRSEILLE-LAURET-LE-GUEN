using System;
using MySql.Data.MySqlClient;

/// <summary>
/// Module cuisinier : gestion des cuisiniers et suivi des plats livrés.
/// </summary>
public static class CuisinierModule
{
    public static void Lancer()
    {
        Console.WriteLine("=== Module Cuisinier ===");
        Console.WriteLine("1. Lister les cuisiniers");
        Console.WriteLine("2. Ajouter un cuisinier");
        Console.WriteLine("3. Supprimer un cuisinier");
        Console.WriteLine("4. Clients servis depuis une date");
        Console.WriteLine("5. Plats réalisés par fréquence");
        Console.WriteLine("6. Plat du jour");
        Console.Write("Choix : ");
        var choix = Console.ReadLine();

        switch (choix)
        {
            case "1": Lister(); break;
            case "2": Ajouter(); break;
            case "3": Supprimer(); break;
            case "4": ClientsServis(); break;
            case "5": PlatsParFrequence(); break;
            case "6": PlatDuJour(); break;
        }
    }

    private static void Lister()
    {
        using var conn = Database.GetConnection();
        conn.Open();
        var cmd = new MySqlCommand("SELECT * FROM cuisinier NATURAL JOIN utilisateur", conn);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
            Console.WriteLine($"{reader["id_utilisateur"]} - {reader["nom"]} {reader["prenom"]}");
    }

    private static void Ajouter()
    {
        Console.Write("Nom: "); var nom = Console.ReadLine();
        Console.Write("Prénom: "); var prenom = Console.ReadLine();
        Console.Write("Adresse: "); var adresse = Console.ReadLine();
        Console.Write("Téléphone: "); var tel = Console.ReadLine();
        Console.Write("Email: "); var email = Console.ReadLine();
        var id = Guid.NewGuid().ToString().Substring(0, 8);

        using var conn = Database.GetConnection(); conn.Open();
        var tx = conn.BeginTransaction();

        var cmd1 = new MySqlCommand("INSERT INTO utilisateur VALUES (@id, @nom, @prenom, @adresse, @tel, @mail, 'mdp')", conn, tx);
        cmd1.Parameters.AddWithValue("@id", id);
        cmd1.Parameters.AddWithValue("@nom", nom);
        cmd1.Parameters.AddWithValue("@prenom", prenom);
        cmd1.Parameters.AddWithValue("@adresse", adresse);
        cmd1.Parameters.AddWithValue("@tel", tel);
        cmd1.Parameters.AddWithValue("@mail", email);
        cmd1.ExecuteNonQuery();

        var cmd2 = new MySqlCommand("INSERT INTO cuisinier VALUES (@id)", conn, tx);
        cmd2.Parameters.AddWithValue("@id", id);
        cmd2.ExecuteNonQuery();

        tx.Commit();
        Console.WriteLine("Cuisinier ajouté.");
    }

    private static void Supprimer()
    {
        Console.Write("ID cuisinier : ");
        var id = Console.ReadLine();
        using var conn = Database.GetConnection(); conn.Open();
        var cmd = new MySqlCommand("DELETE FROM utilisateur WHERE id_utilisateur = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
        Console.WriteLine("Cuisinier supprimé.");
    }

    private static void ClientsServis()
    {
        Console.Write("ID cuisinier : ");
        var id = Console.ReadLine();
        Console.Write("Date depuis (YYYY-MM-DD) : ");
        var date = Console.ReadLine();
        using var conn = Database.GetConnection(); conn.Open();
        var cmd = new MySqlCommand(
            "SELECT DISTINCT u.nom, u.prenom FROM commande c " +
            "JOIN ligne_commande l ON c.id_commande = l.id_commande " +
            "JOIN plat p ON l.id_plat = p.id_plat " +
            "JOIN utilisateur u ON c.id_client = u.id_utilisateur " +
            "WHERE p.id_cuisinier = @id AND c.date_commande >= @date", conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@date", date);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
            Console.WriteLine($"{reader["nom"]} {reader["prenom"]}");
    }

    private static void PlatsParFrequence()
    {
        Console.Write("ID cuisinier : ");
        var id = Console.ReadLine();
        using var conn = Database.GetConnection(); conn.Open();
        var cmd = new MySqlCommand(
            "SELECT nom, COUNT(*) as freq FROM plat " +
            "WHERE id_cuisinier = @id GROUP BY nom ORDER BY freq DESC", conn);
        cmd.Parameters.AddWithValue("@id", id);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
            Console.WriteLine($"{reader["nom"]} - {reader["freq"]} fois");
    }

    private static void PlatDuJour()
    {
        using var conn = Database.GetConnection(); conn.Open();
        var cmd = new MySqlCommand(
            "SELECT nom, type FROM plat WHERE date_fabrication = CURDATE()", conn);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
            Console.WriteLine($"{reader["type"]}: {reader["nom"]}");
    }
}