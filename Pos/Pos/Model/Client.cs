using SQLite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Pos.Model
{
   public  class Client
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int Clid { get; set; }
        [MaxLength(250)]
        public string name { get; set; }
        [MaxLength(250)]
        public string @ref { get; set; }
        [MaxLength(250)]
        public string groupe { get; set; }
        public bool isCompany { get; set; }
        [MaxLength(250)]
        public string adresse { get; set; }
        [MaxLength(250)]
        public string cp { get; set; }
        [MaxLength(250)]
        public string ville { get; set; }
        [MaxLength(250)]
        public string ice { get; set; }
        [MaxLength(250)]
        public string tel { get; set; }
        [MaxLength(250)]
        public string gsm { get; set; }
        [MaxLength(250)]
        public string email { get; set; }
        [MaxLength(250)]
        public string info { get; set; }
        [MaxLength(250)]
        public string img { get; set; }
        [MaxLength(250)]
        public string responsable { get; set; }
        public decimal  porte_Monie { get; set; }
        public bool isBlocked { get; set; }
        [MaxLength(250)]
        public string ModePayement { get; set; }
        public decimal plafond { get; set; }

        [Ignore]
        public Color clColor
        {
            get {
                Color ss = Color.Purple;
                if (Clid == 0) {ss= Color.Red;}

                return ss; }
        }
        public static bool AddNew(Client art)
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Client>();
                int i = con.Insert(art);

                if (i > 0)
                    return true;
            }
            return false;
        }
        public static bool Delete(Client art)
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Client>();
                int i = con.Delete(art);

                if (i > 0)
                    return true;
            }
            return false;
        }
        public static bool Edit(Client art)
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Client>();
                int i = con.Update(art);

                if (i > 0)
                    return true;
            }
            return false;
        }
    }
}
