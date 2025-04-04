using System;
using MySql.Data.MySqlClient;

/// <summary>
/// Module de gestion des commandes : création, affichage, calcul de prix.
/// </summary>
public static class CommandeModule
{
    public static void Lancer()
    {
        Console.WriteLine("=== Module Commande ===");
        Console.WriteLine("1. Créer une commande");
        Console.WriteLine("2. Afficher les commandes d’un client");
        Console.WriteLine("3. Calculer le prix d’une commande");
        Console.Write("Choix : ");
        var choix = Console.ReadLine();

        switch (choix)
        {
            case "1": CreerCommande(); break;
            case "2": AfficherCommandesClient(); break;
            case "3": PrixCommande(); break;
        }
    }

    private static void CreerCommande()
    {
        Console.Write("ID client : ");
        var idClient = Console.ReadLine();
        var idCommande = Guid.NewGuid().ToString().Substring(0, 8);
        decimal total = 0;

        using var conn = Database.GetConnection(); conn.Open();
        var tx = conn.BeginTransaction();

        var cmd = new MySqlCommand("INSERT INTO commande (id_commande, date_commande, montant_total, id_client) VALUES (@id, CURDATE(), 0, @client)", conn, tx);
        cmd.Parameters.AddWithValue("@id", idCommande);
        cmd.Parameters.AddWithValue("@client", idClient);
        cmd.ExecuteNonQuery();

        while (true)
        {
            Console.Write("ID plat (vide pour terminer) : ");
            var idPlat = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(idPlat)) break;

            Console.Write("Quantité : ");
            int qte = int.Parse(Console.ReadLine());
            Console.Write("Date livraison (YYYY-MM-DD) : ");
            var date = Console.ReadLine();
            Console.Write("Lieu livraison : ");
            var lieu = Console.ReadLine();

            var cmdPlat = new MySqlCommand("SELECT prix FROM plat WHERE id_plat = @id", conn, tx);
            cmdPlat.Parameters.AddWithValue("@id", idPlat);
            decimal prix = Convert.ToDecimal(cmdPlat.ExecuteScalar());

            total += prix * qte;

            var idLigne = Guid.NewGuid().ToString().Substring(0, 8);
            var cmdLigne = new MySqlCommand(
                "INSERT INTO ligne_commande (id_commande_ligne, id_commande, id_plat, quantite, date_livraison, lieu_livraison) " +
                "VALUES (@id, @commande, @plat, @qte, @date, @lieu)", conn, tx);
            cmdLigne.Parameters.AddWithValue("@id", idLigne);
            cmdLigne.Parameters.AddWithValue("@commande", idCommande);
            cmdLigne.Parameters.AddWithValue("@plat", idPlat);
            cmdLigne.Parameters.AddWithValue("@qte", qte);
            cmdLigne.Parameters.AddWithValue("@date", date);
            cmdLigne.Parameters.AddWithValue("@lieu", lieu);
            cmdLigne.ExecuteNonQuery();
        }

        var updateTotal = new MySqlCommand("UPDATE commande SET montant_total = @total WHERE id_commande = @id", conn, tx);
        updateTotal.Parameters.AddWithValue("@total", total);
        updateTotal.Parameters.AddWithValue("@id", idCommande);
        updateTotal.ExecuteNonQuery();

        tx.Commit();
        Console.WriteLine("Commande créée avec succès.");
    }

    private static void AfficherCommandesClient()
    {
        Console.Write("ID client : ");
        var id = Console.ReadLine();
        using var conn = Database.GetConnection(); conn.Open();
        var cmd = new MySqlCommand("SELECT * FROM commande WHERE id_client = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
            Console.WriteLine($"{reader["id_commande"]} - {reader["date_commande"]} - {reader["montant_total"]} €");
    }

    private static void PrixCommande()
    {
        Console.Write("ID commande : ");
        var id = Console.ReadLine();
        using var conn = Database.GetConnection(); conn.Open();
        var cmd = new MySqlCommand("SELECT montant_total FROM commande WHERE id_commande = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        var prix = cmd.ExecuteScalar();
        Console.WriteLine($"Montant total de la commande : {prix} €");
    }
}