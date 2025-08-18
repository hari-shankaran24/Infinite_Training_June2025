using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Electricity_Bill
{
    public static class BillValidator
    {
        public static string ValidateUnitsConsumed(int UnitsConsumed)
        {
            return UnitsConsumed < 0 ? "Given units is invalid" : string.Empty;
        }
    }
}