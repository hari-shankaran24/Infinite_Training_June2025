using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Electricity_Bill.App_Code
{
    public class ElectricityBill
    {
        private string consumerNumber;
        private string consumerName;
        private int unitsConsumed;
        private double billAmount;

        public string ConsumerNumber
        {
            get { return consumerNumber; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || Regex.IsMatch(value.Trim(), @"^EB\d{5}$") == false)
                    throw new FormatException("Invalid Consumer Number");
                consumerNumber = value.Trim();
            }
        }

        public string ConsumerName
        {
            get { return consumerName; }
            set { consumerName = (value ?? "").Trim(); }
        }

        public int UnitsConsumed
        {
            get { return unitsConsumed; }
            set { unitsConsumed = value; }
        }

        public double BillAmount
        {
            get { return billAmount; }
            set { billAmount = value; }
        }
    }
}