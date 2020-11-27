using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace OnSale.Prism.Droid
{
    public class InstallListener : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            string rawReferrerString = intent.GetStringExtra("referrer");
            if (rawReferrerString != null)
            {
                Preferences.Set("Referrer", rawReferrerString);
            }
        }
    }
}