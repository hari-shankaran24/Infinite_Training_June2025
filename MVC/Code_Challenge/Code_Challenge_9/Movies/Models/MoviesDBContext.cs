using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Movies.Models
{
    public class MoviesDBContext : DbContext
    {
        public MoviesDBContext() : base("MoviesDbConnection") { }

        public DbSet<Movie> Movies { get; set; }
    }
}