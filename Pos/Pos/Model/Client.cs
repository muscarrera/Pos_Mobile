using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pos.Model
{
   public  class Client
    {
        [PrimaryKey, AutoIncrement]
         public int Clid { get; set; }
        [MaxLength(250)]
        public string ClientName { get; set; }
        [MaxLength(250)]
        public string Info { get; set; }


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
