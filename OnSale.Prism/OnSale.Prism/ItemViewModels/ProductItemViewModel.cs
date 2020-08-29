using OnSale.Common.Entities;
using OnSale.Prism.Views;
using Prism.Commands;
using Prism.Navigation;

namespace OnSale.Prism.ItemViewModels
{
    public class ProductItemViewModel : Product
    {
        #region [ Attributes ]
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectProductCommand;
        #endregion [ Attributes ]

        #region [ Constructor ]
        public ProductItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        #endregion [ Constructor ]

        #region [ Commands ]
        public DelegateCommand SelectProductCommand => _selectProductCommand ?? (_selectProductCommand = new DelegateCommand(SelectProductAsync));
        #endregion [ Commands ]

        #region [ Methods ]
        private async void SelectProductAsync()
        {
            NavigationParameters parameters = new NavigationParameters
            {
                { "product", this }
            };
            //await _navigationService.NavigateAsync(nameof(ProductDetailPage), parameters);
            await _navigationService.NavigateAsync($"/{nameof(OnSaleMasterDetailPage)}/NavigationPage/{nameof(ProductDetailPage)}", parameters);
        }
        #endregion
    }
}
