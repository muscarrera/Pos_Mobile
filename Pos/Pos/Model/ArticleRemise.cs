using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pos.Model
{
    public class ArticleRemise
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int arid { get; set; }
        [MaxLength(250)]
        public string name { get; set; }
        public double qte { get; set; }
        public double remise { get; set; }
        public bool isGros { get; set; }
        public DateTime date { get; set; }

    }
}
