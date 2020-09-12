using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Radzen;
using Radzen.Blazor;
using Microsoft.Extensions.Configuration;

namespace SelectStoredProcedureRadzen.Pages
{
    public partial class MarketsComponent
    {

        private async Task MuyikFetchAllMarketsADONet()
        {
            try
            {
                var myConnectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["conDataConnection"];
               getMarketsResult= await ConData.MuyikReturnAllMarkets(myConnectionString);

               
            }
            catch (Exception)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Markets Fetch Error", "An Error Occurred While Fetching The Markets", 7000);
            }
        }

    }
}
