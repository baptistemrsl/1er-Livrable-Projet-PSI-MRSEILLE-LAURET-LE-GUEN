using MySql.Data.MySqlClient;
using System;

namespace LivinParis
{
    /// <summary>
    /// Point d'entrée de l'application console LivinParis.
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
                Console.WriteLine("=== Menu Principal - LivinParis ===");
                Console.WriteLine("1. Module Client");
                Console.WriteLine("2. Module Cuisinier");
                Console.WriteLine("3. Module Commande");
                Console.WriteLine("4. Module Statistiques");
                Console.WriteLine("5. Requêtes Simples");
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
        /// Menu pour les requêtes SQL simples d'affichage.
        /// </summary>
        static void MenuRequetesSimples(MySqlConnection conn)
        {
            Console.Clear();
            Console.WriteLine("=== Requêtes Simples ===");
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
    }
}