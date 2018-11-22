using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace PriceCalculator.Data
{
    [Table("info")]
    public class Info
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}
