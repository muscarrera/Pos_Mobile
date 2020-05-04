using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pos.Model
{
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Arid { get; set; }
        public int Fid { get; set; }
        public string Cid { get; set; }
        [MaxLength(250)]
        public string ArtName { get; set; }
        [MaxLength(250)]
        public string ArtRef { get; set; }
        [MaxLength(250)]
        public string ImgName { get; set; }
        public double Price { get; set; }
        public double Qte { get; set; }
     

        [Ignore]
        public string ImagePath
        {
            get { return Path.Combine(App.dbPath, ImgName); }
        }

        [Ignore]
        public Article article
        {
            set {
                Arid = value.Arid;
                ArtName = value.ArtName;
                ArtRef = value.ArtRef;
                Price = value.Price;
                Cid = value.Cid;
                ImgName = value.ImgName;        
            }
        }


        public static bool AddNew(Product art)
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Product>();
                int i = con.Insert(art);

                if (i > 0)
                    return true;
            }
            return false;
        }
        public static bool Delete(Product art)
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Product>();
                int i = con.Delete(art);

                if (i > 0)
                    return true;
            }
            return false;
        }
        public static bool Edit(Product art)
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Product>();
                int i = con.Update(art);

                if (i > 0)
                    return true;
            }
            return false;
        }
    }
}
