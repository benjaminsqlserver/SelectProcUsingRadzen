using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace SelectStoredProcedureRadzen
{
    public partial class ConDataService
    {
        //custom method to return all markets

        public async Task<IQueryable<Models.ConData.Market>> MuyikReturnAllMarkets(string connectionString, Radzen.Query query = null)
        {
            var items = new List<Models.ConData.Market>();
            var items1 = items.AsQueryable();

            var connection = new SqlConnection(connectionString);

            try
            {
                var command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "dbo.SelectAllMarkets";
                command.CommandTimeout = 100000;
                command.CommandType = CommandType.StoredProcedure;
                await connection.OpenAsync();
                var dr = await command.ExecuteReaderAsync();
                DataTable dt = new DataTable();
                dt.Load(dr);
                await dr.CloseAsync();
                if(dt.Rows.Count > 0)
                {
                    foreach(DataRow row in dt.Rows)
                    {
                        decimal? marketSizeInHectares = null;

                        decimal marketSizeConverter;

                        if(decimal.TryParse(row["MarketSizeInHectares"].ToString(),out marketSizeConverter ))
                        {
                            marketSizeInHectares = marketSizeConverter;
                        }
                            
                        items.Add(new Models.ConData.Market { MarketListID = Convert.ToInt32(row["MarketListID"]), MarketLocation= Convert.ToString(row["MarketLocation"]), MarketName= Convert.ToString(row["MarketName"]), MarketSizeInHectares=marketSizeInHectares});
                    }

                     items1 = items.AsQueryable();

                    if (query != null)
                    {
                        if (!string.IsNullOrEmpty(query.Filter))
                        {
                            items1 = items1.Where(query.Filter);
                        }

                        if (!string.IsNullOrEmpty(query.OrderBy))
                        {
                            items1 = items1.OrderBy(query.OrderBy);
                        }

                        if (!string.IsNullOrEmpty(query.Expand))
                        {
                            var propertiesToExpand = query.Expand.Split(',');
                            foreach (var p in propertiesToExpand)
                            {
                                items1 = items1.Include(p);
                            }
                        }

                        if (query.Skip.HasValue)
                        {
                            items1 = items1.Skip(query.Skip.Value);
                        }

                        if (query.Top.HasValue)
                        {
                            items1 = items1.Take(query.Top.Value);
                        }
                    }

                    OnMarketsRead(ref items1);


                }

                
            }
            catch(Exception)
            {

            }
            finally
            {
               await connection.CloseAsync();
            }




            return items1;



        }
    }
}
