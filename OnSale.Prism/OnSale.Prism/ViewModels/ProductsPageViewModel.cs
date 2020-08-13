using OnSale.Common.Business;
using OnSale.Common.Entities;
using OnSale.Common.Responses;
using OnSale.Common.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace OnSale.Prism.ViewModels
{
    public class ProductsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ObservableCollection<Product> _products;

        public ProductsPageViewModel(INavigationService navigationService, IApiService apiService)
            :base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Products";
            LoadProductsAsync();
        }

        public ObservableCollection<Product> Products 
        { 
            get => _products;
            set => SetProperty(ref _products, value);
        }

        private async void LoadProductsAsync()
        {
            if(Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Check the internet connection.", "Accept");
                return;
            }

            Response response = await _apiService.GetListAsync<Product>(
                Constants.URL_BASE,
                Constants.SERVICE_PREFIX,
                "/Products");
            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            List<Product> myProducts = (List<Product>)response.Result;
            Products = new ObservableCollection<Product>(myProducts);
        }
    }
}
