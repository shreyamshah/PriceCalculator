﻿using Xamarin.Forms;

namespace PriceCalculator.Views
{
    public partial class ItemAddPage : ContentPage
    {
        public ItemAddPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ItemName.Focus();
        }
    }
}
