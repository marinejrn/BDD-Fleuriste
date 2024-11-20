using MySql.Data.MySqlClient;

namespace projet
{
    public class Commande
    {
        public string Message { get; set; }
        public double Prix { get; set; }
        public string Adresse_livraison { get; set; }
        public int id_commande { get; set; }
        public DateTime date_commande { get; set;}
        public DateTime date_livraison { get; set; }
        public string etat_commande { get; set; }
        public int id_bouquet { get; set; }
        public int id_client { get; set; }
        public string alerte { get; set; }
        public Commande()
        {
        }
        const string quote = "\"";
        public Commande(string message, double prix, string adresse,int idcommande,DateTime datecommande,DateTime datelivraison, string etat,int idbouquet,int idclient,string alerte)
        {
            Message = message;
            Prix = prix;
            Adresse_livraison = adresse;
            id_commande = idcommande;
            date_commande = datecommande;
            date_livraison = datelivraison;
            etat_commande = etat;
            id_bouquet = idbouquet;
            id_client=idclient;
        }
        public void NouvelleCommande(MySqlConnection connection, Clients client)
        {
            Console.Clear();
            Console.WriteLine("NOUVELLE COMMANDE\n");
            Console.WriteLine("Choix magasin :");
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT ville FROM Magasin";
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            string ville = "";
            int y = 1;
            while (reader.Read())
            {
                ville = (string)reader["ville"];
                Console.WriteLine(y + "." + ville);
                y++;
            }
            reader.Close();
            int choix_magasin = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            Console.WriteLine("Choix commande : \n1.Commande standard \n2.Commande personnalisée");
            int choix_commande = Convert.ToInt32(Console.ReadLine());
            //commande standard//
            if (choix_commande == 1)
            {
                command.CommandText = " SELECT id_client FROM clients WHERE id_client=" + client.IdClient + ";";
                reader = command.ExecuteReader();
                int id_client = 0;
                while (reader.Read())
                {
                    id_client = (int)reader["id_client"];
                }
                reader.Close();
                Console.Clear();
                Bouquet bouquet = new Bouquet();
                bouquet.AfficherBouquet(choix_magasin);
                //choix bouquet
                Console.Write("\nChoix bouquet : ");
                int choix_bouquet = Convert.ToInt32(Console.ReadLine());
                switch (choix_bouquet)
                {
                    case 1:
                        bouquet.NomBouquet = "Gros merci";
                        bouquet.PrixBouquet = 45;
                        command.CommandText = "SELECT idbouquet FROM bouquet_standard WHERE nom_bouquet= '" + bouquet.NomBouquet + "' AND idmagasin = " + choix_magasin + ";";
                        bouquet.Id = (int)command.ExecuteScalar();
                        break;
                    case 2:
                        bouquet.NomBouquet = "L'amoureux";
                        bouquet.PrixBouquet = 65;
                        command.CommandText = "SELECT idbouquet FROM bouquet_standard WHERE nom_bouquet=" + quote + bouquet.NomBouquet + quote + " AND idmagasin = " + choix_magasin + ";";
                        bouquet.Id = (int)command.ExecuteScalar();
                        break;
                    case 3:
                        bouquet.NomBouquet = "L'exotique";
                        bouquet.PrixBouquet = 40;
                        command.CommandText = "SELECT idbouquet FROM bouquet_standard WHERE nom_bouquet=" + quote + bouquet.NomBouquet + quote + " AND idmagasin = " + choix_magasin + ";";
                        bouquet.Id = (int)command.ExecuteScalar();
                        break;
                    case 4:
                        bouquet.NomBouquet = "Maman";
                        bouquet.PrixBouquet = 80;
                        command.CommandText = "SELECT idbouquet FROM bouquet_standard WHERE nom_bouquet= '" + bouquet.NomBouquet + "' AND idmagasin = " + choix_magasin + ";";
                        bouquet.Id = (int)command.ExecuteScalar();
                        break;
                    case 5:
                        bouquet.NomBouquet = "Vive la mariée";
                        bouquet.PrixBouquet = 120;
                        command.CommandText = "SELECT idbouquet FROM bouquet_standard WHERE nom_bouquet=" + quote + bouquet.NomBouquet + quote + " AND idmagasin =" + choix_magasin + ";";
                        bouquet.Id = (int)command.ExecuteScalar();
                        break;
                }
                command.CommandText = "SELECT quantite FROM bouquet_standard WHERE nom_bouquet=" + quote + bouquet.NomBouquet + quote + " AND idmagasin=" + choix_magasin + ";";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    bouquet.Quantite = (int)reader["quantite"];
                }
                reader.Close();
                if (bouquet.Quantite > 0)
                {
                    Console.Write("Adresse de livraison : ");
                    string adresse = Convert.ToString(Console.ReadLine());
                    Console.Write("Date de la livraison : ");
                    DateTime livraison = Convert.ToDateTime(Console.ReadLine());
                    Console.Write("Message à mettre avec le bouquet : ");
                    string message_bouquet = Convert.ToString(Console.ReadLine());
                    Int64 id_commande = 0;
                    command.CommandText = "SELECT COUNT(*)AS nb_com FROM commande;";
                    reader = command.ExecuteReader();
                    while (reader.Read())   // parcours ligne par ligne
                    {
                        id_commande = (Int64)reader["nb_com"];
                    }
                    id_commande++;
                    reader.Close();
                    string etat = "";
                    DateTime datecommande = DateTime.Now;
                    TimeSpan difference = livraison.Subtract(datecommande);
                    if (difference.TotalDays <= 3)
                    {
                        etat = "VINV";
                    }
                    else
                    {
                        etat = "CC";
                    }
                    string requete = "INSERT INTO commande(magasin,id_commande,id_client,etat,adresse_livraison,date_livraison,message,date_commande,prix,idbouquet) VALUES (@magasin,@id_commande,@id_client,@etat,@adresse_livraison,@date_livraison,@message,@date_commande,@prix,@idbouquet);";
                    command = new MySqlCommand(requete, connection);
                    command.Parameters.AddWithValue("@magasin", choix_magasin);
                    command.Parameters.AddWithValue("@id_client", id_client);
                    command.Parameters.AddWithValue("@id_commande", id_commande);
                    command.Parameters.AddWithValue("@idbouquet", bouquet.Id);
                    command.Parameters.AddWithValue("@adresse_livraison", adresse);
                    command.Parameters.AddWithValue("@message", message_bouquet);

                    if (client.StatutFidelite == "or")
                    {
                        bouquet.PrixBouquet = bouquet.PrixBouquet * 0.85; //15% de reduction pour les statuts or
                        Console.WriteLine("\nStatut or : réduction de 15% appliquée.");
                    }
                    else if (client.StatutFidelite == "bronze")
                    {
                        bouquet.PrixBouquet = bouquet.PrixBouquet * 0.95; //5% de réduction pour les statuts bronze
                        Console.WriteLine("\nStatut bronze : réduction de 5% appliquée.");
                    }
                    command.Parameters.AddWithValue("@prix", bouquet.PrixBouquet);
                    command.Parameters.AddWithValue("@date_commande", DateTime.Now);
                    command.Parameters.AddWithValue("@date_livraison", livraison);
                    command.Parameters.AddWithValue("@etat", etat);
                    command.ExecuteNonQuery();
                    Console.WriteLine("\nRécapitulatif commande : " + bouquet.NomBouquet + "/ " + adresse + "/ " + quote + message_bouquet + quote + "/ " + bouquet.PrixBouquet + " euros/ " + DateTime.Parse(livraison.ToString()).ToString("dd/MM/yyyy"));

                    //On enleve le bouquet choisi au stock//
                    if (etat != "VINV")
                    {
                        Bouquet.MettreAJourQuantite(connection, bouquet, choix_magasin, -1);
                    }
                }
                else
                {
                    Console.WriteLine("Ce produit n'est pas disponible pour le moment.");
                }
            }
            //commande personnalisée//
            else
            {
                //fleurs
                Console.Clear();
                Console.WriteLine("Voici notre catalogue de fleurs : ");
                command.CommandText = "SELECT * FROM fleurs";
                reader = command.ExecuteReader();
                string nom_fleurs = "";
                string disponibilité = "";
                string stock_fleurs = "";
                float prix_fleurs = 0;
                while (reader.Read())
                {
                    nom_fleurs = (string)reader["nom_fleurs"];
                    disponibilité = (string)reader["disponibilité"];
                    prix_fleurs = (float)reader["prix"];
                    if (disponibilité == "mai à novembre")
                    {
                        if (DateTime.Now.Month < 5 || DateTime.Now.Month > 11)
                        {
                            stock_fleurs = "Indisponible";
                            Console.Write(nom_fleurs + ", " + disponibilité + ", " + prix_fleurs + " euros,  ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(stock_fleurs);
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        stock_fleurs = "Disponible";
                        Console.Write(nom_fleurs + ", " + disponibilité + ", " + prix_fleurs + " euros,  ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(stock_fleurs);
                        Console.ResetColor();
                    }
                }
                reader.Close();

                //accessoire
                Console.WriteLine("\nVoici notre catalogue d'accessoires : ");
                List<Produit> liste = new List<Produit>();

                command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM produit WHERE idmagasin = " + choix_magasin + ";";
                reader = command.ExecuteReader();
                string dispo = "";
                while (reader.Read())
                {
                    Produit produit = new Produit();
                    produit.NomProduit = (string)reader["nom_produit"];
                    produit.Prix = (float)reader["prix"];
                    produit.Stock = (int)reader["stock"];
                    liste.Add(produit);
                }
                reader.Close();
                foreach (Produit produit in liste)
                {
                    if (produit.Stock == 0)
                    {
                        dispo = "Rupture de stock";
                        Console.Write(produit.NomProduit + " : " + produit.Prix + " euros  ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(dispo);
                        Console.ResetColor();
                    }
                    else
                    {
                        dispo = "En stock";
                        Console.Write(produit.NomProduit + " : " + produit.Prix + " euros  ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(dispo);
                        Console.ResetColor();
                    }
                }
                reader.Close();

                //Description composition
                Console.Write("\nDecrivez votre composition : ");
                string arrangement = Convert.ToString(Console.ReadLine());
                bool test = true;
                foreach (Produit produit in liste)
                {
                    if (arrangement.Contains(produit.NomProduit.ToLower()))
                    {
                        if (produit.Stock == 0)
                        {
                            Console.WriteLine(produit.NomProduit + " est en rupture de stock.");
                            Console.ReadKey();
                            test = false;
                        }
                    }
                }
                if (arrangement.Contains("Glaïeul") && DateTime.Now.Month < 5 || DateTime.Now.Month > 11)
                {
                    Console.WriteLine("Les Glaïeuls ne sont pas disponible en ce moment.");
                }
                else if (test == true)
                {
                    Console.Write("Prix de la commande : ");
                    int prix_max = Convert.ToInt32(Console.ReadLine());
                    Int64 id_commande = 0;
                    Console.Write("Adresse de livraison : ");
                    string adresse = Convert.ToString(Console.ReadLine());
                    Console.Write("Date de la livraison : ");
                    DateTime livraison = Convert.ToDateTime(Console.ReadLine());

                    Console.Write("Message à mettre avec le bouquet : ");
                    string message_bouquet = Convert.ToString(Console.ReadLine());
                    command.CommandText = "SELECT COUNT(*)AS nb_com FROM commande;";
                    reader = command.ExecuteReader();
                    while (reader.Read())   // parcours ligne par ligne
                    {
                        id_commande = (Int64)reader["nb_com"];
                    }
                    id_commande++;
                    reader.Close();
                    string insertion = "INSERT INTO commande(magasin,id_commande,id_client,etat,adresse_livraison,date_livraison,message,date_commande,prix) VALUES (@magasin,@id_commande,@id_client,@etat,@adresse_livraison,@date_livraison,@message,@date_commande,@prix);";
                    command = new MySqlCommand(insertion, connection);
                    string etat = "CPAV";
                    command.Parameters.AddWithValue("@magasin", choix_magasin);
                    command.Parameters.AddWithValue("@id_client", client.IdClient);
                    command.Parameters.AddWithValue("@id_commande", id_commande);
                    command.Parameters.AddWithValue("@adresse_livraison", adresse);
                    command.Parameters.AddWithValue("@message", message_bouquet);
                    command.Parameters.AddWithValue("@prix", prix_max);
                    command.Parameters.AddWithValue("@date_commande", DateTime.Now);
                    command.Parameters.AddWithValue("@date_livraison", livraison);
                    command.Parameters.AddWithValue("@etat", etat);
                    command.ExecuteNonQuery();

                    string requete = "INSERT INTO bouquet_perso(idcommande,arrangement,prix_max) VALUES (@idcommande,@arrangement,@prix_max);";
                    command = new MySqlCommand(requete, connection);
                    command.Parameters.AddWithValue("@idcommande", id_commande);
                    command.Parameters.AddWithValue("@arrangement", arrangement);
                    command.Parameters.AddWithValue("prix_max", prix_max);
                    command.ExecuteNonQuery();
                    Console.WriteLine("\nRécapitulatif commande : " + arrangement + "/ " + quote + message_bouquet + quote + "/ " + prix_max + " euros/ " + DateTime.Parse(livraison.ToString()).ToString("dd/MM/yyyy"));
                }
            }
        }
        public void CommandeEnCours(MySqlConnection connection, int magasin)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT clients.nom, clients.prenom,commande.id_commande, commande.idbouquet,commande.etat,commande.adresse_livraison,commande.date_commande,commande.message,commande.prix,commande.date_livraison FROM clients INNER JOIN commande ON clients.id_client = commande.id_client WHERE etat!='CL' AND magasin= " + magasin + " ORDER BY id_commande;";
            MySqlDataReader affichage;
            affichage = command.ExecuteReader();
            List<Clients> clients = new List<Clients>();
            List<Commande> list = new List<Commande>();
            while (affichage.Read())
            {
                Clients client = new Clients();
                client.Nom = (string)affichage["nom"];
                client.Prenom = (string)affichage["prenom"];
                Commande commande1 = new Commande();
                commande1.Prix = (double)affichage["prix"];
                commande1.id_commande = (int)affichage["id_commande"];
                commande1.Adresse_livraison = (string)affichage["adresse_livraison"];
                commande1.date_commande = (DateTime)affichage["date_commande"];
                commande1.date_livraison = (DateTime)affichage["date_livraison"];
                commande1.etat_commande = (string)affichage["etat"];
                TimeSpan difference = commande1.date_livraison.Subtract(DateTime.Now.Date);
                if (commande1.etat_commande == "CC" && difference.TotalDays <= 3)
                {
                    commande1.etat_commande = "CAL";
                }
                commande1.Message = (string)affichage["message"];
                if (affichage.IsDBNull(affichage.GetOrdinal("idbouquet")))
                {
                    commande1.id_bouquet = 0;
                }
                else
                {
                    commande1.id_bouquet = (int)affichage["idbouquet"];
                }

                list.Add(commande1);
                clients.Add(client);
            }
            affichage.Close();
            int i = 0;
            foreach (Commande commande1 in list)
            {
                command = connection.CreateCommand();
                command.CommandText = "SELECT nom_bouquet FROM bouquet_standard WHERE idbouquet= " + commande1.id_bouquet + ";";
                string nom_bouquet = "";
                affichage = command.ExecuteReader();
                if (commande1.id_bouquet == 0)
                {
                    nom_bouquet = "Commande personnalisée";
                }
                else
                {
                    while (affichage.Read())
                    {
                        nom_bouquet = Convert.ToString(affichage["nom_bouquet"]);
                    }
                }
                affichage.Close();
                Console.Write(commande1.id_commande + ". ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(commande1.etat_commande);
                Console.ResetColor();
                Console.WriteLine(" : " + clients[i].Prenom + " " + clients[i].Nom + " : " + DateTime.Parse(commande1.date_commande.ToString()).ToString("dd/MM/yyyy") + "; " + nom_bouquet + "; " + commande1.Prix + " euros; " + quote + commande1.Message + quote + "; " + commande1.Adresse_livraison + "; " + DateTime.Parse(commande1.date_livraison.ToString()).ToString("dd/MM/yyyy") + " ");
                i++;
            }

            //Modifier une commande
            Console.WriteLine("\nChoississez la commande à vérifier (0 pour retourner au menu : )");
            int commande = Convert.ToInt32(Console.ReadLine());
            if (commande != 0)
            {
                bool presence = false;
                foreach (Commande commande1 in list)
                {
                    if (commande1.id_commande == commande)
                    {
                        presence = true;
                        Console.Clear();
                        if (commande1.etat_commande == "CPAV")
                        {
                            Console.WriteLine("(1)Changer statut\n(2)Items utilisés pour la commande personnalisée\n(3)Retour");
                            int choix = Convert.ToInt32(Console.ReadLine());
                            switch (choix)
                            {
                                //changer statut
                                case 1:
                                    Console.Clear();
                                    Console.WriteLine("\nCC Commande complète.\nCAL Commande à livrer.\nCL Commande livrée.La commande a été livrée à l’adresse indiquée par le client.");
                                    Console.Write("Nouveau statut de la commande N°" + commande + " : ");
                                    string nv_statut = Convert.ToString(Console.ReadLine());
                                    command.CommandText = "UPDATE commande SET etat ='" + nv_statut + "' WHERE id_commande=" + commande;
                                    command.ExecuteNonQuery();
                                    Console.Clear();
                                    Console.WriteLine("Statut modifié\n");
                                    break;

                                //commande perso
                                case 2:
                                    Console.Clear();
                                    command = connection.CreateCommand();
                                    command.CommandText = "SELECT arrangement,prix_max FROM bouquet_perso WHERE idcommande= " + commande + ";";
                                    affichage = command.ExecuteReader();
                                    string arrangement = "";
                                    float prix_max = 0;
                                    Produit produit = new Produit();
                                    while (affichage.Read())
                                    {
                                        arrangement = (string)affichage["arrangement"];
                                        prix_max = (float)affichage["prix_max"];
                                    }
                                    affichage.Close();
                                    arrangement = arrangement.ToLower();
                                    float prix_commande = 0;
                                    float nb_vase = 0;
                                    float nb_rubans = 0;
                                    float nb_boite = 0;
                                    float nb_gerbera = 0;
                                    float nb_rose = 0;
                                    float nb_marguerite = 0;
                                    float nb_glaieul = 0;
                                    float nb_ginger = 0;
                                    bool test = false;
                                    do
                                    {
                                        Console.WriteLine("Description de la commande : " + arrangement + "\nPrix maximum : " + prix_max + "\n");
                                        prix_commande = 0;
                                        //fleurs
                                        Console.WriteLine("Voici notre catalogue de fleurs : ");
                                        command.CommandText = "SELECT * FROM fleurs";
                                        affichage = command.ExecuteReader();
                                        string nom_fleurs = "";
                                        string disponibilité = "";
                                        string stock_fleurs = "";
                                        float prix_fleurs = 0;
                                        while (affichage.Read())
                                        {
                                            nom_fleurs = (string)affichage["nom_fleurs"];
                                            disponibilité = (string)affichage["disponibilité"];
                                            prix_fleurs = (float)affichage["prix"];
                                            if (disponibilité == "mai à novembre")
                                            {
                                                if (DateTime.Now.Month < 5 || DateTime.Now.Month > 11)
                                                {
                                                    stock_fleurs = "Indisponible";
                                                    Console.Write(nom_fleurs + ", " + disponibilité + ", " + prix_fleurs + " euros,  ");
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine(stock_fleurs);
                                                    Console.ResetColor();
                                                }
                                            }
                                            else
                                            {
                                                stock_fleurs = "Disponible";
                                                Console.Write(nom_fleurs + ", " + disponibilité + ", " + prix_fleurs + " euros,  ");
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.WriteLine(stock_fleurs);
                                                Console.ResetColor();
                                            }
                                        }
                                        affichage.Close();

                                        //accessoire
                                        Console.WriteLine("\nVoici notre catalogue d'accessoires : ");
                                        List<Produit> liste = new List<Produit>();

                                        command = connection.CreateCommand();
                                        command.CommandText = "SELECT * FROM produit WHERE idmagasin = " + magasin + ";";
                                        affichage = command.ExecuteReader();
                                        string dispo = "";
                                        while (affichage.Read())
                                        {
                                            produit = new Produit();
                                            produit.NomProduit = (string)affichage["nom_produit"];
                                            produit.Prix = (float)affichage["prix"];
                                            produit.Stock = (int)affichage["stock"];
                                            liste.Add(produit);
                                        }
                                        affichage.Close();
                                        foreach (Produit produit2 in liste)
                                        {
                                            if (produit2.Stock == 0)
                                            {
                                                dispo = "Rupture de stock";
                                                Console.Write(produit2.NomProduit + " : " + produit2.Prix + " euros ");
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine(dispo);
                                                Console.ResetColor();
                                            }
                                            else
                                            {
                                                dispo = " en stock";
                                                Console.Write(produit2.NomProduit + " : " + produit2.Prix + " euros, ");
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.WriteLine(produit2.Stock + dispo);
                                                Console.ResetColor();
                                            }
                                        }
                                        affichage.Close();
                                        Console.WriteLine();
                                        if (arrangement.Contains("vase") || arrangement.Contains("vases"))
                                        {
                                            Console.Write("Entrer le nombre de vases : ");
                                            nb_vase = Convert.ToInt32(Console.ReadLine());
                                            command.CommandText = "SELECT stock FROM produit WHERE idmagasin = " + magasin + " AND nom_produit ='Vase';";
                                            int quantite = (int)command.ExecuteScalar();
                                            while (nb_vase > quantite)
                                            {
                                                Console.WriteLine("Stock insuffisant.");
                                                Console.Write("Entrer le nombre de vases : ");
                                                nb_vase = Convert.ToInt32(Console.ReadLine());
                                            }
                                            command.CommandText = "SELECT prix FROM produit WHERE nom_produit='Vase';";
                                            prix_commande += nb_vase * (float)command.ExecuteScalar();
                                            Console.WriteLine("Il vous reste : " + (prix_max - prix_commande) + " euros");
                                        }
                                        if (arrangement.Contains("rubans") || arrangement.Contains("ruban"))
                                        {
                                            Console.Write("Entrer le nombre de rubans : ");
                                            nb_rubans = Convert.ToInt32(Console.ReadLine());
                                            command.CommandText = "SELECT stock FROM produit WHERE idmagasin = " + magasin + " AND nom_produit ='Ruban';";
                                            int quantite = (int)command.ExecuteScalar();
                                            while (nb_rubans > quantite)
                                            {
                                                Console.WriteLine("Stock insuffisant.");
                                                Console.Write("Entrer le nombre de rubans : ");
                                                nb_rubans = Convert.ToInt32(Console.ReadLine());
                                            }
                                            command.CommandText = "SELECT prix FROM produit WHERE nom_produit='Ruban';";
                                            prix_commande += nb_rubans * (float)command.ExecuteScalar();
                                            Console.WriteLine("Il vous reste : " + (prix_max - prix_commande) + " euros");
                                        }
                                        if (arrangement.Contains("boîtes") || arrangement.Contains("boîte") || arrangement.Contains("boites") || arrangement.Contains("boite"))
                                        {
                                            Console.Write("Entrer le nombre de boîtes : ");
                                            nb_boite = Convert.ToInt32(Console.ReadLine());
                                            command.CommandText = "SELECT stock FROM produit WHERE idmagasin = " + magasin + " AND nom_produit ='Boîte';";
                                            int quantite = (int)command.ExecuteScalar();
                                            while (nb_boite > quantite)
                                            {
                                                Console.WriteLine("Stock insuffisant.");
                                                Console.Write("Entrer le nombre de boîtes : ");
                                                nb_boite = Convert.ToInt32(Console.ReadLine());
                                            }
                                            command.CommandText = "SELECT prix FROM produit WHERE nom_produit='Boîte';";
                                            prix_commande += nb_boite * (float)command.ExecuteScalar();
                                            Console.WriteLine("Il vous reste : " + (prix_max - prix_commande) + " euros");
                                        }
                                        if (arrangement.Contains("gerbera") || arrangement.Contains("gerberas"))
                                        {
                                            Console.Write("Entrer le nombre de Gerberas : ");
                                            nb_gerbera = Convert.ToInt32(Console.ReadLine());
                                            command.CommandText = "SELECT prix FROM fleurs WHERE nom_fleurs='Gerbera';";
                                            prix_commande += nb_gerbera * (float)command.ExecuteScalar();
                                            Console.WriteLine("Il vous reste : " + (prix_max - prix_commande) + " euros");
                                        }
                                        if (arrangement.Contains("glaïeul") || arrangement.Contains("glaïeuls"))
                                        {

                                            Console.Write("Entrer le nombre de Glaïeuls : ");
                                            nb_glaieul = Convert.ToInt32(Console.ReadLine());
                                            command.CommandText = "SELECT prix FROM fleurs WHERE nom_fleurs='Glaïeul';";
                                            prix_commande += nb_glaieul * (int)command.ExecuteScalar();
                                            Console.WriteLine("Il vous reste : " + (prix_max - prix_commande) + " euros");
                                        }
                                        if (arrangement.Contains("rose") || arrangement.Contains("roses"))
                                        {
                                            Console.Write("Entrer le nombre de Roses : ");
                                            nb_rose = Convert.ToInt32(Console.ReadLine());
                                            command.CommandText = "SELECT prix FROM fleurs WHERE nom_fleurs='Rose Rouge';";
                                            prix_commande += nb_rose * (float)command.ExecuteScalar();
                                            Console.WriteLine("Il vous reste : " + (prix_max - prix_commande) + " euros");
                                        }
                                        if (arrangement.Contains("ginger") || arrangement.Contains("gingers"))
                                        {
                                            Console.Write("Entrer le nombre de Ginger : ");
                                            nb_ginger = Convert.ToInt32(Console.ReadLine());
                                            command.CommandText = "SELECT prix FROM fleurs WHERE nom_fleurs='Ginger';";
                                            prix_commande += nb_ginger * (float)command.ExecuteScalar();
                                            Console.WriteLine("Il vous reste : " + (prix_max - prix_commande) + " euros");
                                        }
                                        if (arrangement.Contains("marguerite") || arrangement.Contains("marguerites"))
                                        {
                                            Console.Write("Entrer le nombre de Marguerite : ");
                                            nb_marguerite = Convert.ToInt32(Console.ReadLine());
                                            command.CommandText = "SELECT prix FROM fleurs WHERE nom_fleurs='Marguerite';";
                                            prix_commande += nb_marguerite * (float)command.ExecuteScalar();
                                            Console.WriteLine("Il vous reste : " + (prix_max - prix_commande) + " euros");
                                        }
                                        if (prix_commande <= prix_max)
                                        {
                                            test = true;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Vous avez depassez de " + (prix_commande - prix_max) + " euros.");
                                            Console.ReadKey();
                                        }
                                        Console.Clear();
                                    }
                                    while (test == false);
                                    command.CommandText = "UPDATE commande SET etat='CC' WHERE id_commande=" + commande1.id_commande + ";";
                                    command.ExecuteNonQuery();

                                    produit.NomProduit = "Vase";
                                    Produit.MettreAJourQuantite(connection, produit, magasin, -nb_vase);
                                    produit.NomProduit = "Boîte";
                                    Produit.MettreAJourQuantite(connection, produit, magasin, -nb_boite);
                                    produit.NomProduit = "Ruban";
                                    Produit.MettreAJourQuantite(connection, produit, magasin, -nb_rubans);

                                    command.CommandText = "UPDATE fleurs SET nb_achat=nb_achat +  " + nb_gerbera + " WHERE nom_fleurs='Gerbera';";
                                    command.ExecuteNonQuery();
                                    command.CommandText = "UPDATE fleurs SET nb_achat=nb_achat + " + nb_rose + " WHERE nom_fleurs='Rose Rouge';";
                                    command.ExecuteNonQuery();
                                    command.CommandText = "UPDATE fleurs SET nb_achat=nb_achat + " + nb_marguerite + " WHERE nom_fleurs='Marguerite';";
                                    command.ExecuteNonQuery();
                                    command.CommandText = "UPDATE fleurs SET nb_achat=nb_achat + " + nb_glaieul + " WHERE nom_fleurs='Glaïeul';";
                                    command.ExecuteNonQuery();
                                    command.CommandText = "UPDATE fleurs SET nb_achat=nb_achat + " + nb_ginger + " WHERE nom_fleurs='Ginger';";
                                    command.ExecuteNonQuery();

                                    Console.WriteLine("La commande n°" + commande + " contient donc :\n" + nb_vase + " vases\n" + nb_rubans + " rubans\n" + nb_boite + " boîtes\n" + nb_rose + " roses rouges\n" + nb_marguerite + " marguerites\n" + nb_glaieul + " glaïeuls\n" + nb_ginger + " gingers\n" + nb_gerbera + " gerberas\npour un montant de " + prix_commande + " euros.");
                                    Console.ReadKey();
                                    Console.Clear();
                                    break;
                                case 3:
                                    Console.Clear();
                                    break;
                            }
                        }
                        else
                        {
                            if (commande1.etat_commande == "VINV")
                            {
                                Console.WriteLine("(1)Vérifier inventaire\n(0)Retour");
                                int choix = Convert.ToInt32(Console.ReadLine());
                                switch (choix)
                                {
                                    case 1:
                                        Console.Clear();
                                        command = connection.CreateCommand();
                                        command.CommandText = "SELECT nom_bouquet FROM bouquet_standard WHERE idbouquet= " + commande1.id_bouquet + ";";
                                        string nom_bouquet = "";
                                        affichage = command.ExecuteReader();
                                        while (affichage.Read())
                                        {
                                            nom_bouquet = Convert.ToString(affichage["nom_bouquet"]);
                                        }
                                        affichage.Close();
                                        Console.WriteLine("Items nécessaire à la commande : " + nom_bouquet + "\n");
                                        Magasin.AfficherInventaire(connection, magasin);
                                        Console.WriteLine("\n(1)Inventaire suffisant \n(2)Inventaire insuffisant");
                                        int inv = Convert.ToInt32(Console.ReadLine());
                                        if (inv == 1)
                                        {
                                            Bouquet bouquet = new Bouquet();
                                            bouquet.NomBouquet = nom_bouquet;

                                            string nouv_statut = "CC";
                                            command.CommandText = "UPDATE commande SET etat ='" + nouv_statut + "' WHERE id_commande=" + commande1.id_commande;
                                            command.ExecuteNonQuery();
                                            Bouquet.MettreAJourQuantite(connection, bouquet, magasin, -1);
                                        }
                                        else
                                        {

                                        }
                                        break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("(1)Changer statut\n(2)Retour");
                                int choix = Convert.ToInt32(Console.ReadLine());
                                switch (choix)
                                {
                                    case 1:
                                        Console.Clear();
                                        Console.WriteLine("VINV Commande standard pour laquelle un employé doit vérifier l’inventaire. \nCC Commande complète. \nCPAV Commande personnalisée à vérifier.\nCAL Commande à livrer.\nCL Commande livrée.La commande a été livrée à l’adresse indiquée par le client.");
                                        foreach (Commande commande2 in list)
                                        {
                                            if (commande2.id_commande == commande)
                                            {
                                                Console.Write("\nNouveau statut de la commande N°" + commande + " : ");
                                                string nv_statut = Convert.ToString(Console.ReadLine());
                                                command.CommandText = "UPDATE commande SET etat ='" + nv_statut + "' WHERE id_commande=" + commande;
                                                command.ExecuteNonQuery();
                                            }
                                        }
                                        Console.Clear();
                                        if (commande != 0)
                                        {
                                            Console.WriteLine("Statut modifié\n");
                                        }
                                        break;
                                    case 2:
                                        Console.Clear();
                                        break;
                                }
                            }
                        }
                    }
                }
                if (presence == false)
                {
                    Console.WriteLine("Vous ne pouvez pas modifier cette commande");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            else
            {
                Console.Clear();
            }
            connection.Close();
        }
        public void Historique(MySqlConnection connection, int magasin)
        {

            //nombre de commandes par clients//
            Console.WriteLine("Nombre de commandes par clients : " + "\n");
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT nom, prenom, COUNT(*) AS nb_commandes FROM clients JOIN commande ON clients.id_client = commande.id_client WHERE etat='CL' AND magasin= " + magasin + " GROUP BY clients.id_client; ";
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            string nom = "";
            string prenom = "";
            Int64 nb_commandes = 0;
            while (reader.Read())
            {
                nom = (string)reader["nom"];
                prenom = (string)reader["prenom"];
                nb_commandes = (Int64)reader["nb_commandes"];
                Console.WriteLine(prenom + " " + nom + " : " + nb_commandes);
            }
            reader.Close();

            //Détails des commmandes//
            Console.WriteLine("\n(1)Détails des commandes\n(0)Retour");
            int rep = Convert.ToInt32(Console.ReadLine());
            switch (rep)
            {
                case 1:
                    Console.Write("Entrer le nom du client :");
                    string reponse = Convert.ToString(Console.ReadLine());
                    command.CommandText = "SELECT clients.nom, clients.prenom,clients.id_client, commande.idbouquet,commande.prix, commande.date_commande,commande.date_livraison, commande.adresse_livraison,commande.message FROM clients INNER JOIN commande ON clients.id_client = commande.id_client WHERE nom=" + quote + reponse + quote + " AND etat='CL' AND magasin= " + magasin + ";";
                    MySqlDataReader affichage;
                    affichage = command.ExecuteReader();
                    Console.Clear();
                    int i = 0;
                    List<Commande> list = new List<Commande>();
                    List<Clients> listclient = new List<Clients>();
                    while (affichage.Read())
                    {
                        Commande commande = new Commande();
                        Clients client = new Clients();
                        client.Nom = (string)affichage["nom"];
                        client.Prenom = (string)affichage["prenom"];
                        commande.Prix = (double)affichage["prix"];
                        commande.Adresse_livraison = (string)affichage["adresse_livraison"];
                        commande.date_commande = (DateTime)affichage["date_commande"];
                        commande.date_livraison = (DateTime)affichage["date_livraison"];
                        commande.Message = (string)affichage["message"];
                        if (affichage.IsDBNull(affichage.GetOrdinal("idbouquet")))
                        {
                            commande.id_bouquet = 0;
                        }
                        else
                        {
                            commande.id_bouquet = (int)affichage["idbouquet"];
                        }
                        listclient.Add(client);
                        list.Add(commande);
                    }
                    affichage.Close();
                    foreach (Commande commande in list)
                    {
                        command = connection.CreateCommand();
                        command.CommandText = "SELECT nom_bouquet FROM bouquet_standard WHERE idbouquet= " + commande.id_bouquet + ";";
                        reader = command.ExecuteReader();
                        string nom_bouquet = "";
                        if (commande.id_bouquet == 0)
                        {
                            nom_bouquet = "Commande personnalisée";
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                nom_bouquet = Convert.ToString(reader["nom_bouquet"]);
                            }
                        }
                        reader.Close();
                        Console.WriteLine(listclient[i].Prenom + " " + listclient[i].Nom + " : " + nom_bouquet + ", " + commande.Prix + " euros" + ", " + quote + commande.Message + quote + ", " + DateTime.Parse(commande.date_commande.ToString()).ToString("dd/MM/yyyy") + ", " + DateTime.Parse(commande.date_livraison.ToString()).ToString("dd/MM/yyyy"));
                        i++;
                    }
                    break;
                case 2:
                    Console.Clear();
                    break;
            }
        }
    }
}
