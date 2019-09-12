using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;
using Prism.Mvvm;
using SQLite;

namespace PriceCalculator.Data
{
    [Table("product")]
    public class Product : Entity
    {
        public Product()
        {
            ItemsUsed = new ObservableCollection<ItemUsed>();
        }

        private string name;
        [Column("name")]
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private string imgName;
        [Column("imgName")]
        public string ImgName
        {
            get { return imgName; }
            set { SetProperty(ref imgName, value); }
        }

        private string category;
        [Column("category")]
        public string Category
        {
            get { return category; }
            set { SetProperty(ref category, value); }
        }

        private ObservableCollection<ItemUsed> itemsUsed;
        [Ignore]
        public ObservableCollection<ItemUsed> ItemsUsed
        {
            get
            {
                return itemsUsed;
            }
            set { SetProperty(ref itemsUsed, value); }
        }

        private double costPrice;
        [Column("costPrice")]
        public double CostPrice
        {
            get { return costPrice; }
            set
            {
                SetProperty(ref costPrice, value);
                SellingPrice = costPrice * 100 / (100 - ((double)ProfitPercent));
            }
        }

        private int profitPercent;
        [Column("profitPercent")]
        public int ProfitPercent
        {
            get { return profitPercent; }
            set
            {
                SetProperty(ref profitPercent, value);
                if(profitPercent != 0)
                {
                    SellingPrice = CostPrice * 100/(100-((double)profitPercent));
                }
            }
        }

        private double sellingPrice;
        [Column("sellingPrice")]
        public double SellingPrice
        {
            get { return sellingPrice; }
            set { SetProperty(ref sellingPrice, Math.Round(value,2)); }
        }
    }

    [Table("itemsUsed")]
    public class ItemUsed : BindableBase
    {
        public ItemUsed()
        { }

        private string id;
        [Column("id"),PrimaryKey,AutoIncrement]
        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                SetProperty(ref id, value);
            }
        }

        private string productId;
        [Column("productId")]
        public string ProductId
        {
            get
            {
                return productId;
            }
            set
            {
                SetProperty(ref productId, value);
            }
        }

        private string itemId;
        [Column("itemId")]
        public string ItemId
        {
            get { return itemId; }
            set { SetProperty(ref itemId, value); }
        }

        private string type;
        [Column("type")]
        public string Type
        {
            get { return type; }
            set { SetProperty(ref type, value); }
        }

        private int quantity;
        [Column("quantity")]
        public int Quantity
        {
            get { return quantity; }
            set
            {
                SetProperty(ref quantity, value);
                Multiply();
            }
        }

        private string unit;
        [Column("unit")]
        public string Unit
        {
            get { return unit; }
            set
            {
                SetProperty(ref unit, value);
                if (unit != null)
                {
                    Multiply();
                }
            }
        }

        private double price;
        [Column("price")]
        public double Price
        {
            get { return price; }
            set { SetProperty(ref price, value); }
        }

        private double total;
        [Column("total")]
        public double Total
        {
            get { return total; }
            set { SetProperty(ref total, value); }
        }

        private Item itemSelected;
        [Ignore]
        public Item ItemSelected
        {
            get { return itemSelected; }
            set
            {
                SetProperty(ref itemSelected, value);
                if (itemSelected != null)
                {
                    ItemId = itemSelected.Id;
                    Price = itemSelected.Rate;
                    if (itemSelected.Unit.Equals("kg") && Unit != "kg")
                        Unit = "grams";
                    else
                        Unit = itemSelected.Unit;
                    Type = itemSelected.Name;
                }
            }
        }
        public void Multiply()
        {
            if (ItemSelected == null)
                return;
            if (Unit.Equals(ItemSelected.Unit))
            {
                Total = Price * Quantity;
            }
            else
            {
                Total = Price * Quantity * ConversionFactor();
            }
        }
        public double ConversionFactor()
        {
            if (ItemSelected.Unit.Equals("kg") && Unit.Equals("grams"))
            {
                return 0.001;
            }
            else
                return -1;
        }
    }

}
