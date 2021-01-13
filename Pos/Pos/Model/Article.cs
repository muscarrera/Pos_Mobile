
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
        [PrimaryKey]
        public int arid { get; set; }
        public int  cid { get; set; }
        [MaxLength(250)]
        public string name { get; set; }
        [MaxLength(250)] 
        public string @ref { get; set; }
        [MaxLength(250)]
        public string code { get; set; }
        [MaxLength(250)]
        public double bprice { get; set; }
        public double sprice { get; set; }
        public double tva { get; set; }
        public double prixPromo { get; set; }
        public double priceGF { get; set; }
        public double priceGR { get; set; }
        public double commGR { get; set; }
        public double cmmDT { get; set; }
        [MaxLength(250)]
        public string stockType { get; set; }
        public double alertStock { get; set; }
        [MaxLength(250)]
        public string periode { get; set; }
        public int depot { get; set; }
        public Boolean  isPromo { get; set; }
        public Boolean  isStocked { get; set; }

        [MaxLength(250)] 
        public string img { get; set; }
        public double CUMP { get; set; }
        public double poid { get; set; }



        //[Ignore]
       
        public ImageSource ImagePath
        {
          get { 
        //        if (img != null)
        //        {
        //            byte[] b = img;
        //            Stream ms = new MemoryStream(b);
                   
        //            return ImageSource.FromStream(() => ms);
        //        }
               return null;
           }
         }

        [Ignore]
        public string PriceText
        {
            get { return string.Format("{0:F2} Dh/" + "U", priceGF); }
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
