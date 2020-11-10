using Newtonsoft.Json;
using OnSale.Common.Business;
using OnSale.Common.Helpers;
using OnSale.Common.Responses;
using OnSale.Common.Services;
using OnSale.Prism.Helpers;
using OnSale.Prism.ItemViewModels;
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
    public class ShowHistoryPageViewModel : ViewModelBase
    {
        #region [ Attributes ]
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ObservableCollection<OrderItemViewModel> _orders;
        private bool _isRunning;
        private string _search;
        private int _cartNumber;
        private List<OrderResponse> _myOrders;
        private DelegateCommand _searchCommand;
        #endregion [ Attributes ]

        #region [ Constructor ]
        public ShowHistoryPageViewModel(INavigationService navigationService, IApiService apiService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.ShowPurchaseHistory;
            LoadOrdersAsync();
        }
        #endregion [ Constructor ]


        #region [ Properties ]
        public string Search
        {
            get => _search;
            set
            {
                SetProperty(ref _search, value);
                ShowOrders();
            }
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public ObservableCollection<OrderItemViewModel> Orders
        {
            get => _orders;
            set => SetProperty(ref _orders, value);
        }
        #endregion [ Properties ]

        #region [ Commands ]
        public DelegateCommand SearchCommand => _searchCommand ?? (_searchCommand = new DelegateCommand(ShowOrders));
        #endregion [ Commands ]

        #region [ Methods ]
        private async void LoadOrdersAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }

            IsRunning = true;
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            Response response = await _apiService.GetListAsync<OrderResponse>(Constants.URL_BASE, Constants.SERVICE_PREFIX, Constants.EndPoints.PostOrders, token.Token);
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            _myOrders = (List<OrderResponse>)response.Result;
            ShowOrders();
        }

        private void ShowOrders()
        {
            if (_myOrders == null)
                return;

            if (string.IsNullOrEmpty(Search))
            {
                Orders = new ObservableCollection<OrderItemViewModel>(_myOrders.Select(o => new OrderItemViewModel(_navigationService)
                {
                    Date = o.Date,
                    DateConfirmed = o.DateConfirmed,
                    DateSent = o.DateSent,
                    Id = o.Id,
                    OrderDetails = o.OrderDetails,
                    OrderStatus = o.OrderStatus,
                    PaymentMethod = o.PaymentMethod,
                    Remarks = o.Remarks,
                    User = o.User
                })
                    .OrderByDescending(o => o.Date)
                    .ToList());
            }
            else
            {
                Orders = new ObservableCollection<OrderItemViewModel>(_myOrders.Select(o => new OrderItemViewModel(_navigationService)
                {
                    Date = o.Date,
                    DateConfirmed = o.DateConfirmed,
                    DateSent = o.DateSent,
                    Id = o.Id,
                    OrderDetails = o.OrderDetails,
                    OrderStatus = o.OrderStatus,
                    PaymentMethod = o.PaymentMethod,
                    Remarks = o.Remarks,
                    User = o.User
                })
                    .Where(o => o.Value.ToString().Contains(Search))
                    .OrderByDescending(o => o.Date)
                    .ToList());
            }
        }
        #endregion [ Methods ]
    }

}
