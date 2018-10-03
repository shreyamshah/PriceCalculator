using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using PriceCalculator.Data;

namespace PriceCalculator.Helper
{
    public class DatabaseHelper
    {
        public List<Product> GetAllProducts()
        {
            if (App.Connection != null)
            {
                string query = "select * from product";
                try
                {
                    return App.Connection.Query<Product>(query);
                }
                catch ( Exception ex )
                {
                    throw;
                }
            }
            return null;
        }
        public int SaveProduct(Product product)
        {
            try
            {
                if (App.Connection != null)
                {
                    return App.Connection.Insert(product);
                }
            }
            catch(Exception ex)
            {

            }
            return -1;
        }

        public int SaveItem(Item item)
        {
            try
            {
                if (App.Connection != null)
                {
                    int add = App.Connection.Insert(item);
                    return add;
                }
            }
            catch (Exception ex)
            {

            }
            return -1;
        }

        public List<Item> GetAllItems()
        {
            if (App.Connection != null)
            {
                string query = "select * from item";
                try
                {
                    List<Item> items = App.Connection.Query<Item>(query);
                    return items;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return null;
        }
    }
}
