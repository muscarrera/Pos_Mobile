using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pos.Model
{
    public class Category
    {
       [PrimaryKey, AutoIncrement]
        public int cid { get; set; }
        [MaxLength(250)]
        public string name { get; set; }
        [MaxLength(250)]
        public string img { get; set; }
        public int? parent { get; set; }
        public decimal? remise { get; set; }

        public static bool AddNew(Category art)
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Category>();
                int i = con.Insert(art);

                if (i > 0)
                    return true;
            }
            return false;
        }
        public static bool Delete(Category art)
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Category>();
                int i = con.Delete(art);

                if (i > 0)
                    return true;
            }
            return false;
        }
        public static bool Edit(Category art)
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Category>();
                int i = con.Update(art);

                if (i > 0)
                    return true;
            }
            return false;
        }
    }
}
