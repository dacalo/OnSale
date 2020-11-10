using OnSale.Common.Responses;
using OnSale.Prism.Views;
using Prism.Commands;
using Prism.Navigation;

namespace OnSale.Prism.ItemViewModels
{
    public class OrderItemViewModel : OrderResponse
    {
        #region [ Attributes ]
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectOrderCommand;
        #endregion [ Attributes ]

        #region [ Constructor ]
        public OrderItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        #endregion [ Constructor ]

        #region [ Commands ]
        public DelegateCommand SelectOrderCommand => _selectOrderCommand ?? (_selectOrderCommand = new DelegateCommand(SelectOrderAsync));
        #endregion [ Commands ]

        #region [ Methods ]
        private async void SelectOrderAsync()
        {
            NavigationParameters parameters = new NavigationParameters
            {
                { "order", this }
            };

            await _navigationService.NavigateAsync(nameof(OrderPage), parameters);
        }
        #endregion [ Methods ]
    }

}
