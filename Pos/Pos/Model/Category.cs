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
        public string catName { get; set; }
    }
}
