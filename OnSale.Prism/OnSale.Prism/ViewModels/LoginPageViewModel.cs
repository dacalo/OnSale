﻿using Newtonsoft.Json;
using OnSale.Common.Business;
using OnSale.Common.Helpers;
using OnSale.Common.Requests;
using OnSale.Common.Responses;
using OnSale.Common.Services;
using OnSale.Prism.Helpers;
using OnSale.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Essentials;

namespace OnSale.Prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        #region [ Attributes ]
        private string _pageReturn;
        private bool _isRunning;
        private bool _isEnabled;
        private string _password;
        private DelegateCommand _loginCommand;
        private DelegateCommand _registerCommand;
        private DelegateCommand _forgotPasswordCommand;
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IRegexHelper _regexHelper;
        #endregion [ Attributes ]

        #region [ Constructor ]
        public LoginPageViewModel(
            INavigationService navigationService,
            IApiService apiService,
            IRegexHelper regexHelper) : base(navigationService)
        {
            Title = Languages.Login;
            IsEnabled = true;
            _navigationService = navigationService;
            _apiService = apiService;
            _regexHelper = regexHelper;
            Email = "divadchl@gmail.com";
            Password = "123456";
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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("pageReturn"))
                _pageReturn = parameters.GetValue<string>("pageReturn");
        }

        private async void LogingAsync()
        {
            if (string.IsNullOrEmpty(Email) || !_regexHelper.IsValidEmail(Email))
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

            Response response = await _apiService.GetTokenAsync(
                Constants.URL_BASE,
                Constants.SERVICE_PREFIX,
                Constants.EndPoints.PostCreateToken,
                request);
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

            if (string.IsNullOrEmpty(_pageReturn))
                await _navigationService.NavigateAsync($"/{nameof(OnSaleMasterDetailPage)}/NavigationPage/{nameof(ProductsPage)}");
            else
                await _navigationService.NavigateAsync($"/{nameof(OnSaleMasterDetailPage)}/NavigationPage/{_pageReturn}");

            Password = string.Empty;
        }

        private async void RegisterAsync()
        {
            await _navigationService.NavigateAsync(nameof(RegisterPage));
        }

        private async void ForgotPasswordAsync()
        {
            await _navigationService.NavigateAsync(nameof(RecoverPasswordPage));
        }
        #endregion [ Methods ]
    }
}
