using System;
using Newtonsoft.Json;

namespace OnSale.Common.Models
{
    public class NotificationPush
    {
        [JsonProperty(PropertyName = "notification")]
        public Notification Notification { get; set; } = new Notification();
        [JsonProperty(PropertyName = "data")]
        public DataNotification Data { get; set; } = new DataNotification();
    }

    public class Notification
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; }
    }

    public class DataNotification
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
    }
}
