using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriceCalculator.Data;
using SQLite;

namespace PriceCalculator.Helper
{
    public class DatabaseHelper
    {
        public async Task<List<Product>> GetAllProducts(string category="")
        {
            if (App.Connection != null)
            {
                string query = $"select * from product";
                if (!string.IsNullOrEmpty(category))
                    query += $" where category='{category}'";
                query += " order by id";
                try
                {
                    List<Product> res = await App.Connection.QueryAsync<Product>(query);
                    //await App.Connection.CloseAsync();
                    return res;
                }
                catch ( Exception ex )
                {

                    throw;
                }
            }
            return null;
        }
        /// <summary>
        /// Saves Product
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Newly Created Id</returns>
        public async Task<int> SaveProduct(Product product)
        {
            try
            {
                if (App.Connection != null)
                {
                    int res = 0;
                    if (string.IsNullOrEmpty(product.Id))
                        res = await App.Connection.InsertAsync(product);
                    else
                        res = await App.Connection.UpdateAsync(product);
                    //await App.Connection.CloseAsync();
                    return res;
                }
            }
            catch(Exception ex)
            {

            }
            return -1;
        }

        public async Task<int> SaveItem(Item item)
        {
            try
            {
                if (App.Connection != null)
                {
                    int add = 0;
                    if (string.IsNullOrEmpty(item.Id))
                    {
                        add = await App.Connection.InsertAsync(item);
                    }
                    else
                    {
                        Item prevItem = await App.Connection.GetAsync<Item>(item.Id);
                        add = await App.Connection.UpdateAsync(item);
                        if (prevItem.Rate != item.Rate)
                        {
                            string updateItemUsed = $"update itemsUsed set price = {item.Rate}, total = quantity * {item.Rate} where itemId == {item.Id}";
                            int res = await App.Connection.ExecuteAsync(updateItemUsed);
                            if(res>0)
                            {
                                List<ItemUsed> ids = await App.Connection.QueryAsync<ItemUsed>($"select productId from itemsused where itemId == {item.Id}");
                                if (ids != null && ids.Count > 0)
                                {
                                    foreach (string id in ids.Select(e=>e.ProductId))
                                    {
                                        string productUpdate = $"update product set costPrice =  (select sum(total) from itemsUsed where productId == {id})," +
                                            $" sellingPrice = (select sum(total) from itemsUsed where productId =={id})*100/(100 - profitPercent) where id = {id}";
                                        await App.Connection.ExecuteAsync(productUpdate);
                                    }
                                }
                            }

                        }
                    }
                   // await App.Connection.CloseAsync();
                    return add;
                }
            }
            catch (Exception ex)
            {

            }
            return -1;
        }

        public async Task<List<Item>> GetAllItems( string category )
        {
            if (App.Connection != null)
            {
                string query = $"select * from item where categoryId = '{category}'";
                try
                {
                    List<Item> items = await App.Connection.QueryAsync<Item>(query);
                    //await App.Connection.CloseAsync();
                    return items;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return null;
        }

        public async Task<int> SaveCategory(Category category)
        {
            try
            {
                if (App.Connection != null)
                {
                    int add = await App.Connection.InsertAsync(category);
                    //await App.Connection.CloseAsync();
                    return add;
                }
            }
            catch (Exception ex)
            {

            }
            return -1;
        }

        public async Task<List<Category>> GetAllCategory()
        {
            if (App.Connection != null)
            {
                string query = "select * from category";
                try
                {
                    List<Category> categories = await App.Connection.QueryAsync<Category>(query);
                    //await App.Connection.CloseAsync();
                    return categories;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return null;
        }

        public async Task<int> SaveParty(Party party)
        {
            try
            {
                if (App.Connection != null)
                {
                    int add = await App.Connection.InsertAsync(party);
                    //await App.Connection.CloseAsync();
                    return add;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return -1;
        }

        public async Task<List<Party>> GetAllParty()
        {
            if (App.Connection != null)
            {
                string query = "select * from party";
                try
                {
                    List<Party> parties = await App.Connection.QueryAsync<Party>(query);
                    //await App.Connection.CloseAsync();
                    return parties;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return null;
        }

        public async Task ExecuteQuery(string query)
        {
            if(App.Connection != null)
            {
                try
                {
                    await App.Connection.ExecuteScalarAsync<bool>(query);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<List<Info>> GetScriptsLoaded(string value="script")
        {
            if(App.Connection!=null)
            {
                try
                {
                    List<Info> info = await App.Connection.QueryAsync<Info>($"select * from info where value = '{value}'");
                    //await App.Connection.CloseAsync();
                    return info;
                }
                catch (Exception ex)
                {
                    
                }
            }
            return new List<Info>();
        }

        public async Task SaveInfo(string key,string value)
        {
            if(App.Connection!=null)
            {
                try
                {
                    await App.Connection.ExecuteAsync("insert into info values (?,?)", key, value);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public async Task<int> GetTableCount()
        {
            if (App.Connection != null)
            {
                return await App.Connection.ExecuteScalarAsync<int>("select count(*) from sqlite_master");
            }
            else
                return 0;
        }

        public async Task<List<ItemUsed>> GetAllItemUsed(string productId = "")
        {
            if (App.Connection != null)
            {
                string query = $"select * from itemsUsed";
                if (!string.IsNullOrEmpty(productId))
                    query += $" where productId='{productId}'";
                try
                {
                    List<ItemUsed> res = await App.Connection.QueryAsync<ItemUsed>(query);
                    //await App.Connection.CloseAsync();
                    return res;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return null;
        }
        public async Task<int> SaveItemUsed(ItemUsed itemUsed)
        {
            try
            {
                if (App.Connection != null)
                {
                    int res = 0;
                    if (string.IsNullOrEmpty(itemUsed.Id))
                        res = await App.Connection.InsertAsync(itemUsed);
                    else
                        res = await App.Connection.UpdateAsync(itemUsed);
                    //await App.Connection.CloseAsync();
                    return res;
                }
            }
            catch (Exception ex)
            {

            }
            return -1;
        }

        public async Task<int> DeleteProduct(int id)
        {
            if (App.Connection != null)
            {
                string query = $"delete from product where id = {id}";
                await App.Connection.ExecuteAsync(query);
                string itemQuery = $"delete from itemsUsed where productId= {id}";
                return await App.Connection.ExecuteAsync(itemQuery);
            }
            return await Task.FromResult(0);
        }
    }
}
