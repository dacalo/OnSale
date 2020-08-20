using OnSale.Prism.Helpers;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace OnSale.Prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        #region [ Constructor ]
        public LoginPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = Languages.Login;
        }
        #endregion [ Constructor ]
    }
}
