using System;
using System.Collections.Generic;
using System.Text;

namespace Pos.Model
{
    public class FullCommand
    {
        public Facture Commande { get; set; }
        public List<Product> Products { get; set; }
    }
}
