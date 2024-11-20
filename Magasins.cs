using System;
using MySql.Data.MySqlClient;

namespace projet
{
    public class Magasin
    {
        public int Id { get; set; }
        public string Ville { get; set; }
        public string NomProprio { get; set; }
        public Magasin()
        {
        }
        public Magasin(int id,string ville, string proprio)
        {
            Id = id;
            Ville = ville;
            NomProprio = proprio;
        }
        public static void AfficherMagasin(MySqlConnection connection)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Magasin;";

            MySqlDataReader reader;
            reader = command.ExecuteReader();
            Console.WriteLine("Liste des magasins :");
            while (reader.Read())
            {
                int id=reader.GetInt32("idMagasin");   
                string ville = (string)reader["ville"];
                string proprio = (string)reader["nom_proprio"];
                Console.WriteLine(id+" : " + ville + ", "+proprio);
            }
            Console.WriteLine();
            reader.Close();
        }
        public static void AfficherInventaire(MySqlConnection connection,int magasin)
        {
            //inventaire produit
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT produit.prix,produit.nom_produit,produit.stock FROM fleuriste.produit WHERE idmagasin=" + magasin + ";";
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            int i = 1;
            List<Produit> produits = new List<Produit>();
            while (reader.Read())
            {
                Produit produit = new Produit();
                produit.NomProduit = (string)reader["nom_produit"];
                produit.Prix = (float)reader["prix"];
                produit.Stock = (int)reader["stock"];
                produits.Add(produit);
            }
            foreach (Produit produit in produits)
            {
                Console.Write(i+". "+produit.NomProduit + " : " + produit.Prix + " euros, ");
                if (produit.Stock == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("en rupture de stock");
                    Console.ResetColor();
                }
                else if (produit.Stock < 10)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(produit.Stock + " en stock");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(produit.Stock + " en stock");
                    Console.ResetColor();
                }
                i++;
            }
            Console.WriteLine();
            reader.Close();
            //inventaire bouquet
            command.CommandText = "SELECT nom_bouquet,prix_bouquet,quantite,composition,occasion FROM bouquet_standard WHERE idmagasin =" + magasin + ";";
            reader = command.ExecuteReader();
            List<Bouquet> bouquets = new List<Bouquet>();
            while (reader.Read())
            {
                Bouquet bouquet = new Bouquet();
                bouquet.NomBouquet = (string)reader["nom_bouquet"];
                bouquet.PrixBouquet = (double)reader["prix_bouquet"];
                bouquet.Quantite = (int)reader["quantite"];
                bouquet.Composition = (string)reader["composition"];
                bouquet.Occasion = (string)reader["occasion"];
                bouquets.Add(bouquet);
            }
            foreach (Bouquet bouquet in bouquets)
            {
                Console.Write(i+". "+bouquet.NomBouquet + " : " + bouquet.Composition + ", " + bouquet.Occasion + ", " + bouquet.PrixBouquet + " euros, ");
                if (bouquet.Quantite == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" en rupture de stock");
                    Console.ResetColor();
                }
                else if (bouquet.Quantite < 10)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(bouquet.Quantite + " en stock");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(bouquet.Quantite + " en stock");
                    Console.ResetColor();
                }
                i++;
            }
            reader.Close();
        }
        public void ListeMagasins(MySqlConnection connection)
        {
            Magasin monMagasin = new Magasin();
            Magasin.AfficherMagasin(connection);
            Console.Write("Informations du magasin N°");
            int mag = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("\n(1)Inventaire du magasin\n(2)Chiffre d'affaires\n(0)Retour");
            int choix = Convert.ToInt32(Console.ReadLine());
            while (choix > 0)
            {
                if (choix == 1)
                {
                    Console.Clear();
                    Program program=new Program();
                    Inventaire(connection, mag);
                    Console.Clear();
                }
                if (choix == 2)
                {
                    Console.Clear();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT magasin.ville, SUM(commande.prix) AS total_prix FROM magasin INNER JOIN commande ON magasin.idMagasin = commande.magasin WHERE magasin.idMagasin= " + mag + ";";
                    MySqlDataReader reader = command.ExecuteReader();
                    string ville;
                    double totalprix = 0;
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(reader.GetOrdinal("ville")))
                        {
                            Console.WriteLine("Aucune commande n'a été passé.");
                        }
                        else
                        {
                            ville = (string)reader["ville"];
                            totalprix = (double)reader["total_prix"];
                            Console.Write("Chiffre d'affaires du magasin de " + ville + " : " + totalprix + " euros");
                        }
                        Console.ReadKey();
                        Console.Clear();
                    }
                    reader.Close();
                }
                Console.WriteLine("(1)Inventaire du magasin\n(2)Chiffre d'affaires\n(0)Retour");
                choix = Convert.ToInt32(Console.ReadLine());
            }
            Console.Clear();

        }
        public void Inventaire(MySqlConnection connection, int choix_magasin)
        {
            Magasin.AfficherInventaire(connection, choix_magasin);
            Console.WriteLine("\n(1)Modifier l'inventaire \n(0)Retour");
            int choix2 = Convert.ToInt32(Console.ReadLine());
            switch (choix2)
            {
                case 1:
                    Console.Clear();
                    Magasin.AfficherInventaire(connection, choix_magasin);
                    //Modifier inventaire
                    Console.Write("\nNuméro du produit à ajouter (0.Retour): ");
                    int nb_produit = Convert.ToInt32(Console.ReadLine());
                    while (nb_produit != 0)
                    {
                        float nb = 0;
                        Produit produit = new Produit();
                        Bouquet bouquet = new Bouquet();
                        switch (nb_produit)
                        {
                            case 0:
                                Console.Clear();
                                break;
                            case 1:
                                produit.NomProduit = "Boîte";
                                Console.Write("Nombre de boîtes ajoutées : ");
                                nb = (float)Convert.ToInt32(Console.ReadLine());
                                Produit.MettreAJourQuantite(connection, produit, choix_magasin, nb);
                                break;
                            case 2:
                                produit.NomProduit = "Vase";
                                Console.Write("Nombre de vases ajoutés : ");
                                nb = (float)Convert.ToInt32(Console.ReadLine());
                                Produit.MettreAJourQuantite(connection, produit, choix_magasin, nb);
                                break;
                            case 3:
                                produit.NomProduit = "Ruban";
                                Console.Write("Nombre de rubans ajoutés : ");
                                nb = (float)Convert.ToInt32(Console.ReadLine());
                                Produit.MettreAJourQuantite(connection, produit, choix_magasin, nb);
                                break;
                            case 4:
                                bouquet.NomBouquet = "Gros Merci";
                                Console.Write("Nombre de bouquets Gros Merci ajoutés : ");
                                nb = (float)Convert.ToInt32(Console.ReadLine());
                                Bouquet.MettreAJourQuantite(connection, bouquet, choix_magasin, nb);
                                break;
                            case 5:
                                bouquet.NomBouquet = "L'amoureux";
                                Console.Write("Nombre de bouquets L'amoureux ajoutés : ");
                                nb = (float)Convert.ToInt32(Console.ReadLine());
                                Bouquet.MettreAJourQuantite(connection, bouquet, choix_magasin, nb);
                                break;
                            case 6:
                                bouquet.NomBouquet = "L'Exotique";
                                Console.Write("Nombre de bouquets L'Exotique ajoutés : ");
                                nb = (float)Convert.ToInt32(Console.ReadLine());
                                Bouquet.MettreAJourQuantite(connection, bouquet, choix_magasin, nb);
                                break;
                            case 7:
                                bouquet.NomBouquet = "Maman";
                                Console.Write("Nombre de bouquets Maman ajoutés : ");
                                nb = (float)Convert.ToInt32(Console.ReadLine());
                                Bouquet.MettreAJourQuantite(connection, bouquet, choix_magasin, nb);
                                break;
                            case 8:
                                bouquet.NomBouquet = "Vive la mariée";
                                Console.Write("Nombre de bouquets Vive la mariée ajoutés : ");
                                nb = (float)Convert.ToInt32(Console.ReadLine());
                                Bouquet.MettreAJourQuantite(connection, bouquet, choix_magasin, nb);
                                break;
                        }
                        Console.Clear();
                        Magasin.AfficherInventaire(connection, choix_magasin);
                        Console.Write("\nNuméro du produit à ajouter (0.Retour): ");
                        nb_produit = Convert.ToInt32(Console.ReadLine());
                    }
                    break;
            }
            Console.Clear();
        }
    }
}
