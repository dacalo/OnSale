using OnSale.Common.Models;
using OnSale.Prism.Views;
using Prism.Commands;
using Prism.Navigation;

namespace OnSale.Prism.ViewModels
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
            await _navigationService.NavigateAsync($"/{nameof(OnSaleMasterDetailPage)}/NavigationPage/{PageName}");
        }
        #endregion [ Methods ]
    }

}
