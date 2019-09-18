using System;
using System.Collections.Generic;
using System.Text;
using Prism.Mvvm;
using SQLite;

namespace PriceCalculator.Data
{
    public class Entity : BindableBase,IEquatable<Entity>
    {

        private string id;
        [Column("id"), PrimaryKey, AutoIncrement]
        public string Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        public bool Equals(Entity other)
        {
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data. 
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal. 
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            int hashProductName = Id == null ? 0 : Id.GetHashCode();
            return hashProductName;
        }
    }
}
