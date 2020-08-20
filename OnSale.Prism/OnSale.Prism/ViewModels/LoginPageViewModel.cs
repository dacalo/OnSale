using Newtonsoft.Json;
using OnSale.Common.Business;
using OnSale.Common.Helpers;
using OnSale.Common.Requests;
using OnSale.Common.Responses;
using OnSale.Common.Services;
using OnSale.Prism.Helpers;
using OnSale.Prism.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OnSale.Prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        #region [ Attributes ]
        private bool _isRunning;
        private bool _isEnabled;
        private string _password;
        private DelegateCommand _loginCommand;
        private DelegateCommand _registerCommand;
        private DelegateCommand _forgotPasswordCommand;
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        #endregion [ Attributes ]

        #region [ Constructor ]
        public LoginPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            Title = Languages.Login;
            IsEnabled = true;
            _navigationService = navigationService;
            _apiService = apiService;
        }
        #endregion [ Constructor ]

        #region [ Properties ]
        public string Email { get; set; }
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

        public string Password 
        { 
            get => _password;
            set => SetProperty(ref _password, value);
        }

        #endregion [ Properties ]

        #region [ Commands ]
        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(LogingAsync));

        public DelegateCommand RegisterCommand => _registerCommand ?? (_registerCommand = new DelegateCommand(RegisterAsync));

        public DelegateCommand ForgotPasswordCommand => _forgotPasswordCommand ?? (_forgotPasswordCommand = new DelegateCommand(ForgotPasswordAsync));
        #endregion [ Commands ]

        #region [ Methods ]
        private async void LogingAsync()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.EmailError, Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PasswordError, Languages.Accept);
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            if(Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }

            TokenRequest request = new TokenRequest
            {
                Username = Email,
                Password = Password
            };

            Response response = await _apiService.GetTokenAsync(Constants.URL_BASE, Constants.SERVICE_PREFIX, "/Account/CreateToken", request);
            IsRunning = false;
            IsEnabled = true;

            if(!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.LoginError, Languages.Accept);
                Password = string.Empty;
                return;
            }

            TokenResponse token = (TokenResponse)response.Result;
            Settings.Token = JsonConvert.SerializeObject(token);
            Settings.IsLogin = true;

            IsRunning = false;
            IsEnabled = true;

            await _navigationService.NavigateAsync($"/{nameof(OnSaleMasterDetailPage)}/NavigationPage/{nameof(ProductsPage)}");
            Password = string.Empty;
        }

        private async void RegisterAsync()
        {

        }

        private async void ForgotPasswordAsync()
        {

        }
        #endregion [ Methods ]
    }
}
