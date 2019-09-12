using System;
using System.Collections.Generic;
using System.Text;
using Prism.Mvvm;
using SQLite;

namespace PriceCalculator.Data
{
    public class Entity : BindableBase
    {

        private string id;
        [Column("id"), PrimaryKey, AutoIncrement]
        public string Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }
    }
}
