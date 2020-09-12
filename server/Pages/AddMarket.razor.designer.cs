using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using SelectStoredProcedureRadzen.Models.ConData;
using Microsoft.EntityFrameworkCore;

namespace SelectStoredProcedureRadzen.Pages
{
    public partial class AddMarketComponent : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected ConDataService ConData { get; set; }

        SelectStoredProcedureRadzen.Models.ConData.Market _market;
        protected SelectStoredProcedureRadzen.Models.ConData.Market market
        {
            get
            {
                return _market;
            }
            set
            {
                if (!object.Equals(_market, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "market", NewValue = value, OldValue = _market };
                    _market = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Load();
        }
        protected async System.Threading.Tasks.Task Load()
        {
            market = new SelectStoredProcedureRadzen.Models.ConData.Market(){};
        }

        protected async System.Threading.Tasks.Task Form0Submit(SelectStoredProcedureRadzen.Models.ConData.Market args)
        {
            try
            {
                var conDataCreateMarketResult = await ConData.CreateMarket(market);
                DialogService.Close(market);
            }
            catch (System.Exception conDataCreateMarketException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new Market!");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
