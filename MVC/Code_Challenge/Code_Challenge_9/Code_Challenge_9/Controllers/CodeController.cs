using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Code_Challenge_9.Models;

namespace Code_Challenge_9.Controllers
{
    public class CodeController : Controller
    {
        private NorthwindEntities1 db = new NorthwindEntities1();

        // 1. Return all customers residing in Germany
        public ActionResult CustomersInGermany()
        {
            var germanCustomers = db.Customers
                                    .Where(c => c.Country == "Germany")
                                    .ToList();

            return View(germanCustomers);
        }

        // 2. Return customer details with an orderId == 10248
        public ActionResult CustomerByOrder()
        {
            var customer = (from o in db.Orders
                            where o.OrderID == 10248
                            select o.Customer).FirstOrDefault();

            return View(customer);
        }
    }
}