using Radzen;
using System;
using System.Web;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using SelectStoredProcedureRadzen.Data;

namespace SelectStoredProcedureRadzen
{
    public partial class ConDataService
    {
        private readonly ConDataContext context;
        private readonly NavigationManager navigationManager;

        public ConDataService(ConDataContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public async Task ExportMarketsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/markets/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')") : $"export/condata/markets/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')", true);
        }

        public async Task ExportMarketsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/markets/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')") : $"export/condata/markets/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')", true);
        }

        partial void OnMarketsRead(ref IQueryable<Models.ConData.Market> items);

        public async Task<IQueryable<Models.ConData.Market>> GetMarkets(Query query = null)
        {
            var items = context.Markets.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    items = items.Where(query.Filter);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnMarketsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnMarketCreated(Models.ConData.Market item);

        public async Task<Models.ConData.Market> CreateMarket(Models.ConData.Market market)
        {
            OnMarketCreated(market);

            context.Markets.Add(market);
            context.SaveChanges();

            return market;
        }
      public async Task<int> MuyikInsertNewMarkets(string MarketLocation, string MarketName, float? MarketSizeInHectares)
      {
          OnMuyikInsertNewMarketsDefaultParams(ref MarketLocation, ref MarketName, ref MarketSizeInHectares);

          SqlParameter[] @params =
          {
              new SqlParameter("@returnVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
              new SqlParameter("@MarketLocation", SqlDbType.VarChar) {Direction = ParameterDirection.Input, Value = MarketLocation},
              new SqlParameter("@MarketName", SqlDbType.VarChar) {Direction = ParameterDirection.Input, Value = MarketName},
              new SqlParameter("@MarketSizeInHectares", SqlDbType.Decimal) {Direction = ParameterDirection.Input, Value = MarketSizeInHectares},
          };
          context.Database.ExecuteSqlRaw("EXEC @returnVal=[dbo].[MuyikInsertNewMarket] @MarketLocation, @MarketName, @MarketSizeInHectares", @params);

          int result = Convert.ToInt32(@params[0].Value);

          OnMuyikInsertNewMarketsInvoke(ref result);

          return await Task.FromResult(result);
      }

      partial void OnMuyikInsertNewMarketsDefaultParams(ref string MarketLocation, ref string MarketName, ref float? MarketSizeInHectares);
      partial void OnMuyikInsertNewMarketsInvoke(ref int result);

      public async Task ExportSelectAllMarketsToExcel(Query query = null, string fileName = null)
      {
          navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/selectallmarkets/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')") : $"export/condata/selectallmarkets/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')", true);
      }

      public async Task ExportSelectAllMarketsToCSV(Query query = null, string fileName = null)
      {
          navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/selectallmarkets/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')") : $"export/condata/selectallmarkets/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? fileName : "Export")}')", true);
      }

      public async Task<IQueryable<Models.ConData.SelectAllMarket>> GetSelectAllMarkets()
      {
          var items = context.SelectAllMarkets.FromSqlRaw("EXEC [dbo].[SelectAllMarkets]").ToList().AsQueryable();

          OnSelectAllMarketsInvoke(ref items);

          return await Task.FromResult(items);
      }

      partial void OnSelectAllMarketsInvoke(ref IQueryable<Models.ConData.SelectAllMarket> items);

        partial void OnMarketDeleted(Models.ConData.Market item);

        public async Task<Models.ConData.Market> DeleteMarket(int? marketListId)
        {
            var item = context.Markets
                              .Where(i => i.MarketListID == marketListId)
                              .FirstOrDefault();

            if (item == null)
            {
               throw new Exception("Item no longer available");
            }

            OnMarketDeleted(item);

            context.Markets.Remove(item);

            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                context.Entry(item).State = EntityState.Unchanged;
                throw ex;
            }

            return item;
        }

        partial void OnMarketGet(Models.ConData.Market item);

        public async Task<Models.ConData.Market> GetMarketByMarketListId(int? marketListId)
        {
            var items = context.Markets
                              .AsNoTracking()
                              .Where(i => i.MarketListID == marketListId);

            var item = items.FirstOrDefault();

            OnMarketGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ConData.Market> CancelMarketChanges(Models.ConData.Market item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnMarketUpdated(Models.ConData.Market item);

        public async Task<Models.ConData.Market> UpdateMarket(int? marketListId, Models.ConData.Market market)
        {
            OnMarketUpdated(market);

            var item = context.Markets
                              .Where(i => i.MarketListID == marketListId)
                              .FirstOrDefault();
            if (item == null)
            {
               throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(market);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return market;
        }
    }
}
