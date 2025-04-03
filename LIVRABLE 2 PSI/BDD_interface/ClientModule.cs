using System;
using MySql.Data.MySqlClient;

/// <summary>
/// Module de gestion des clients corrigé pour éviter les doublons liés aux jointures.
/// </summary>
public static class ClientModule
{
    public static void Lancer()
    {
        Console.WriteLine("=== Module Client ===");
        Console.WriteLine("1. Lister tous les clients");
        Console.WriteLine("2. Ajouter un client");
        Console.WriteLine("3. Supprimer un client");
        Console.WriteLine("4. Trier par nom");
        Console.WriteLine("5. Trier par rue");
        Console.WriteLine("6. Trier par montant total d’achats");
        Console.Write("Choix : ");
        var choix = Console.ReadLine();

        string baseQuery = "SELECT u.id_utilisateur, u.nom, u.prenom, u.adresse, u.telephone, u.email " +
                           "FROM client c JOIN utilisateur u ON c.id_client = u.id_utilisateur";

        switch (choix)
        {
            case "1": Lister(baseQuery); break;
            case "2": AjouterClient(); break;
            case "3": SupprimerClient(); break;
            case "4": Lister(baseQuery + " ORDER BY u.nom"); break;
            case "5": Lister(baseQuery + " ORDER BY u.adresse"); break;
            case "6":
                Lister("SELECT u.nom, u.prenom, SUM(c.montant_total) AS total " +
                       "FROM utilisateur u JOIN client cl ON u.id_utilisateur = cl.id_client " +
                       "JOIN commande c ON c.id_client = cl.id_client " +
                       "GROUP BY cl.id_client ORDER BY total DESC");
                break;
        }
    }

    private static void Lister(string query)
    {
        using var conn = Database.GetConnection();
        conn.Open();
        var cmd = new MySqlCommand(query, conn);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            for (int i = 0; i < reader.FieldCount; i++)
                Console.Write($"{reader.GetName(i)}: {reader.GetValue(i)}\t");
            Console.WriteLine();
        }
    }

    private static void AjouterClient()
    {
        Console.Write("Nom: "); var nom = Console.ReadLine();
        Console.Write("Prénom: "); var prenom = Console.ReadLine();
        Console.Write("Adresse: "); var adresse = Console.ReadLine();
        Console.Write("Téléphone: "); var tel = Console.ReadLine();
        Console.Write("Email: "); var email = Console.ReadLine();
        string id = Guid.NewGuid().ToString().Substring(0, 8);

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

        var cmd2 = new MySqlCommand("INSERT INTO client VALUES (@id)", conn, tx);
        cmd2.Parameters.AddWithValue("@id", id);
        cmd2.ExecuteNonQuery();

        tx.Commit();
        Console.WriteLine("Client ajouté !");
    }

    private static void SupprimerClient()
    {
        Console.Write("ID client : ");
        var id = Console.ReadLine();
        using var conn = Database.GetConnection(); conn.Open();
        var cmd = new MySqlCommand("DELETE FROM utilisateur WHERE id_utilisateur = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
        Console.WriteLine("Client supprimé.");
    }
}