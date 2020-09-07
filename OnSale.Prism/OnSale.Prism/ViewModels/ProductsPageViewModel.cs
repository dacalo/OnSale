using Newtonsoft.Json;
using OnSale.Common.Business;
using OnSale.Common.Entities;
using OnSale.Common.Helpers;
using OnSale.Common.Models;
using OnSale.Common.Responses;
using OnSale.Common.Services;
using OnSale.Prism.Helpers;
using OnSale.Prism.ItemViewModels;
using OnSale.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace OnSale.Prism.ViewModels
{
    public class ProductsPageViewModel : ViewModelBase
    {
        #region [ Attributes ]
        private bool _isBusy;
        private bool _isRefreshing;
        private bool _isRunning;
        private string _search; 
        private int _cartNumber;
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private DelegateCommand _searchCommand;
        private List<ProductResponse> _myProducts;
        private ObservableCollection<ProductItemViewModel> _products;
        private DelegateCommand _showCartCommand;
        #endregion [ Attributes ]

        #region [ Constructor ]
        public ProductsPageViewModel(INavigationService navigationService, IApiService apiService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.Products;
            LoadSkeleton();
            LoadCartNumber();
            _ = LoadProductsAsync();
        }
        #endregion [ Constructor ]

        #region [ Properties ]
        public ObservableCollection<ProductItemViewModel> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public string Search
        {
            get => _search;
            set
            {
                SetProperty(ref _search, value);
                ShowProducts();
            }
        }

        public int CartNumber
        {
            get => _cartNumber;
            set => SetProperty(ref _cartNumber, value);
        }
        #endregion [ Properties ]

        #region [ Commands ]
        public DelegateCommand RefreshCommand => new DelegateCommand(async () => await Refresh());

        public DelegateCommand SearchCommand => _searchCommand ?? (_searchCommand = new DelegateCommand(ShowProducts));

        public DelegateCommand ShowCartCommand => _showCartCommand ?? (_showCartCommand = new DelegateCommand(ShowCartAsync));

        #endregion [ Commands ]

        #region [ Methods ]
        private void LoadSkeleton()
        {
            List<ProductItemViewModel> list = new List<ProductItemViewModel>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new ProductItemViewModel(_navigationService)
                {
                    Id = 0,
                    Name = string.Empty,
                    ProductImages = new List<ProductImage> { new ProductImage { UrlImage = Constants.TextString.GuidImageEmpty } }
                });
            }
            Products = new ObservableCollection<ProductItemViewModel>(list);
        }

        private async Task LoadProductsAsync()
        {
            IsBusy = true;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }

            IsRunning = true;
            Response response = await _apiService.GetListAsync<ProductResponse>(
                Constants.URL_BASE,
                Constants.SERVICE_PREFIX,
                Constants.EndPoints.GetProducts);
            IsRunning = false;
            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            _myProducts = (List<ProductResponse>)response.Result;
            ShowProducts();
            IsBusy = false;
        }

        private async Task Refresh()
        {
            IsRefreshing = true;
            await LoadProductsAsync();
            IsRefreshing = false;
        }

        private void ShowProducts()
        {
            if (string.IsNullOrEmpty(Search))
            {
                Products = new ObservableCollection<ProductItemViewModel>(_myProducts.Select(p => new ProductItemViewModel(_navigationService)
                {
                    Category = p.Category,
                    Description = p.Description,
                    Id = p.Id,
                    IsActive = p.IsActive,
                    IsStarred = p.IsStarred,
                    Name = p.Name,
                    Price = p.Price,
                    ProductImages = p.ProductImages,
                    Qualifications = p.Qualifications
                }).ToList());
            }
            else
            {
                Products = new ObservableCollection<ProductItemViewModel>(_myProducts.Select(p => new ProductItemViewModel(_navigationService)
                {
                    Category = p.Category,
                    Description = p.Description,
                    Id = p.Id,
                    IsActive = p.IsActive,
                    IsStarred = p.IsStarred,
                    Name = p.Name,
                    Price = p.Price,
                    ProductImages = p.ProductImages,
                    Qualifications = p.Qualifications
                }).Where(p => p.Name.ToLower().Contains(Search.ToLower())));
            }
        }

        private void LoadCartNumber()
        {
            List<OrderDetailResponse> orderDetails = JsonConvert.DeserializeObject<List<OrderDetailResponse>>(Settings.OrderDetails);
            if (orderDetails == null)
            {
                orderDetails = new List<OrderDetailResponse>();
                Settings.OrderDetails = JsonConvert.SerializeObject(orderDetails);
            }

            CartNumber = orderDetails.Count;
        }

        private async void ShowCartAsync()
        {
            await _navigationService.NavigateAsync(nameof(ShowCarPage));
        }

        #endregion [ Methods ]

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
        }
    }
}
