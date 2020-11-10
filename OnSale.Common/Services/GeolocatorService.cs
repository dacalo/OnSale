using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Threading.Tasks;

namespace OnSale.Common.Services
{
    public class GeolocatorService : IGeolocatorService
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public async Task GetLocationAsync()
        {
            try
            {
                IGeolocator locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                Position location = await locator.GetPositionAsync();
                Latitude = location.Latitude;
                Longitude = location.Longitude;

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }

}
