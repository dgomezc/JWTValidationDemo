using JWTValidationDemoApp.Core.Helpers;
using JWTValidationDemoApp.Core.Models;
using JWTValidationDemoApp.Core.Services;
using JWTValidationDemoApp.Helpers;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace JWTValidationDemoApp.ViewModels
{
    public class OrdersViewModel : Observable
    {
        private IdentityService IdentityService => Singleton<IdentityService>.Instance;
        private HttpDataService _httpDataService = new HttpDataService("http://localhost:53849/api");
        private SampleOrder _selected;

        public SampleOrder Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public StatusViewModel Status { get; set; } = new StatusViewModel();

        public OrdersViewModel()
        {
            IdentityService.LoggedIn += async (s,e) => await LoadDataAsync(MasterDetailsViewState.Both);
            IdentityService.LoggedOut += async (s, e) => await LoadDataAsync(MasterDetailsViewState.Both);
        }

        public ObservableCollection<SampleOrder> SampleItems { get; } = new ObservableCollection<SampleOrder>();
        
        public async Task LoadDataAsync(MasterDetailsViewState viewState)
        {
            Status.ShowPanel = true;
            Status.ShowProgressBar = true;

            SampleItems.Clear();

            try
            {
                Status.Message = "Loading orders";
                var data = await GetDataAsync(viewState);
                Status.ShowPanel = false;

            }
            catch (Exception ex)
            {
                Status.Message = $"Error when loading orders: {ex.Message}";
            }

            Status.ShowProgressBar = false;
        }

        private async Task<IEnumerable<SampleOrder>> GetDataAsync(MasterDetailsViewState viewState)
        {
            var accessToken = await IdentityService.GetTokenSilentToMyClientAsync();
            var data = await _httpDataService.GetAsync<IEnumerable<SampleOrder>>("orders", accessToken);

            foreach (var item in data)
            {
                SampleItems.Add(item);
            }

            if (viewState == MasterDetailsViewState.Both)
            {
                Selected = SampleItems.First();
            }

            return data;
        }        
    }
}
