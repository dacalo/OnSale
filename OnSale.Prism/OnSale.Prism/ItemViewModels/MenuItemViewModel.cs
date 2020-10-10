using OnSale.Common.Helpers;
using OnSale.Common.Models;
using OnSale.Prism.Views;
using Prism.Commands;
using Prism.Navigation;

namespace OnSale.Prism.ItemViewModels
{
    public class MenuItemViewModel : Menu
    {
        #region [ Attributes ]
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectMenuCommand;
        #endregion [ Attributes ]

        #region [ Constructor ]
        public MenuItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        #endregion [ Constructor ]

        #region [ Commands ]
        public DelegateCommand SelectMenuCommand => _selectMenuCommand ?? (_selectMenuCommand = new DelegateCommand(SelectMenuAsync));
        #endregion [ Commands ]

        #region [ Methods ]
        private async void SelectMenuAsync()
        {
            if (PageName == nameof(LoginPage) && Settings.IsLogin)
            {
                Settings.IsLogin = false;
                Settings.Token = null;
                Settings.OrderDetails = null;
            }

            if (IsLoginRequired && !Settings.IsLogin)
            {
                NavigationParameters parameters = new NavigationParameters
                {
                    {"pageReturn", PageName}
                };
                await _navigationService.NavigateAsync($"/{nameof(OnSaleMasterDetailPage)}/NavigationPage/{nameof(LoginPage)}", parameters);
            }
            else
            {
                await _navigationService.NavigateAsync($"/{nameof(OnSaleMasterDetailPage)}/NavigationPage/{PageName}");
            }

        }
        #endregion [ Methods ]
    }
}
