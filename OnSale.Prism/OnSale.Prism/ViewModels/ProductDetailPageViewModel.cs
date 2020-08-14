using OnSale.Common.Entities;
using OnSale.Prism.Helpers;
using Prism.Navigation;
using System.Collections.ObjectModel;

namespace OnSale.Prism.ViewModels
{
    public class ProductDetailPageViewModel : ViewModelBase
    {
        #region [ Attributes ]
        private Product _product;
        private ObservableCollection<ProductImage> _images;
        #endregion [ Attributes ]

        #region [ Constructor ]
        public ProductDetailPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = Languages.Product;
        }
        #endregion [ Constructor ]

        #region [ Properties ]
        public Product Product
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

        #region [ Methods ]
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("product"))
            {
                Product = parameters.GetValue<Product>("product");
                Title = Product.Name;
                Images = new ObservableCollection<ProductImage>(Product.ProductImages);
            }
        }
        #endregion [ Methods ]
    }
}
