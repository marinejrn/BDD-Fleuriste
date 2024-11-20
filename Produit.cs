using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace projet

{
    public class Produit
    {
        public string NomProduit { get; set; }
        public float Prix { get; set; }
        public int Stock { get; set; }
        public Produit()
        {
        }
        public Produit(string nom,float prix,int stock)
        {
            NomProduit = nom;
            Prix = prix;
            Stock = stock;
        }
        public static void MettreAJourQuantite(MySqlConnection connection, Produit produit,int magasin,float nombre)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE produit SET stock = stock + "+nombre+" WHERE nom_produit = @NomProduit AND idmagasin="+magasin+";";
            command.Parameters.AddWithValue("@NomProduit", produit.NomProduit);
            command.ExecuteNonQuery();

            // Mettre à jour la quantité de l'objet  également
            produit.Stock = produit.Stock + (int)nombre;
        }
    }
}
