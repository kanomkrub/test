using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TNI.Controllers;

namespace TNI.Access
{
    public static class InformixAccess
    {
        public static bool IsCustomer(string national_id)
        {
            return true;
        }

        internal static Dictionary<string, string> GetPoint(string type_of_point, string dayend_date)
        {
            Dictionary<string, string> result;
            switch (type_of_point.ToLower())
            {
                case "d":
                    //get point (daily)
                    result = GetDailyPoint(dayend_date);
                    break;
                case "m":
                    //get point (monthly)
                    result = GetMonthlyPoint(dayend_date);
                    break;
                case "e":
                    result = GetExpiryPoint(dayend_date);
                    //expiry business logic
                    break;
                default:
                    throw new ArgumentException("type_of_point value not valid");
            }
            return result;
        }

        private static Dictionary<string, string> GetExpiryPoint(string dayend_date)
        {
            var result = new Dictionary<string, string>();
            result.Add("national_id", Guid.NewGuid().ToString());
            result.Add("type_of_data", "E");
            result.Add("point_of_date", DateTime.Today.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            result.Add("point_of_id", Guid.NewGuid().ToString());
            return result;
        }

        internal static object Test()
        {
            return null;
        }

        private static Dictionary<string, string> GetMonthlyPoint(string dayend_date)
        {
            //DateTime.ParseExact(dayend_date, "dd_MM_yyyy", CultureInfo.InvariantCulture);
            var result = new Dictionary<string, string>();
            result.Add("national_id", Guid.NewGuid().ToString());
            result.Add("type_of_data", "M");
            result.Add("point_of_date", DateTime.Today.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            result.Add("point_of_id", Guid.NewGuid().ToString());
            return result;
        }

        private static Dictionary<string, string> GetDailyPoint(string dayend_date)
        {
            var result = new Dictionary<string, string>();
            result.Add("national_id", Guid.NewGuid().ToString());
            result.Add("type_of_data", "D");
            result.Add("point_of_date", DateTime.Today.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            result.Add("point_of_id", Guid.NewGuid().ToString());
            return result;
        }
        
    }
}
