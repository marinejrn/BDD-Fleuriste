using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace projet
{
    
    public class Clients
    {
        // Propriétés de la classe
        public int IdClient { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public int NumTel { get; set; }
        public string Courriel { get; set; }
        public string AdresseFacturation { get; set; }
        public int Mdp { get; set; }
        public string cdc { get; set; }
        public string StatutFidelite { get; set; }
        public Clients()
        {
        }
        public Clients(string nom, string prenom, int id_client, int tel, string courriel, int mdp, string adresseFacturation, string carteCredit, string statutFidelite)
        {
            Prenom = prenom;
            Nom = nom;
            IdClient = id_client;
            NumTel = tel;
            Courriel = courriel;
            Mdp = mdp;
            AdresseFacturation = adresseFacturation;
            cdc = carteCredit;
            StatutFidelite = statutFidelite;
        }
        const string quote = "\"";
        public void AfficherClients()
        {
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=fleuriste;UID=root;PASSWORD=bateau11;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            List<Clients> clients = new List<Clients>();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT id_client, nom, prenom, num_tel, courriel, adresse_facturation, statut_fidelite FROM clients ORDER BY id_client";

            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Clients client = new Clients(Nom, Prenom, IdClient, NumTel, Courriel, Mdp, AdresseFacturation, cdc, StatutFidelite);
                client.IdClient = (int)reader["id_client"];
                client.Nom = (string)reader["nom"];
                client.Prenom = (string)reader["prenom"];
                client.NumTel = (int)reader["num_tel"];
                client.Courriel = (string)reader["courriel"];
                client.AdresseFacturation = (string)reader["adresse_facturation"];
                client.StatutFidelite = (string)reader["statut_fidelite"];

                clients.Add(client);
            }
            reader.Close();

            foreach (Clients client in clients)
            {
                Console.WriteLine(client.IdClient + " " + client.Prenom + " " + client.Nom + " : " + client.NumTel + ", " + client.Courriel + ", " + client.AdresseFacturation + ", " + client.StatutFidelite);
            }
            Console.WriteLine("\n");
        }
        public void AjouterClient(MySqlConnection connection)
        {
            string requete = "INSERT INTO clients VALUES (@id_client, @nom, @prenom, @num_tel, @courriel, @mdp, @adresse_facturation, @carte_credit, @statut_fidelite)";
            MySqlCommand command = new MySqlCommand(requete, connection);

            command.Parameters.AddWithValue("@id_client", this.IdClient);
            command.Parameters.AddWithValue("@nom", this.Nom);
            command.Parameters.AddWithValue("@prenom", this.Prenom);
            command.Parameters.AddWithValue("@num_tel", this.NumTel);
            command.Parameters.AddWithValue("@courriel", this.Courriel);
            command.Parameters.AddWithValue("@mdp", this.Mdp);
            command.Parameters.AddWithValue("@adresse_facturation", this.AdresseFacturation);
            command.Parameters.AddWithValue("@carte_credit", this.cdc);
            command.Parameters.AddWithValue("@statut_fidelite", this.StatutFidelite);
            command.ExecuteNonQuery();
        }       
        public void CommandesClients(MySqlConnection connection, Clients client)
        {
            Console.Clear();
            Console.WriteLine("(1)Anciennes commandes \n(2)Commande en cours ");
            int i = Convert.ToInt32(Console.ReadLine());
            MySqlCommand command = connection.CreateCommand();
            MySqlDataReader reader;
            //ancienne commande
            if (i == 1)
            {
                Console.Clear();
                Console.WriteLine("Anciennes commandes : \n");
                command = connection.CreateCommand();
                command.CommandText = " SELECT id_client FROM clients WHERE id_client=" + client.IdClient + ";";
                reader = command.ExecuteReader();
                int id_client = 0;
                while (reader.Read())
                {
                    id_client = (int)reader["id_client"];
                }
                reader.Close();
                List<Commande> list = new List<Commande>();
                command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM commande WHERE id_client = " + id_client + " AND etat='CL';";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Commande commande = new Commande();
                    commande.Prix = (double)reader["prix"];
                    commande.Adresse_livraison = (string)reader["adresse_livraison"];
                    commande.date_commande = (DateTime)reader["date_commande"];
                    commande.date_livraison = (DateTime)reader["date_livraison"];
                    commande.etat_commande = (string)reader["etat"];
                    commande.Message = (string)reader["message"];
                    if (reader.IsDBNull(reader.GetOrdinal("idbouquet")))
                    {
                        commande.id_bouquet = 0;
                    }
                    else
                    {
                        commande.id_bouquet = (int)reader["idbouquet"];
                    }
                    list.Add(commande);
                }
                reader.Close();
                foreach (Commande commande in list)
                {
                    command = connection.CreateCommand();
                    command.CommandText = "SELECT nom_bouquet FROM bouquet_standard WHERE idbouquet= " + commande.id_bouquet + ";";
                    string nom_bouquet = "";
                    reader= command.ExecuteReader();
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
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(commande.etat_commande);
                    Console.ResetColor();
                    Console.WriteLine(" : " + DateTime.Parse(commande.date_commande.ToString()).ToString("dd/MM/yyyy") + ", " + nom_bouquet + ", " + commande.Prix + " euros, " + quote + commande.Message + quote + ", " + commande.Adresse_livraison + ", " + DateTime.Parse(commande.date_livraison.ToString()).ToString("dd/MM/yyyy")); ;
                    reader.Close();
                }               
            }
            //commande en cours
            else
            {
                Console.Clear();
                Console.WriteLine("Commande en cours : \n");
                command = connection.CreateCommand();
                command.CommandText = " SELECT id_client FROM clients WHERE courriel=" + quote + client.Courriel + quote + ";";
                reader = command.ExecuteReader();
                int id_client = 0;
                while (reader.Read())
                {
                    id_client = (int)reader["id_client"];
                }
                reader.Close();
                command = connection.CreateCommand();
                command.CommandText = "SELECT commande.idbouquet,commande.prix,commande.etat, commande.date_commande,commande.date_livraison, commande.adresse_livraison,commande.message FROM commande WHERE id_client = " +id_client + " AND etat != 'CL'";
                List<Commande> list = new List<Commande>();
                MySqlDataReader affichage;
                affichage = command.ExecuteReader();
                while (affichage.Read())
                {
                    Commande commande = new Commande();
                    commande.Prix = (double)affichage["prix"];
                    commande.Adresse_livraison = (string)affichage["adresse_livraison"];
                    commande.date_commande = (DateTime)affichage["date_commande"];
                    commande.date_livraison = (DateTime)affichage["date_livraison"];
                    commande.Message = (string)affichage["message"];
                    commande.etat_commande = (string)affichage["etat"];
                    if (affichage.IsDBNull(affichage.GetOrdinal("idbouquet")))
                    {
                        commande.id_bouquet = 0; // or handle the DBNull value in some other way
                    }
                    else
                    {
                        commande.id_bouquet = (int)affichage["idbouquet"];
                    }
                    list.Add(commande);
                }
                affichage.Close();
                foreach (Commande commande in list)
                {
                    command = connection.CreateCommand();
                    command.CommandText = "SELECT nom_bouquet FROM bouquet_standard WHERE idbouquet= " + commande.id_bouquet + ";";
                    string nom_bouquet = "";
                    reader = command.ExecuteReader();
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
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(commande.etat_commande);
                    Console.ResetColor();
                    Console.WriteLine(" : " + DateTime.Parse(commande.date_commande.ToString()).ToString("dd/MM/yyyy") + ", " + nom_bouquet + ", " + commande.Prix + " euros, " + quote + commande.Message + quote + ", " + commande.Adresse_livraison + ", " + DateTime.Parse(commande.date_livraison.ToString()).ToString("dd/MM/yyyy")); ;
                    reader.Close();

                }
            }
        }
        public void AfficherProfilClient(MySqlConnection connection, Clients client)
        {
            MySqlCommand command = connection.CreateCommand();
            MySqlDataReader reader;
            Console.WriteLine("MON PROFIL\n-----------\n");
            Console.WriteLine(client.Prenom + " " + client.Nom + " :\nNuméro de télephone : "+client.NumTel + "\nCourriel : " + client.Courriel + "\nMot de passe : " + client.Mdp + "\nCarte de Crédit : " + client.cdc + "\nAdresse de Facturation : " + client.AdresseFacturation + "\nStatut de Fidelité : " + client.StatutFidelite);
            Console.WriteLine("\n(1) Modifer mes informations\n(2) Retour");
            int choix = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            switch (choix)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine(client.Prenom + " " + client.Nom + " :\nNuméro de télephone : " + client.NumTel + "\nCourriel : " + client.Courriel + "\nMot de passe : " + client.Mdp + "\nCarte de Crédit : " + client.cdc + "\nAdresse de Facturation : " + client.AdresseFacturation + "\nStatut de Fidelité : " + client.StatutFidelite);
                    Console.WriteLine("\nMODIFIER :\n1.Prenom\n2.Nom\n3.Numero de telephone\n4.Courriel\n5.Mot de passe\n6.Carte de crédit\n7.Adresse de facturation");
                    int choix2 = Convert.ToInt32(Console.ReadLine());
                    while (choix2 != 0)
                    {
                        switch (choix2)
                        {

                            case 1:
                                Console.Clear();
                                Console.WriteLine("Nouveau Prenom : ");
                                string nouv_prenom = Convert.ToString(Console.ReadLine());
                                command.CommandText = "UPDATE clients SET prenom ='" + nouv_prenom + "' WHERE id_client=" + client.IdClient;
                                command.ExecuteNonQuery();
                                client.Prenom = nouv_prenom;
                                break;
                            case 2:
                                Console.Clear();
                                Console.WriteLine("Nouveau Nom : ");
                                string nouv_nom = Convert.ToString(Console.ReadLine());
                                command.CommandText = "UPDATE clients SET nom ='" + nouv_nom + "' WHERE id_client=" + client.IdClient;
                                command.ExecuteNonQuery();
                                client.Nom= nouv_nom;
                                break;
                            case 3:
                                Console.Clear();
                                Console.WriteLine("Nouveau Numero de telephone : ");
                                int nouv_tel = Convert.ToInt32(Console.ReadLine());
                                command.CommandText = "UPDATE clients SET num_tel =" + nouv_tel + " WHERE id_client=" + client.IdClient;
                                command.ExecuteNonQuery();
                                client.NumTel = nouv_tel;
                                break;
                            case 4:
                                Console.Clear();
                                Console.WriteLine("Nouveau Courriel : ");
                                string nouv_courriel = Convert.ToString(Console.ReadLine());
                                command.CommandText = "UPDATE clients SET courriel ='" + nouv_courriel + "' WHERE id_client=" + client.IdClient;
                                command.ExecuteNonQuery();
                                client.Courriel = nouv_courriel;
                                break;
                            case 5:
                                Console.Clear();
                                Console.WriteLine("Nouveau Mot de passe: ");
                                int nouv_mdp = Convert.ToInt32(Console.ReadLine());
                                command.CommandText = "UPDATE clients SET mdp =" + nouv_mdp + " WHERE id_client=" + client.IdClient;
                                command.ExecuteNonQuery();
                                client.Mdp = nouv_mdp;
                                break;
                            case 6:
                                Console.Clear();
                                Console.WriteLine("Nouvelle Carte de crédit: ");
                                string nouv_cdc = Convert.ToString(Console.ReadLine());
                                client.cdc = nouv_cdc;
                               
                                break;
                            case 7:
                                Console.Clear();
                                Console.WriteLine("Nouvelle adresse de facturation : ");
                                string nouv_adresse = Convert.ToString(Console.ReadLine());
                                command.CommandText = "UPDATE clients SET adresse_facturation ='" + nouv_adresse + "' WHERE id_client=" + client.IdClient;
                                command.ExecuteNonQuery();
                                client.AdresseFacturation = nouv_adresse;
                                break;
                        }
                        Console.WriteLine(client.Prenom + " " + client.Nom + " :\nNuméro de télephone : " + client.NumTel + "\nCourriel : " + client.Courriel + "\nMot de passe : " + client.Mdp + "\nCarte de Crédit : " + client.cdc + "\nAdresse de Facturation : " + client.AdresseFacturation + "\nStatut de Fidelité : " + client.StatutFidelite);
                        Console.WriteLine("\nMODIFIER :\n1.Prenom\n2.Nom\n3.Numero de telephone\n4.Courriel\n5.Mot de passe\n6.Carte de crédit\n7.Adresse de facturation\n0.Retour");
                        choix2 = Convert.ToInt32(Console.ReadLine());
                        Console.Clear();
                    }
                    break;
                case 2:

                    break;
            }
        }
        public void NouveauClient(MySqlConnection connection)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*)AS nb_client FROM clients;";
            int id_client = Convert.ToInt32(command.ExecuteScalar());
            id_client++;
            Console.Write("Nom : ");
            string nom = Convert.ToString(Console.ReadLine());
            Console.Write("Prenom : ");
            string prenom = Convert.ToString(Console.ReadLine());
            Console.Write("Numéro de téléphone : ");
            int tel = Convert.ToInt32(Console.ReadLine());
            Console.Write("Courriel : ");
            string courriel = Convert.ToString(Console.ReadLine());
            Console.Write("Mot de passe (minimun 5 chiffres) : ");
            int mdp = Convert.ToInt32(Console.ReadLine());
            Console.Write("Adresse (rue,ville) : ");
            string adresse = Convert.ToString(Console.ReadLine());
            Console.Write("Carte de crédit : ");
            string cdc = Convert.ToString(Console.ReadLine());
            string statut = "sans statut";
            command.CommandText = "SELECT COUNT(*) AS nb_mail FROM clients WHERE courriel =" + quote + courriel + quote + ";";
            MySqlDataReader reader = command.ExecuteReader();
            Int64 nb_mail = 0;
            while (reader.Read())   // parcours ligne par ligne
            {
                nb_mail = (Int64)reader["nb_mail"];
            }
            reader.Close();
            if (nb_mail == 1)
            {
                Console.WriteLine("Adresse mail déjà utilisée. Veuillez vous connecter à votre compte.");
            }
            else
            {
                Clients nouveauClient = new Clients(nom, prenom, id_client, tel, courriel, mdp, adresse, cdc, statut);
                nouveauClient.AjouterClient(connection);
            }
            Console.WriteLine("\nVotre compte a bien été créé. Vous pouvez vous connecter.\n");
        }
        public void SeConnecter(MySqlConnection connection)
        {
            Console.Write("Adresse mail du compte : ");
            string mail = Convert.ToString(Console.ReadLine());
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) AS verif FROM clients WHERE courriel =" + quote + mail + quote + ";";
            MySqlDataReader reader = command.ExecuteReader();
            Int64 verif = 0;
            while (reader.Read())   // parcours ligne par ligne
            {
                verif = (Int64)reader["verif"];
            }
            reader.Close();
            if (verif == 1)
            {
                Console.Write("Mot de passe : ");
                int mdp = Convert.ToInt32(Console.ReadLine());
                command.CommandText = "SELECT COUNT(*) AS verifi FROM clients WHERE mdp =" + quote + mdp + quote + " AND courriel=" + quote + mail + quote + ";";
                Int64 verifi = 0;
                reader = command.ExecuteReader();
                while (reader.Read())   // parcours ligne par ligne
                {
                    verifi = (Int64)reader["verifi"];
                }
                reader.Close();
                if (verifi == 1)
                {
                    Console.Clear();
                    Clients client = new Clients();
                    command.CommandText = "SELECT * FROM clients WHERE courriel='" + mail + "';";
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        client = new Clients((string)reader["nom"], (string)reader["prenom"], (int)reader["id_client"], (int)reader["num_tel"], (string)reader["courriel"], (int)reader["mdp"], (string)reader["adresse_facturation"], (string)reader["carte_credit"], (string)reader["statut_fidelite"]);

                    }
                    reader.Close();
                    Console.WriteLine("Bienvenue sur votre espace client\n");

                    Console.WriteLine("(MENU)\n\n1.Mon profil\n2.Mes commandes\n3.Nouvelle commande\n0.Quitter");
                    int reponse = Convert.ToInt32(Console.ReadLine());
                    while (reponse != 0)
                    {
                        switch (reponse)
                        {
                            //Mon Profil//
                            case 1:
                                Console.Clear();
                                command.CommandText = "SELECT * FROM clients WHERE courriel='" + mail + "';";
                                reader = command.ExecuteReader();
                                while (reader.Read())
                                {
                                    client = new Clients((string)reader["nom"], (string)reader["prenom"], (int)reader["id_client"], (int)reader["num_tel"], (string)reader["courriel"], (int)reader["mdp"], (string)reader["adresse_facturation"], (string)reader["carte_credit"], (string)reader["statut_fidelite"]);

                                }
                                reader.Close();
                                client.AfficherProfilClient(connection, client);
                                break;

                            //Mes anciennes commandes//
                            case 2:
                                command.CommandText = "SELECT * FROM clients WHERE courriel='" + mail + "';";
                                reader = command.ExecuteReader();
                                while (reader.Read())
                                {
                                    client = new Clients((string)reader["nom"], (string)reader["prenom"], (int)reader["id_client"], (int)reader["num_tel"], (string)reader["courriel"], (int)reader["mdp"], (string)reader["adresse_facturation"], (string)reader["carte_credit"], (string)reader["statut_fidelite"]);

                                }
                                reader.Close();
                                client.CommandesClients(connection, client);
                                break;

                            //Nouvelle commande//
                            case 3:
                                command.CommandText = "SELECT * FROM clients WHERE courriel='" + mail + "';";
                                reader = command.ExecuteReader();
                                while (reader.Read())
                                {
                                    client = new Clients((string)reader["nom"], (string)reader["prenom"], (int)reader["id_client"], (int)reader["num_tel"], (string)reader["courriel"], (int)reader["mdp"], (string)reader["adresse_facturation"], (string)reader["carte_credit"], (string)reader["statut_fidelite"]);
                                }
                                reader.Close();
                                Commande commande = new Commande();
                                commande.NouvelleCommande(connection, client);
                                UpdateStatutFidelite(connection);
                                break;
                        }
                        Console.WriteLine("\n(MENU)\n\n1.Mon profil\n2.Mes commandes\n3.Nouvelle commande\n0.Quitter");
                        reponse = Convert.ToInt32(Console.ReadLine());

                    }
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Mot de passe incorrect\n");
                }

            }
            else
            {
                Console.WriteLine("Adresse mail connectée à aucun compte. Veuillez vous créer un compte.\n");
            }
        }
        public void UpdateStatutFidelite(MySqlConnection connection)
        {

            MySqlCommand updateCommand = connection.CreateCommand();
            updateCommand.CommandText = "UPDATE clients SET statut_fidelite = 'or' WHERE id_client IN (SELECT id_client FROM commande WHERE YEAR(date_commande) = @annee AND MONTH(date_commande) = @mois GROUP BY id_client HAVING COUNT(*) >= 5)";
            updateCommand.Parameters.AddWithValue("@annee", 2023);
            updateCommand.Parameters.AddWithValue("@mois", DateTime.Now.Month);
            updateCommand.ExecuteNonQuery();
            updateCommand = connection.CreateCommand();
            updateCommand.CommandText = "UPDATE clients SET statut_fidelite = 'bronze' WHERE statut_fidelite='sans statut' AND id_client IN (SELECT id_client FROM commande WHERE YEAR(date_commande) = @annee AND MONTH(date_commande) = @mois GROUP BY id_client HAVING COUNT(*)=1)";
            updateCommand.Parameters.AddWithValue("@annee", 2023);
            updateCommand.Parameters.AddWithValue("@mois", DateTime.Now.Month);
            updateCommand.ExecuteNonQuery();
            updateCommand.CommandText = "SELECT statut_fidelite,id_client FROM clients";
            MySqlDataReader reader = updateCommand.ExecuteReader();
            string statut = "";
            int idClient = 0;
            while (reader.Read())
            {
                statut = (string)reader["statut_fidelite"];
                idClient = (int)reader["id_client"];
            }

            connection.Close();
            connection.Open();
        }
    }
}




