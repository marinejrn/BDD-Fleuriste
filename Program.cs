using MySql.Data.MySqlClient;

namespace projet
{
    public class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=fleuriste;UID=root;PASSWORD=bateau11;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Votre Magasin BelleFleur\n");
            Console.ResetColor();
            Console.WriteLine("(1) Espace Vendeur \n(2) Espace Client");
            int reponse = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            connection.Open();
            while (reponse != 0)
            {
                switch (reponse)
                {
                    case 1:
                        EspaceVendeur(connection);
                        break;
                    case 2:
                        EspaceClient(connection);
                        break;
                }
                Console.WriteLine("(1) Espace Vendeur \n(2) Espace Client \n\n(0)Quitter");
                reponse = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
            }
            Console.WriteLine("\nA bientôt !");
            Console.ReadKey();
            connection.Close();
        }
        const string quote = "\"";

        static void EspaceVendeur(MySqlConnection connection)
        {
            Console.WriteLine("Mot de passe : ");
            string rep = Convert.ToString(Console.ReadLine());
            Console.Clear();
            if (rep == "motdepasse")
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Bienvenue dans l'espace vendeur de BelleFleur !\n");
                Console.ResetColor();

                //Verification des stocks des magasins
                MySqlCommand verification = connection.CreateCommand();
                verification.CommandText = "SELECT idmagasin,nom_produit FROM produit WHERE stock=0 UNION SELECT idmagasin,nom_bouquet FROM bouquet_standard WHERE quantite=0;";
                MySqlDataReader reader=verification.ExecuteReader();
                int magasin = 0;
                string produit = "";
                while (reader.Read())
                {                   
                    magasin = (int)reader["idmagasin"];
                    produit = (string)reader["nom_produit"];
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ATTENTION \nIl n'y a plus de "+produit+" dans le magasin N°"+magasin+"\n");
                    Console.ResetColor();
                }
                reader.Close();
                Console.WriteLine("(MENU)" + "\n\n1.Liste clients" + "\n2.Historique des commandes" + "\n3.Commande en cours" + "\n4.Liste des magasins" + "\n5.Statistiques\n\n0.Retour Menu Principal");
                int repo = Convert.ToInt32(Console.ReadLine());
                Commande commande = new Commande();
                Console.Clear();
                while (repo != 0)
                {
                    switch (repo)
                    {
                        case 1:
                            Clients clients = new Clients();
                            clients.AfficherClients();
                            Console.ReadKey();
                            Console.Clear();
                            connection.Close();
                            break;
                        case 2:
                            Magasin.AfficherMagasin(connection);
                            Console.Write("Historique du magasin N°");
                            magasin=Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("\n");
                            Console.Clear();                            
                            commande.Historique(connection,magasin);
                            Console.ReadKey();
                            Console.Clear();
                            connection.Close();
                            break;
                        case 3:
                            Magasin.AfficherMagasin(connection);
                            Console.Write("Commande en cours du magasin N°");
                            magasin = Convert.ToInt32(Console.ReadLine());
                            commande.CommandeEnCours(connection,magasin);
                            connection.Close();
                            break;
                        case 4:
                            Magasin mag=new Magasin();
                            mag.ListeMagasins(connection);
                            connection.Close();
                            break;
                        case 5:
                            Statistiques(connection);
                            connection.Close();
                            break;
                    }
                    connection.Open();
                    //Verification des stocks des magasins
                    verification = connection.CreateCommand();
                    verification.CommandText = "SELECT idmagasin,nom_produit FROM produit WHERE stock=0 UNION SELECT idmagasin,nom_bouquet FROM bouquet_standard WHERE quantite=0;";
                    reader = verification.ExecuteReader();
                    magasin = 0;
                     produit = "";
                    while (reader.Read())
                    {
                        magasin = (int)reader["idmagasin"];
                        produit = (string)reader["nom_produit"];
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("ATTENTION \nIl n'y a plus de " + produit + " dans le magasin N°" + magasin + "\n");
                        Console.ResetColor();
                    }
                    reader.Close();
                    Console.WriteLine("(Menu)" + "\n\n1.Liste clients" + "\n2.Historique des commandes" + "\n3.Commande en cours" + "\n4.Liste des magasins"+ "\n5.Statistiques" + "\n\n0.Retour Menu Principal");
                    repo = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();
                }
            }
            else
            {
                Console.WriteLine("Mot de passe incorrect.\n");
            }
        }
        static void EspaceClient(MySqlConnection connection)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Bienvenue dans l'espace client de BelleFleur !");
            Console.ResetColor();
            
            Console.WriteLine("\n(MENU)" + "\n\n1.Créer un compte" + "\n2.Se connecter" + "\n\n0.Retour Menu Principal");
            int repon = Convert.ToInt32(Console.ReadLine());
            Clients client = new Clients();
            Console.Clear();
            while (repon != 0)
            {
                switch (repon)
                {
                    case 1:
                        client.NouveauClient(connection);
                        connection.Close();
                        break;
                    case 2:
                        client.SeConnecter(connection);
                        connection.Close();
                        break;

                }
                connection.Open();
                Console.WriteLine("(MENU)" + "\n\n1.Créer un compte" + "\n2.Se connecter" + "\n\n0.Retour Menu principal");
                repon = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
            }
        }        
        static void Statistiques(MySqlConnection connection)
        {
            MySqlCommand command= connection.CreateCommand(); ;
            MySqlDataReader reader;
            Console.WriteLine("1. Meilleur client de l'année" + "\n" + "2. Magasin le plus rentable" + "\n" + "3. Bouquet le plus acheté" + "\n" + "4. Prix moyen d'un bouquet" + "\n" + "5. Fleur exotique la moins vendue\n6. Statistiques fleurs\n0. Retour");
            int reponse = Convert.ToInt32(Console.ReadLine());
            while(reponse > 0)
            {
                switch (reponse)
                {
                    case 1:
                        Console.Clear();
                        command.CommandText = "SELECT nom, prenom, COUNT(*) AS nb_commandes FROM clients JOIN commande ON clients.id_client = commande.id_client GROUP BY clients.id_client ORDER BY nb_commandes DESC;";
                        reader = command.ExecuteReader();
                        Int64 nb = 0;
                        string nom = "";
                        string prenom = "";
                        if (reader.Read())
                        {
                            nom = (string)reader["nom"];
                            prenom = (string)reader["prenom"];
                            nb = (Int64)reader["nb_commandes"];
                            Console.Write("\nLe client de l'année est " + prenom + " " + nom + " avec " + nb + " commandes.");
                            Console.ReadKey();
                            Console.Clear();
                            reader.Close();
                            break;
                        }
                        reader.Close();
                        break;
                    case 2:
                        //Afficher le magasin le plus rentable de l'année
                        Console.Clear();
                        command.CommandText = "SELECT magasin.ville, SUM(commande.prix) AS total_prix FROM magasin INNER JOIN commande ON magasin.idMagasin = commande.magasin GROUP BY magasin.ville ORDER BY total_prix DESC";
                        reader = command.ExecuteReader();
                        string ville="";
                        double totalprix = 0;
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(reader.GetOrdinal("ville")))
                            {
                                ville= "\nAucune commande n'a été passé.";
                            }
                            else
                            {
                                ville = (string)reader["ville"];
                                totalprix = (double)reader["total_prix"];
                                Console.Write("\nLe magasin le plus rentable est celui de " + ville + " avec " + totalprix + " euros de chiffre d'affaires.");
                            }
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        }
                        reader.Close();
                        break;

                    case 3:
                        //Afficher le bouquet le plus acheté de l'année
                        Console.Clear();
                        command.CommandText = "SELECT commande.idbouquet, COUNT(commande.idbouquet) as nombre_achats FROM commande JOIN bouquet_standard ON commande.idbouquet = bouquet_standard.idbouquet GROUP BY commande.idbouquet ORDER BY nombre_achats DESC LIMIT 1; ";
                        reader = command.ExecuteReader();
                        string nombouquet;
                        int idbouquet = 0;
                        Int64 nombreachat=0;
                        while (reader.Read())
                        {
                            idbouquet = (int)reader["idbouquet"];
                            nombreachat = (Int64)reader["nombre_achats"];
                        }
                        reader.Close();
                        command.CommandText = "SELECT nom_bouquet FROM bouquet_standard WHERE idbouquet ="+idbouquet+";";
                        nombouquet = (string)command.ExecuteScalar();
                        Console.Write("\nLe bouquet le plus acheté est " + nombouquet + " avec " + nombreachat + " bouquets vendus.");
                        Console.ReadKey();
                        Console.Clear();
                        break;

                    case 4:
                        //Afficher le prix moyen d'un bouquet
                        Console.Clear();
                        command.CommandText = "SELECT avg (prix) AS prix_moyen FROM commande;";
                        double prix_moyen = (double)command.ExecuteScalar();
                        Console.WriteLine("\nLe prix moyen d'un bouquet est de " + prix_moyen+" euros");
                        Console.ReadKey();
                        Console.Clear();
                        break;

                    case 5:
                        Console.Clear();
                        //Afficher la fleur la moins achetée
                        command.CommandText = "SELECT nom_fleurs,nb_achat FROM fleurs ORDER BY nb_achat LIMIT 1 ;";
                        reader = command.ExecuteReader();
                        int nb_achat = 0;
                        string fleur_nulle = "";
                        while (reader.Read())
                        {
                            nb_achat = (int)reader["nb_achat"];
                            fleur_nulle = (string)reader["nom_fleurs"];
                        }
                        reader.Close();
                        Console.WriteLine("\nLa fleur la moins achetée est " + fleur_nulle+" avec "+nb_achat+" fleurs vendues");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 6:
                        Console.Clear();
                        //Afficher la fleur la plus achetée
                        command.CommandText = "SELECT nom_fleurs,nb_achat FROM fleurs ORDER BY nb_achat DESC;";
                        reader = command.ExecuteReader();
                        int nb_acheté = 0;
                        string nom_fleur = "";
                        while (reader.Read())
                        {
                            nb_acheté = (int)reader["nb_achat"];
                            nom_fleur = (string)reader["nom_fleurs"];
                            Console.WriteLine("\n"+nom_fleur + " : " + nb_acheté + " fleurs vendues");
                        }
                        reader.Close();
                        Console.ReadKey();
                        Console.Clear();
                        break;

                }
                Console.WriteLine("1. Meilleur client de l'année" + "\n" + "2. Magasin le plus rentable" + "\n" + "3. Bouquet le plus acheté" + "\n" + "4. Prix moyen d'un bouquet" + "\n" + "5. Fleur exotique la moins vendue\n6. Statistiques Fleurs\n0. Retour");
                reponse = Convert.ToInt32(Console.ReadLine());
            }
            Console.Clear();
        }
    }
}
