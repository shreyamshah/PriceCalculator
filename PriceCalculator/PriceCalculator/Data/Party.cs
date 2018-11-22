using System;
using System.Collections.Generic;
using System.Text;
using Prism.Mvvm;
using SQLite;

namespace PriceCalculator.Data
{
    [Table("party")]
    public class Party : BindableBase
    {
        private int id;
        [Column("id"),PrimaryKey,AutoIncrement]
        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private string name;
        [Column("name")]
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private int profitPercent;
        [Column("profitPerecent")]
        public int ProfitPercent
        {
            get { return profitPercent; }
            set { SetProperty(ref profitPercent, value); }
        }
    }
}
