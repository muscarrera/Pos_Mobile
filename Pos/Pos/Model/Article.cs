
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace Pos.Model
{
   public class Article
    {
        [PrimaryKey, AutoIncrement]
        public int Arid { get; set; }
        public string  Cid { get; set; }
        [MaxLength(250)]
        public string ArtName { get; set; }
        [MaxLength(250)] 
        public string ArtRef { get; set; }
        [MaxLength(250)]
        public string Unit { get; set; }
        public double Price { get; set; }
        [MaxLength(250)] 
        public string ImgName { get; set; }

       [Ignore]
        public string ImagePath
        {
            get { return Path.Combine(App.imgPath, ImgName); }
        }

        [Ignore]
        public string PriceText
        {
            get { return string.Format("{0:F2} Dh/" + Unit, Price); }
        }


        public static bool AddNew(Article art)
        {
             using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Article>();
                int i = con.Insert(art);

                if (i > 0)
                   return true;
            }
            return false;
        }
        public static bool Delete(Article art)
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {
                con.CreateTable<Article>();
                int i = con.Delete(art);

                if (i > 0)
                    return true;
            }
            return false;
        }
        public static bool Edit(Article art)
        {
            using (SQLiteConnection con = new SQLiteConnection(App.dbPath))
            {               
                con.CreateTable<Article>();
                int i = con.Update(art);

                if (i > 0)
                    return true;
            }
            return false;
        }
    }
}
