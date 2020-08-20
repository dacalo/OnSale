﻿using OnSale.Common.Interfaces;
using OnSale.Prism.Resources;
using System.Globalization;
using Xamarin.Forms;

namespace OnSale.Prism.Helpers
{
    public static class Languages
    {
        static Languages()
        {
            CultureInfo ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            Culture = ci.Name;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Culture { get; set; }

        public static string Accept => Resource.Accept;
        public static string ConnectionError => Resource.ConnectionError;
        public static string Error => Resource.Error;
        public static string Products => Resource.Products;
        public static string Loading => Resource.Loading;
        public static string Product => Resource.Product;
        public static string SearchProduct => Resource.SearchProduct;
        public static string Name => Resource.Name;
        public static string Description => Resource.Description;
        public static string Price => Resource.Price;
        public static string IsStarred => Resource.IsStarred;
        public static string Category => Resource.Category;
        public static string AddCart => Resource.AddCart;
        public static string Login => Resource.Login;
        public static string ShowShoppingCar => Resource.ShowShoppingCar;
        public static string ShowPurchaseHistory => Resource.ShowPurchaseHistory;
        public static string ModifyUser => Resource.ModifyUser;
    }

}
