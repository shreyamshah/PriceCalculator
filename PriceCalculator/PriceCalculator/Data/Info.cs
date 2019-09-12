using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace PriceCalculator.Data
{
    [Table("info")]
    public class Info<T> where T:class
    {
        public string key { get; set; }
        public T value { get; set; }
    }
}
