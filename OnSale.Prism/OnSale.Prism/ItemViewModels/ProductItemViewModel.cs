﻿using Newtonsoft.Json;
using OnSale.Common.Helpers;
using OnSale.Common.Responses;
using OnSale.Prism.Views;
using Prism.Commands;
using Prism.Navigation;

namespace OnSale.Prism.ItemViewModels
{
    public class ProductItemViewModel : ProductResponse
    {
        #region [ Attributes ]
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectProductCommand;
        private DelegateCommand _selectProduct2Command;
        #endregion [ Attributes ]

        #region [ Constructor ]
        public ProductItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        #endregion [ Constructor ]

        #region [ Properties ]
        public float Quantity { get; set; }

        public string Remarks { get; set; }

        public decimal Value => (decimal)Quantity * Price;

        #endregion

        #region [ Commands ]
        public DelegateCommand SelectProductCommand => _selectProductCommand ?? (_selectProductCommand = new DelegateCommand(SelectProductAsync));
        public DelegateCommand SelectProduct2Command => _selectProduct2Command ?? (_selectProduct2Command = new DelegateCommand(SelectProduct2Async));
        #endregion [ Commands ]

        #region [ Methods ]
        private async void SelectProductAsync()
        {
            NavigationParameters parameters = new NavigationParameters
            {
                { "product", this }
            };
            Settings.Product = JsonConvert.SerializeObject(this);
            await _navigationService.NavigateAsync(nameof(ProductTabbedPage), parameters);
        }

        private async void SelectProduct2Async()
        {
            NavigationParameters parameters = new NavigationParameters
            {
                { "product", this }
            };

            Settings.Product = JsonConvert.SerializeObject(this);
            await _navigationService.NavigateAsync(nameof(ModifiyOrderPage), parameters);
        }
        #endregion
    }
}
