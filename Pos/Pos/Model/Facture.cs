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
        public int Fid { get; set; }
        public int Clid { get; set; }
        [MaxLength(250)]
        public string ClientName { get; set; }
        public DateTime FctDate { get; set; }
        public double Total { get; set; }
        public double Avance { get; set; }
        [Ignore]
        public bool IsPayed  {   get => Avance >= Total;  }



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
