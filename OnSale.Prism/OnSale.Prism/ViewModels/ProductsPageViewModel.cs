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
using System.Threading.Tasks;
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
                    Name = "",
                    ProductImages = new List<ProductImage> { new ProductImage { ImageId = Guid.Parse("d844c6c4-c929-4518-abeb-e900ac95ac53") } }
                });
            }
            Products = new ObservableCollection<Product>(list);
        }

        private async void LoadProductsAsync()
        {
            IsBusy = true;
            await Task.Delay(5000);
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
            IsBusy = false;
        }
    }
}
