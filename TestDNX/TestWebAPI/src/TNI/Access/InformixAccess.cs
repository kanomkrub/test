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

        internal static object GetPoint(string type_of_point, string dayend_date)
        {
            DateTime dayend;
            if (!DateTime.TryParseExact(dayend_date, "dd_MM_yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out dayend))
                return new { system_error_description = new { ErrorCode = "2", ErrorDesc = "invalid dateformat(dd_MM_yyyy) :" + dayend_date } };
            if (!new List<string>() { "d", "m", "e" }.Contains(type_of_point.ToLower()))
                return new { system_error_description = new { ErrorCode = "2", ErrorDesc = "invalid type :"+type_of_point } };
            var result = new
            {
                system_error_description = new { ErrorCode = "0" },
                customer_point = new
                {
                    National_ID = Guid.NewGuid().ToString(),
                    Type_of_Point = type_of_point.ToUpper(),
                    Point_of_Date = DateTime.Today.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Point_of_ID = DateTime.Now.Millisecond
                }
            };
            return result;
        }
    }
}
