using Newtonsoft.Json;
using OnSale.Common.Business;
using OnSale.Common.Enums;
using OnSale.Common.Helpers;
using OnSale.Common.Responses;
using OnSale.Common.Services;
using OnSale.Prism.Helpers;
using OnSale.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace OnSale.Prism.ViewModels
{
    public class OrderPageViewModel : ViewModelBase
    {
        #region [ Attributes ]
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private OrderResponse _order;
        private ObservableCollection<OrderDetailResponse> _orderDetails;
        private bool _isVisible;
        private DelegateCommand _updateRemarksCommand;
        private DelegateCommand _cancelOrderCommand;
        #endregion [ Attributes ]

        #region [ Constructor ]
        public OrderPageViewModel(INavigationService navigationService, IApiService apiService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.Order;
        }
        #endregion [ Constructor ]

        #region [ Properties ]
        public OrderResponse Order
        {
            get => _order;
            set => SetProperty(ref _order, value);
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        public ObservableCollection<OrderDetailResponse> OrderDetails
        {
            get => _orderDetails;
            set => SetProperty(ref _orderDetails, value);
        }
        #endregion [ Properties ]

        #region [ Commands ]
        public DelegateCommand UpdateRemarksCommand => _updateRemarksCommand ?? (_updateRemarksCommand = new DelegateCommand(UpdateRemarksAsync));

        public DelegateCommand CancelOrderCommand => _cancelOrderCommand ?? (_cancelOrderCommand = new DelegateCommand(CancelOrderAsync));
        #endregion [ Commands ]

        #region [ Methods ]
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("order"))
            {
                Order = parameters.GetValue<OrderResponse>("order");
                OrderDetails = new ObservableCollection<OrderDetailResponse>(Order.OrderDetails);
             
                if (Order.OrderStatus == OrderStatus.Pending)
                    IsVisible = true;
            }
        }

        private async void UpdateRemarksAsync()
        {
            if (string.IsNullOrEmpty(Order.Remarks))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.RemarksError, Languages.Accept);
                return;
            }

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            Response response = await _apiService.PutAsync(Constants.URL_BASE, Constants.SERVICE_PREFIX, Constants.EndPoints.PostOrders, Order, token.Token);

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            await App.Current.MainPage.DisplayAlert(Languages.Ok, Languages.OrderUpdatedOk, Languages.Accept);
            await _navigationService.NavigateAsync($"/{nameof(OnSaleMasterDetailPage)}/NavigationPage/{nameof(ProductsPage)}");
        }

        private async void CancelOrderAsync()
        {
            bool asnwer = await App.Current.MainPage.DisplayAlert(Languages.Question, Languages.CancelOrdenConfirm, Languages.Yes, Languages.No);
            if (!asnwer)
                return;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }

            Order.OrderStatus = OrderStatus.Cancelled;
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            Response response = await _apiService.PutAsync(Constants.URL_BASE, Constants.SERVICE_PREFIX, Constants.EndPoints.PostOrders, Order, token.Token);

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            await App.Current.MainPage.DisplayAlert(Languages.Ok, Languages.OrderUpdatedOk, Languages.Accept);
            await _navigationService.NavigateAsync($"/{nameof(OnSaleMasterDetailPage)}/NavigationPage/{nameof(ProductsPage)}");
        }
        #endregion [ Methods ]
    }
}
