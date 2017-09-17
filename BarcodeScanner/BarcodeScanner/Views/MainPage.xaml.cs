using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarcodeReaderAbstractions;
using Xamarin.Forms;

namespace BarcodeScanner
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as ViewModels.MainPageViewModel)?.Initialize();
        }

        protected override void OnDisappearing()
        {
            // TODO: Figure out why this event doesn't seem to fire

            (BindingContext as ViewModels.MainPageViewModel)?.Stop();
            base.OnDisappearing();
        }
    }
}
