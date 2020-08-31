using OnSale.Common.Entities;
using OnSale.Common.Responses;
using OnSale.Prism.Helpers;
using OnSale.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;

namespace OnSale.Prism.ViewModels
{
    public class ProductDetailPageViewModel : ViewModelBase
    {
        #region [ Attributes ]
        private ProductResponse _product;
        private ObservableCollection<ProductImage> _images;
        private DelegateCommand _addToCartCommand;
        private readonly INavigationService _navigationService;
        #endregion [ Attributes ]

        #region [ Constructor ]
        public ProductDetailPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = Languages.Details;
            _navigationService = navigationService;
        }
        #endregion [ Constructor ]

        #region [ Properties ]
        public ProductResponse Product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }

        public ObservableCollection<ProductImage> Images
        {
            get => _images;
            set => SetProperty(ref _images, value);
        }

        #endregion [ Properties ]

        #region [ Commands ]
        public DelegateCommand AddToCartCommand => _addToCartCommand ?? (_addToCartCommand = new DelegateCommand(AddToCartAsync));
        #endregion [ Commands ]

        #region [ Methods ]
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("product"))
            {
                Product = parameters.GetValue<ProductResponse>("product");
                Images = new ObservableCollection<ProductImage>(Product.ProductImages);
            }
        }

        private async void AddToCartAsync()
        {
            NavigationParameters parameters = new NavigationParameters
            {
                { "product", _product }
            };

            await _navigationService.NavigateAsync(nameof(AddToCartPage), parameters);
        }
        #endregion [ Methods ]
    }
}
