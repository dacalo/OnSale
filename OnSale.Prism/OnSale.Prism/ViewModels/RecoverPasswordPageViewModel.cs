using OnSale.Common.Business;
using OnSale.Common.Helpers;
using OnSale.Common.Requests;
using OnSale.Common.Responses;
using OnSale.Common.Services;
using OnSale.Prism.Helpers;
using Prism.Commands;
using Prism.Navigation;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace OnSale.Prism.ViewModels
{
    public class RecoverPasswordPageViewModel : ViewModelBase
    {
        #region [ Attributes ]
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IRegexHelper _regexHelper;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _recoverCommand;
        #endregion [ Attributes ]

        #region [ Constructor ]
        public RecoverPasswordPageViewModel(
            INavigationService navigationService,
            IApiService apiService,
            IRegexHelper regexHelper)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _regexHelper = regexHelper;
            Title = Languages.RecoverPassword;
            IsEnabled = true;
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
        #endregion [ Properties ]

        #region [ Commands ]
        public DelegateCommand RecoverCommand => _recoverCommand ?? (_recoverCommand = new DelegateCommand(RecoverAsync));
        #endregion [ Commands ]
        
        #region [ Methods ]
        private async void RecoverAsync()
        {
            bool isValid = await ValidateData();
            if (!isValid)
                return;

            IsRunning = true;
            IsEnabled = false;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }

            EmailRequest request = new EmailRequest { Email = Email };
            Response response = await _apiService.RecoverPasswordAsync(
                Constants.URL_BASE,
                Constants.SERVICE_PREFIX,
                Constants.EndPoints.PostRecoverPassword,
                request);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                if (response.Message == "Error001")
                    await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.Error001, Languages.Accept);
                else
                    await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);

                return;
            }

            await App.Current.MainPage.DisplayAlert(Languages.Ok, Languages.RecoverPasswordMessage, Languages.Accept);
            await _navigationService.GoBackAsync();
        }

        private async Task<bool> ValidateData()
        {
            if (string.IsNullOrEmpty(Email) || !_regexHelper.IsValidEmail(Email))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.EmailError, Languages.Accept);
                return false;
            }

            return true;
        }
        #endregion[ Methods ]
    }
}
