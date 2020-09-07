using Newtonsoft.Json;
using OnSale.Common.Entities;
using OnSale.Common.Helpers;
using OnSale.Common.Models;
using OnSale.Common.Responses;
using OnSale.Prism.Helpers;
using OnSale.Prism.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace OnSale.Prism.ViewModels
{
    public class AddToCartPageViewModel : ViewModelBase
    {
        #region [ Attributes ]
        private readonly INavigationService _navigationService;
        private ProductResponse _product;
        private ObservableCollection<ProductImage> _images;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _addToCartCommand;
        #endregion[ Attributes ]

        #region [ Constructor ]
        public AddToCartPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            Title = Languages.AddCart;
            IsEnabled = true;
            Quantity = 1;
        }
        #endregion[ Constructor ]

        #region [ Properties ]
        public float Quantity { get; set; }

        public string Remarks { get; set; }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public ObservableCollection<ProductImage> Images
        {
            get => _images;
            set => SetProperty(ref _images, value);
        }

        public ProductResponse Product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }
        #endregion[ Properties ]

        #region [ Commands]
        public DelegateCommand AddToCartCommand => _addToCartCommand ?? (_addToCartCommand = new DelegateCommand(AddToCartAsync));
        #endregion[ Commands]
        #region [ Methods ]
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("product"))
            {
                Product = parameters.GetValue<ProductResponse>("product");
                Images = new ObservableCollection<ProductImage>(Product.ProductImages);
            }
        }

        private async void AddToCartAsync()
        {
            bool isValid = await ValidateDataAsync();
            if (!isValid)
                return;

            List<OrderDetail> orderDetails = JsonConvert.DeserializeObject<List<OrderDetail>>(Settings.OrderDetails);
            if (orderDetails == null)
                orderDetails = new List<OrderDetail>();

            orderDetails.Add(new OrderDetail
            {
                Product = Product,
                Quantity = Quantity,
                Remarks = Remarks
            });

            Settings.OrderDetails = JsonConvert.SerializeObject(orderDetails);
            await App.Current.MainPage.DisplayAlert(Languages.Ok, Languages.AddToCartMessage, Languages.Accept);
            await _navigationService.NavigateAsync($"/{nameof(OnSaleMasterDetailPage)}/NavigationPage/{nameof(ProductsPage)}");
        }

        private async Task<bool> ValidateDataAsync()
        {
            if (Quantity == 0)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.QuantityError, Languages.Accept);
                return false;
            }

            return true;
        }

        #endregion[ Methods ]
    }
}
