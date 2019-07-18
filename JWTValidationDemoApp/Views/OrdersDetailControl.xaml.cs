using System;

using JWTValidationDemoApp.Core.Models;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JWTValidationDemoApp.Views
{
    public sealed partial class OrdersDetailControl : UserControl
    {
        public SampleOrder MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as SampleOrder; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(SampleOrder), typeof(OrdersDetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));

        public OrdersDetailControl()
        {
            InitializeComponent();
        }

        private static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as OrdersDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
