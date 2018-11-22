using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using PriceCalculator.Data;

namespace PriceCalculator.Helper
{
    public class DatabaseHelper
    {
        public List<Product> GetAllProducts(string category="")
        {
            if (App.Connection != null)
            {
                string query = $"select * from product where category='{category}'";
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

        public List<Item> GetAllItems( string category )
        {
            if (App.Connection != null)
            {
                string query = $"select * from item where categoryId = '{category}'";
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

        public int SaveCategory(Category category)
        {
            try
            {
                if (App.Connection != null)
                {
                    int add = App.Connection.Insert(category);
                    return add;
                }
            }
            catch (Exception ex)
            {

            }
            return -1;
        }

        public List<Category> GetAllCategory()
        {
            if (App.Connection != null)
            {
                string query = "select * from category";
                try
                {
                    List<Category> categories = App.Connection.Query<Category>(query);
                    return categories;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return null;
        }

        public int SaveParty(Party party)
        {
            try
            {
                if (App.Connection != null)
                {
                    int add = App.Connection.Insert(party);
                    return add;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return -1;
        }

        public List<Party> GetAllParty()
        {
            if (App.Connection != null)
            {
                string query = "select * from party";
                try
                {
                    List<Party> parties = App.Connection.Query<Party>(query);
                    return parties;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return null;
        }

        public void ExecuteQuery(string query)
        {
            if(App.Connection != null)
            {
                try
                {
                    App.Connection.ExecuteScalar<bool>(query);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<Info> GetScriptsLoaded(string value="script")
        {
            if(App.Connection!=null)
            {
                try
                {
                    List<Info> info = App.Connection.Query<Info>($"select * from info where value = '{value}'");
                    return info;
                }
                catch (Exception ex)
                {
                    
                }
            }
            return new List<Info>();
        }

        public void SaveInfo(string key,string value)
        {
            if(App.Connection!=null)
            {
                try
                {
                    App.Connection.Execute("insert into info values (?,?)", key, value);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public int GetTableCount()
        {
            if (App.Connection != null)
            {
                return App.Connection.ExecuteScalar<int>("select count(*) from sqlite_master");
            }
            else
                return 0;
        }

    }
}
