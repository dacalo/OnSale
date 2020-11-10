using Prism;
using Prism.Ioc;
using OnSale.Prism.ViewModels;
using OnSale.Prism.Views;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Xamarin.Forms;
using OnSale.Common.Services;
using Syncfusion.Licensing;
using OnSale.Common.Business;
using OnSale.Common.Helpers;
using OnSale.Prism.Helpers;

namespace OnSale.Prism
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            Device.SetFlags(new string[] { "Shapes_Experimental" });
            SyncfusionLicenseProvider.RegisterLicense(Constants.TextString.SyncfusionLicense);
            InitializeComponent();

            await NavigationService.NavigateAsync($"{nameof(OnSaleMasterDetailPage)}/NavigationPage/{nameof(ProductsPage)}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.Register<IRegexHelper, RegexHelper>();
            containerRegistry.Register<IFilesHelper, FilesHelper>();
            containerRegistry.Register<ICombosHelper, CombosHelper>();
            containerRegistry.Register<IGeolocatorService, GeolocatorService>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<ProductsPage, ProductsPageViewModel>();
            containerRegistry.RegisterForNavigation<ProductDetailPage, ProductDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<OnSaleMasterDetailPage, OnSaleMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<ShowCarPage, ShowCarPageViewModel>();
            containerRegistry.RegisterForNavigation<ShowHistoryPage, ShowHistoryPageViewModel>();
            containerRegistry.RegisterForNavigation<ModifyUserPage, ModifyUserPageViewModel>();
            containerRegistry.RegisterForNavigation<QualificationsPage, QualificationsPageViewModel>();
            containerRegistry.RegisterForNavigation<QualificationDetailPage, QualificationDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<ProductTabbedPage, ProductTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<AddToCartPage, AddToCartPageViewModel>();
            containerRegistry.RegisterForNavigation<AddQualificationPage, AddQualificationPageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterPageViewModel>();
            containerRegistry.RegisterForNavigation<RecoverPasswordPage, RecoverPasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<ChangePasswordPage, ChangePasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<ModifiyOrderPage, ModifiyOrderPageViewModel>();
            containerRegistry.RegisterForNavigation<FinishOrderPage, FinishOrderPageViewModel>();
            containerRegistry.RegisterForNavigation<OrderPage, OrderPageViewModel>();
            containerRegistry.RegisterForNavigation<MapPage, MapPageViewModel>();
        }
    }
}
