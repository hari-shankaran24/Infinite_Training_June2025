using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Question2.Models; 
using System.Web.Http.Description;

namespace Question2.Controllers
{
    [RoutePrefix("api/orders")] // Route prefix for all routes in this controller
    public class OrdersController : ApiController
    {
        private NorthwindEntities2 db = new NorthwindEntities2(); // ✅ Updated context

        // GET: api/orders/employee/5
        [HttpGet]
        [Route("employee/{employeeId:int}")]
        public IHttpActionResult GetOrdersByEmployee(int employeeId)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;

                var employee = db.Employees.FirstOrDefault(e => e.EmployeeID == employeeId);
                if (employee == null)
                    return NotFound();

                if (!string.Equals(employee.FirstName, "Steven", StringComparison.OrdinalIgnoreCase)
                    || !string.Equals(employee.LastName, "Buchanan", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest("EmployeeId does not match Steven Buchanan.");
                }

                var orders = db.Orders
                               .Where(o => o.EmployeeID == employeeId)
                               .Select(o => new
                               {
                                   o.OrderID,
                                   o.OrderDate,
                                   o.RequiredDate,
                                   o.ShipName,
                                   o.ShipCity,
                                   o.ShipCountry,
                                   o.CustomerID
                               })
                               .ToList();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}
