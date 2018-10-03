using System;
using System.Collections.Generic;
using System.Text;
using Prism.Mvvm;
using SQLite;

namespace PriceCalculator.Data
{
    [Table("item")]
    public class Item : BindableBase
    {
        private int id;
        [Column("id"),PrimaryKey,AutoIncrement]
        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private double rate;
        [Column("rate")]
        public double Rate
        {
            get { return rate; }
            set { SetProperty(ref rate, value); }
        }

        private string name;
        [Column("name")]
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private string unit;
        [Column("unit")]
        public string Unit
        {
            get { return unit; }
            set { SetProperty(ref unit, value); }
        }
    }
}
