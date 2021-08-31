using System;
using System.Diagnostics;
using System.Linq;
using Foundation;
using OnSale.Common.Business;
using OnSale.Common.Models;
using Plugin.FacebookClient;
using Prism;
using Prism.Ioc;
using Syncfusion.SfBusyIndicator.XForms.iOS;
using Syncfusion.SfNumericTextBox.XForms.iOS;
using Syncfusion.SfRating.XForms.iOS;
using Syncfusion.SfRotator.XForms.iOS;
using Syncfusion.XForms.iOS.MaskedEdit;
using Syncfusion.XForms.iOS.TextInputLayout;
using UIKit;
using UserNotifications;
using WindowsAzure.Messaging;

namespace OnSale.Prism.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private SBNotificationHub Hub { get; set; }
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            Xamarin.FormsMaps.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            new SfNumericTextBoxRenderer();
            new SfBusyIndicatorRenderer();
            new SfRotatorRenderer();
            SfTextInputLayoutRenderer.Init();
            SfMaskedEditRenderer.Init();
            SfRatingRenderer.Init();
            FacebookClientManager.Initialize(app, options);
            LoadApplication(new App(new iOSInitializer()));

            base.FinishedLaunching(app, options);

            RegisterForRemoteNotifications();

            return true;

        }

        public override void OnActivated(UIApplication uiApplication)
        {
            base.OnActivated(uiApplication);
            FacebookClientManager.OnActivated();
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            return FacebookClientManager.OpenUrl(app, url, options);
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            return FacebookClientManager.OpenUrl(application, url, sourceApplication, annotation);
        }

        void RegisterForRemoteNotifications()
        {
            // register for remote notifications based on system version
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert |
                    UNAuthorizationOptions.Badge |
                    UNAuthorizationOptions.Sound,
                    (granted, error) =>
                    {
                        if (granted)
                            InvokeOnMainThread(UIApplication.SharedApplication.RegisterForRemoteNotifications);
                    });
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }
        }
        
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            byte[] dt = deviceToken.ToArray();
            string token = BitConverter.ToString(dt).Replace("-", "").ToUpperInvariant();

            var DeviceToken = deviceToken.Description;
            if (!string.IsNullOrWhiteSpace(DeviceToken)) {
                DeviceToken = DeviceToken.Trim('<').Trim('>');
            }

            // Get previous device token
            var oldDeviceToken = NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");

            // Has the token changed?
            if (string.IsNullOrEmpty(oldDeviceToken) || !oldDeviceToken.Equals(DeviceToken))
            {
                //TODO: Put your own logic here to notify your server that the device token has changed/been created!
            }

            // Save new device token
            NSUserDefaults.StandardUserDefaults.SetString(DeviceToken, "PushDeviceToken");

            Hub = new SBNotificationHub(AppConstants.ListenConnectionString, AppConstants.NotificationHubName);
            
            Hub.UnregisterAll(deviceToken, (error) =>
            {
                if (error != null)
                {
                    System.Diagnostics.Debug.WriteLine("Error calling Unregister: {0}", error.ToString());
                    return;
                }

                NSSet tags = new NSSet(AppConstants.SubscriptionTags.ToArray()); // create tags if you want
                Hub.RegisterNative(deviceToken, tags, (errorCallback) => {
                    if (errorCallback != null)
                        System.Diagnostics.Debug.WriteLine("RegisterNative error: " + errorCallback.ToString());
                });
                                              
            });
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            ProcessNotification(userInfo, false);
        }

        void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            // make sure we have a payload
            if (options != null && options.ContainsKey(new NSString("aps")))
            {               
                NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;
                string payload = string.Empty;
                NSString payloadKey = new NSString("alert");
                if (aps.ContainsKey(payloadKey))
                {
                    payload = aps.ObjectForKey(new NSString("alert")) as NSString;
                    var parameters = aps.ObjectForKey(new NSString("parameters")) as NSDictionary;

                    NotificationPush notificationPush = new NotificationPush();
                    notificationPush.Notification = new Notification
                    {
                        Title = parameters["title"].ToString(),
                        Body = parameters["body"].ToString(),
                        Tag = parameters["tag"].ToString(),                       
                    };
                    notificationPush.Data = new DataNotification { Id = new Guid(parameters["id"].ToString()) };

                    NSError error;
                    var json = NSJsonSerialization.Serialize(parameters, NSJsonWritingOptions.PrettyPrinted, out error);
                    Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(json.ToString(NSStringEncoding.UTF8));
                }
                                
                if (!string.IsNullOrWhiteSpace(payload))
                {
                    var myAlert = UIAlertController.Create("Notification", payload, UIAlertControllerStyle.Alert);
                    myAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                    UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(myAlert, true, null);

                    
                }
            }
            else
            {
                Debug.WriteLine($"Received request to process notification but there was no payload.");
            }
        }

        
    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}
