using System;
using System.Collections.Generic;
using System.Text;

namespace Pos.Model
{
   public class Item
    {
        public int? arid { get; set; }
       public int cid { get; set; }
        public string name { get; set; }
       public string @ref { get; set; }
        public double price { get; set; }
        public double bprice { get; set; }
        public double remise { get; set; }
        public double qte { get; set; }
        public double commession { get; set; }
        public double tva { get; set; }
        public double poid { get; set; }
        public int depot { get; set; }

    }
}
