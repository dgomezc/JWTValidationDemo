﻿using System;

using JWTValidationDemoApp.ViewModels;

using Windows.UI.Xaml.Controls;

namespace JWTValidationDemoApp.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
