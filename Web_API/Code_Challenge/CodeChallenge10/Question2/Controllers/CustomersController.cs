using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Question2.Models; 

namespace Question2.Controllers
{
    public class CustomersController : ApiController
    {
        private NorthwindEntities2 db = new NorthwindEntities2(); 

        // GET: api/customers/bycountry/{country}
        [HttpGet]
        [Route("api/customers/bycountry/{country}")]
        [ResponseType(typeof(IEnumerable<Customer>))]
        public IHttpActionResult GetCustomersByCountry(string country)
        {
            if (string.IsNullOrWhiteSpace(country))
            {
                return BadRequest("Country parameter is required.");
            }

            try
            {
                var customers = db.Database.SqlQuery<Customer>(
                    "EXEC GetCustomersByCountry @p0", country
                ).ToList();

                if (customers == null || customers.Count == 0)
                {
                    return Ok(new List<Customer>());
                }

                return Ok(customers);
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
