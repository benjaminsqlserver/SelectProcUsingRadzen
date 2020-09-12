using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectStoredProcedureRadzen.Data;

namespace SelectStoredProcedureRadzen
{
    public partial class ExportConDataController : ExportController
    {
        private readonly ConDataContext context;

        public ExportConDataController(ConDataContext context)
        {
            this.context = context;
        }
        [HttpGet("/export/ConData/markets/csv")]
        [HttpGet("/export/ConData/markets/csv(fileName='{fileName}')")]
        public FileStreamResult ExportMarketsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(context.Markets, Request.Query), fileName);
        }

        [HttpGet("/export/ConData/markets/excel")]
        [HttpGet("/export/ConData/markets/excel(fileName='{fileName}')")]
        public FileStreamResult ExportMarketsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(context.Markets, Request.Query), fileName);
        }

        [HttpGet("/export/ConData/selectallmarkets/csv")]
        [HttpGet("/export/ConData/selectallmarkets/csv(fileName='{fileName}')")]
        public FileStreamResult ExportSelectAllMarketsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(context.SelectAllMarkets, Request.Query), fileName);
        }

        [HttpGet("/export/ConData/selectallmarkets/excel")]
        [HttpGet("/export/ConData/selectallmarkets/excel(fileName='{fileName}')")]
        public FileStreamResult ExportSelectAllMarketsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(context.SelectAllMarkets, Request.Query), fileName);
        }            
    }
}
