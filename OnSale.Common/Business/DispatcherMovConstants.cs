using System;
namespace OnSale.Common.Business
{
    public static class DispatcherMovConstants
    {
        public static string[] SubscriptionTags { get; set; } = { "default" };
        public static string NotificationHubName { get; set; } = "OnSaleNotifications";
        public static string FullAccessConnectionString { get; set; } = "Endpoint=sb://notificationsonsalenamespace.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=lKKPlBQHygdCB+NLXdECyzo+dtyWsg1MJA9Noc6sMes=";
    }
}
