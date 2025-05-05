using MySql.Data.MySqlClient;
using System;

namespace LivInParis
{
    public static class RequetesComplexes
    {
        public static void AfficherPlatsPlusChersQueDesserts()
        {
            Console.WriteLine("\n--- Plats plus chers que tous les desserts ---");
            string query = @"
                SELECT p.nom, p.prix, p.type
                FROM plat p
                WHERE p.prix > ALL (
                    SELECT prix 
                    FROM plat 
                    WHERE type = 'Dessert'
                );";

            ExecuterRequete(query);
        }

        public static void AfficherClientsDepensesSuperieuresMoyenne()
        {
            Console.WriteLine("\n--- Clients avec dépenses > moyenne ---");
            string query = @"
                SELECT u.nom, u.prenom, c.montant_total
                FROM commande c
                JOIN utilisateur u ON c.id_client = u.id_utilisateur
                WHERE c.montant_total > ANY (
                    SELECT AVG(montant_total) 
                    FROM commande
                );";

            ExecuterRequete(query);
        }

        public static void AfficherCuisiniersAvecPlusDeTroisLivraisons()
        {
            Console.WriteLine("\n--- Cuisiniers (>3 livraisons) ---");
            string query = @"
                SELECT c.id_cuisinier, u.nom, u.prenom, COUNT(l.id_livraison) AS nb_livraisons
                FROM cuisinier c
                JOIN utilisateur u ON c.id_cuisinier = u.id_utilisateur
                JOIN livraison l ON c.id_cuisinier = l.id_cuisinier
                GROUP BY c.id_cuisinier, u.nom, u.prenom
                HAVING COUNT(l.id_livraison) > 3
                ORDER BY nb_livraisons DESC;";

            ExecuterRequete(query);
        }

        public static void AfficherPlatsPerimantApresLivraisonsCuisinier(string idCuisinier = "U1")
        {
            Console.WriteLine($"\n--- Plats périmant après toutes les livraisons du cuisinier {idCuisinier} ---");
            string query = $@"
                SELECT p.nom, p.date_peremption
                FROM plat p
                WHERE p.date_peremption > ALL (
                    SELECT date_livraison 
                    FROM livraison 
                    WHERE id_cuisinier = '{idCuisinier}'
                );";

            ExecuterRequete(query);
        }

        public static void AfficherClientsDepensesSuperieures50Euros(string dateMin = "2025-02-01")
        {
            Console.WriteLine($"\n--- Clients avec dépenses > 50€ (après {dateMin}) ---");
            string query = $@"
                SELECT c.id_client, u.nom, u.prenom, SUM(c.montant_total) AS total_depense
                FROM commande c
                JOIN utilisateur u ON c.id_client = u.id_utilisateur
                WHERE c.date_commande > '{dateMin}'
                GROUP BY c.id_client, u.nom, u.prenom
                HAVING SUM(c.montant_total) > 50
                ORDER BY total_depense DESC;";

            ExecuterRequete(query);
        }

        private static void ExecuterRequete(string query)
        {
            using var conn = Database.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(query, conn);
            var reader = cmd.ExecuteReader();
            
            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write($"{reader.GetName(i)}\t");
            }
            Console.WriteLine("\n" + new string('-', 50));

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                    Console.Write($"{reader.GetValue(i)}\t");
                Console.WriteLine();
            }
            reader.Close();
        }
    }
}