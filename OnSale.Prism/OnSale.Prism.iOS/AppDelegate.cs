using Foundation;
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
using WindowsAzure.Messaging;
using UserNotifications;
using OnSale.Common.Business;
using System.Diagnostics;
using System;
using System.Linq;

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

        private void RegisterForRemoteNotifications()
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
                        {
                            InvokeOnMainThread(UIApplication.SharedApplication.RegisterForRemoteNotifications);
                        }
                    });
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                UIUserNotificationSettings pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
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
            Hub = new SBNotificationHub(AppConstants.ListenConnectionString, AppConstants.NotificationHubName);
            var token = deviceToken.ToString().Trim('<').Trim('>');
            // update registration with Azure Notification Hub
            Hub.UnregisterAll(deviceToken, (error) =>
            {
                if (error != null)
                {
                    Debug.WriteLine($"Unable to call unregister {error}");
                    return;
                }

                NSSet tags = new NSSet(AppConstants.SubscriptionTags.ToArray());
                Hub.RegisterNative(deviceToken, tags, (errorCallback) =>
                {
                    if (errorCallback != null)
                    {
                        Debug.WriteLine($"RegisterNativeAsync error: {errorCallback}");
                    }
                });

                string templateExpiration = DateTime.Now.AddDays(120).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                Hub.RegisterTemplate(deviceToken, "defaultTemplate", AppConstants.APNTemplateBody, templateExpiration, tags, (errorCallback) =>
                {
                    if (errorCallback != null)
                    {
                        if (errorCallback != null)
                        {
                            Debug.WriteLine($"RegisterTemplateAsync error: {errorCallback}");
                        }
                    }
                });
            });
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            ProcessNotification(userInfo, false);
        }

        private void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            // make sure we have a payload
            if (options != null && options.ContainsKey(new NSString("aps")))
            {
                // get the APS dictionary and extract message payload. Message JSON will be converted
                // into a NSDictionary so more complex payloads may require more processing
                NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;
                string payload = string.Empty;
                NSString payloadKey = new NSString("alert");
                if (aps.ContainsKey(payloadKey))
                {
                    payload = aps[payloadKey].ToString();
                }

                if (!string.IsNullOrWhiteSpace(payload))
                {
                    //TournamentsPageViewModel.GetInstance().AddMessage(payload);
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
