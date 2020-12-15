using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Model
{
    public class Facture
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int cid { get; set; }
        public int commercialID { get; set; }
        public int compteId { get; set; }
        public int pj { get; set; }
        [MaxLength(250)]
        public string name { get; set; }
        [MaxLength(250)]
        public string writer { get; set; }
        public DateTime date { get; set; }
        public double Total { get; set; }
        public double Avance { get; set; }
        public double tva { get; set; }
        public double remise { get; set; }
        [MaxLength(250)]
        public string isAdmin { get; set; }
        public bool isValid { get; set; }
        public int droitTimbre { get; set; }
        [MaxLength(250)]
        public string modePayement { get; set; }

        [MaxLength(250)]
        public string driver { get; set; }
        [MaxLength(250)]
        public string Devis { get; set; }
        [MaxLength(250)]
        public string Commande_Client { get; set; }
        [MaxLength(250)]
        public string Bon_Commande { get; set; }
        [MaxLength(250)]
        public string Bon_Livraison { get; set; }
        
        [Ignore]
        public bool IsPayed  {   get => Avance >= Total;  }
        [Ignore]
        public List<Product> items { get; set; }



        public static bool AddNew(Facture art)
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Facture>();
                int i = con.Insert(art);

                if (i > 0)
                    return true;
            }
            return false;
        }
        public static bool Delete(Facture art)
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Facture>();
                int i = con.Delete(art);

                if (i > 0)
                    return true;
            }
            return false;
        }
        public static int Edit(Facture art)
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Facture>();
                return con.Update(art);

            }
        
        }
    }
}
