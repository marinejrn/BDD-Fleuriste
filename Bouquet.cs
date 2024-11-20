using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace projet
{
    public class Bouquet
    {
        public string NomBouquet { get; set; }
        public string Composition { get; set; }
        public double PrixBouquet { get; set; }
        public string Occasion { get; set; }
        public int Quantite { get; set; }
        public int Id { get; set; }
        public Bouquet(int id,string nomBouquet, string composition, double prixBouquet, string occasion, int quantite)
        {
            Id = id;
            NomBouquet = nomBouquet;
            Composition = composition;
            PrixBouquet = prixBouquet;
            Occasion = occasion;
            Quantite = quantite;
        }
        public Bouquet()
        {
        }
        public void AfficherBouquet(int magasin)
        {
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=fleuriste;UID=root;PASSWORD=bateau11;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            List<Bouquet> bouquets = new List<Bouquet>();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT nom_bouquet,prix_bouquet,quantite,composition,occasion FROM bouquet_standard WHERE idmagasin = " + magasin + " ORDER BY nom_bouquet ;";
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            string disponible = "";
            int i = 1;
            while (reader.Read())
            {
                Bouquet bouquet = new Bouquet();
                bouquet.NomBouquet = (string)reader["nom_bouquet"];
                bouquet.Composition = (string)reader["composition"];
                bouquet.PrixBouquet = (double)reader["prix_bouquet"];
                bouquet.Occasion = (string)reader["occasion"];
                bouquet.Quantite = (int)reader["quantite"];
                bouquets.Add(bouquet);
            }
            reader.Close();

            foreach (Bouquet bouquet in bouquets)
            {
                if (bouquet.Quantite == 0)
                {
                    disponible = "Rupture de stock";
                    Console.Write(i+" : "+bouquet.NomBouquet + ", " + bouquet.Composition + ", " + bouquet.Occasion + ", " + bouquet.PrixBouquet + " euros, ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(disponible);
                    Console.ResetColor();
                }
                else
                {
                    disponible = "En stock";
                    Console.Write(i+" : "+bouquet.NomBouquet + ", " + bouquet.Composition + ", " + bouquet.Occasion + ", " + bouquet.PrixBouquet + " euros, ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(disponible);
                    Console.ResetColor();
                }
                i++;
            }
        }
        public static void MettreAJourQuantite(MySqlConnection connection,Bouquet bouquet, int magasin, float nombre)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE bouquet_standard SET quantite = quantite + "+nombre+" WHERE nom_bouquet = @NomBouquet AND idmagasin = "+magasin+";";
            command.Parameters.AddWithValue("@NomBouquet", bouquet.NomBouquet);
            command.ExecuteNonQuery();

            // Mettre à jour la quantité de l'objet Bouquet également
            bouquet.Quantite = bouquet.Quantite +(int)nombre;
        }
    }    
}
