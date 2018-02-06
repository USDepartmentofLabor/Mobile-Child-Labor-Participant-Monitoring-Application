using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Threading.Tasks;
using System;

namespace MDPMS.Helper.Gps
{
    public static class GpsHelper
    {
        public static async Task<Position> GetGpsPosition()
        {
            try
            {
                var position = await CrossGeolocator.Current.GetPositionAsync(new TimeSpan(0, 0, 0, 0, 1000));
                return position;
            }
            catch
            {
                return null;
            }
        }
    }
}
