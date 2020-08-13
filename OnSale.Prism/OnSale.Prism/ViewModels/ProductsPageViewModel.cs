using OnSale.Common.Business;
using OnSale.Common.Entities;
using OnSale.Common.Responses;
using OnSale.Common.Services;
using OnSale.Prism.Helpers;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Title = Languages.Products;
            LoadSkeleton();
            LoadProductsAsync();
        }

        public ObservableCollection<Product> Products 
        { 
            get => _products;
            set => SetProperty(ref _products, value);
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private void LoadSkeleton()
        {
            List<Product> list = new List<Product>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new Product
                {
                    Id = 0,
                    Name = string.Empty,
                    ProductImages = new List<ProductImage> { new ProductImage { ImageId = Guid.Parse(Constants.TextString.GuidImageEmpty) } }
                });
            }
            Products = new ObservableCollection<Product>(list);
        }

        private async void LoadProductsAsync()
        {
            IsBusy = true;
            if(Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }

            Response response = await _apiService.GetListAsync<Product>(
                Constants.URL_BASE,
                Constants.SERVICE_PREFIX,
                Constants.EndPoints.GetProducts);
            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            List<Product> myProducts = (List<Product>)response.Result;
            Products = new ObservableCollection<Product>(myProducts);
            IsBusy = false;
        }
    }
}
