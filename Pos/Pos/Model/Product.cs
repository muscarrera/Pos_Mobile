using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace Pos.Model
{
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int? arid { get; set; }
        public int fctid { get; set; }
        public int cid { get; set; }
        [MaxLength(250)]
        public string name { get; set; }
        [MaxLength(250)]
        public string @ref { get; set; }
        public double price { get; set; }
        public double bprice { get; set; }
        public double remise { get; set; }
        public double qte { get; set; }
        public double commession { get; set; }
        public double tva { get; set; }
        public double poid { get; set; }
        public int depot { get; set; }

        [MaxLength(250)]
        public string unit { get; set; }
        [MaxLength(250)]
        public string ImgName { get; set; }


        [Ignore]
        public string PriceText
        {
            get { return string.Format("{0} ({1})  x  {2:F2} Dh",qte,unit, price); }
        }
        [Ignore]
        public string Total
        {
            get { return string.Format("{0:F2}", qte * price); }
        }

        [Ignore]
        public Article article
        {
            set {
                arid = value.arid;
                cid = value.cid;

                name = value.name;
                @ref = value.@ref;
                price = value.priceGF;
                bprice = value.bprice;

                tva = value.tva;
                commession  = value.commGR;
                remise  = 0;
                poid = value.poid;
                depot = value.depot;

                qte = 1;        
                unit = "u";
                ImgName = value.img;
              
            }
        }

        [Ignore]

        public ImageSource ImagePath
        {
            get
            {

                //byte[] b = ImgName;
                //Stream ms = new MemoryStream(b);

                //return ImageSource.FromStream(() => ms);
                return null;
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
