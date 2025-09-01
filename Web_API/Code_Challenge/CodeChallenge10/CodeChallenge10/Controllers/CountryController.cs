using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CodeChallenge10.Models;

namespace CodeChallenge10.Controllers
{
    public class CountryController : ApiController
    {
        // Mock in-memory data store
        private static List<Country> countries = new List<Country>
        {
            new Country { ID = 1, CountryName = "USA", Capital = "Washington, D.C." },
            new Country { ID = 2, CountryName = "India", Capital = "New Delhi" },
            new Country { ID = 3, CountryName = "Japan", Capital = "Tokyo" }
        };

        // GET api/country
        [HttpGet]
        public IHttpActionResult GetCountries()
        {
            return Ok(countries);
        }

        // GET api/country/5
        [HttpGet]
        public IHttpActionResult GetCountry(int id)
        {
            var country = countries.FirstOrDefault(c => c.ID == id);
            if (country == null)
                return NotFound();

            return Ok(country);
        }

        // POST api/country
        [HttpPost]
        public IHttpActionResult CreateCountry([FromBody] Country newCountry)
        {
            if (newCountry == null)
                return BadRequest("Invalid data.");

            // Auto-increment ID
            newCountry.ID = countries.Any() ? countries.Max(c => c.ID) + 1 : 1;
            countries.Add(newCountry);

            return CreatedAtRoute("DefaultApi", new { id = newCountry.ID }, newCountry);
        }

        // PUT api/country/5
        [HttpPut]
        public IHttpActionResult UpdateCountry(int id, [FromBody] Country updatedCountry)
        {
            if (updatedCountry == null || updatedCountry.ID != id)
                return BadRequest("Invalid data.");

            var country = countries.FirstOrDefault(c => c.ID == id);
            if (country == null)
                return NotFound();

            country.CountryName = updatedCountry.CountryName;
            country.Capital = updatedCountry.Capital;

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE api/country/5
        [HttpDelete]
        public IHttpActionResult DeleteCountry(int id)
        {
            var country = countries.FirstOrDefault(c => c.ID == id);
            if (country == null)
                return NotFound();

            countries.Remove(country);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
