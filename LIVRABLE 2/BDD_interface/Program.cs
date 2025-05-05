using LivInParis;
using MySql.Data.MySqlClient;
using System;

namespace LivinParis
{
    /// <summary>
    /// Affiche le menu principal et redirige vers les modules fonctionnels.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            MySqlConnection conn = null;
            try
            {
                string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=LivinParis;" +
                                         "UID=root;PASSWORD=root";
                conn = new MySqlConnection(connexionString);
                conn.Open();
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Erreur Connexion : " + e.ToString());
                return;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- Menu Principal - LivinParis ---");
                Console.WriteLine("1. Module Client");
                Console.WriteLine("2. Module Cuisinier");
                Console.WriteLine("3. Module Commande");
                Console.WriteLine("4. Module Statistiques");
                Console.WriteLine("5. Requêtes Simples");
                Console.WriteLine("5. Requêtes Complexes");
                Console.WriteLine("0. Quitter");
                Console.Write("Choix : ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ClientModule.Lancer();
                        break;
                    case "2":
                        CuisinierModule.Lancer();
                        break;
                    case "3":
                        CommandeModule.Lancer();
                        break;
                    case "4":
                        StatistiquesModule.Lancer();
                        break;
                    case "5":
                        MenuRequetesSimples(conn);
                        break;
                    case "6":
                        MenuRequetesComplexes(conn);
                        break;
                    case "0":
                        conn.Close();
                        return;
                    default:
                        Console.WriteLine("Choix invalide.");
                        break;
                }

                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Menu pour les requêtes SQL simples et complexes.
        /// </summary>
        static void MenuRequetesSimples(MySqlConnection conn)
        {
            Console.Clear();
            Console.WriteLine("--- Requêtes Simples ---");
            Console.WriteLine("1. Plats périmés");
            Console.WriteLine("2. Utilisateurs par ordre alphabétique");
            Console.WriteLine("3. Nombre de commandes par client");
            Console.WriteLine("0. Retour");

            Console.Write("Choix : ");
            var choix = Console.ReadLine();
            switch (choix)
            {
                case "1":
                    RequetesSimples.AfficherPlatsPerimes(conn);
                    break;
                case "2":
                    RequetesSimples.AfficherUtilisateursParNom(conn);
                    break;
                case "3":
                    RequetesSimples.AfficherNbCommandesParClient(conn);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Choix invalide.");
                    break;
            }
        }
        static void MenuRequetesComplexes(MySqlConnection conn)
        {
            Console.Clear();
            Console.WriteLine("--- Requêtes Complexes ---");
            Console.WriteLine("1. Plats plus chers que tous les desserts");
            Console.WriteLine("2. Clients avec dépenses > moyenne");
            Console.WriteLine("3. Cuisiniers (>3 livraisons)");
            Console.WriteLine("4. Plats périmant après livraisons (cuisinier U1)");
            Console.WriteLine("5. Clients avec dépenses > 50€ (après 2025-02-01)");
            Console.WriteLine("0. Retour");

            Console.Write("Choix : ");
            var choix = Console.ReadLine();
            switch (choix)
            {
                case "1": RequetesComplexes.AfficherPlatsPlusChersQueDesserts(); break;
                case "2": RequetesComplexes.AfficherClientsDepensesSuperieuresMoyenne(); break;
                case "3": RequetesComplexes.AfficherCuisiniersAvecPlusDeTroisLivraisons(); break;
                case "4": RequetesComplexes.AfficherPlatsPerimantApresLivraisonsCuisinier(); break;
                case "5": RequetesComplexes.AfficherClientsDepensesSuperieures50Euros(); break;
                case "0": return;
                default: Console.WriteLine("Choix invalide."); break;
            }
        }
    }
}
