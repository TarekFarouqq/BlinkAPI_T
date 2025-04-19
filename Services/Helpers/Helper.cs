namespace Blink_API.Services.Helpers
{
    public class Helper
    {
        public static double CalculateDistance(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
        {
            try
            {
                var R = 6371.0; // Radius of the Earth in km (double)
                var dLat = (double)(lat2 - lat1) * Math.PI / 180;
                var dLon = (double)(lon2 - lon1) * Math.PI / 180;

                var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                        Math.Cos((double)lat1 * Math.PI / 180) * Math.Cos((double)lat2 * Math.PI / 180) *
                        Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

                var distance = R * c; // Distance in km (double)
                return distance;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating distance: {ex.Message}");
                return double.MaxValue; // Return a high value to avoid any undefined behavior
            }
        }


    }
}
