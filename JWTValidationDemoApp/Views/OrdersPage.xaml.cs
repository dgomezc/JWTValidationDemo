using System;

using JWTValidationDemoApp.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JWTValidationDemoApp.Views
{
    public sealed partial class OrdersPage : Page
    {
        public OrdersViewModel ViewModel { get; } = new OrdersViewModel();

        public OrdersPage()
        {
            InitializeComponent();
            Loaded += OrdersPage_Loaded;
        }

        private async void OrdersPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync(MasterDetailsViewControl.ViewState);
        }
    }
}
