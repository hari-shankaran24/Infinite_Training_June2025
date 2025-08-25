using Movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movies.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private MoviesDBContext db = new MoviesDBContext();

        public IEnumerable<Movie> GetAll() => db.Movies.ToList();

        public Movie GetById(int id) => db.Movies.Find(id);

        public void Add(Movie movie)
        {
            db.Movies.Add(movie);
            db.SaveChanges();
        }

        public void Update(Movie movie)
        {
            var existing = db.Movies.Find(movie.Mid);
            if (existing != null)
            {
                existing.MovieName = movie.MovieName;
                existing.DirectorName = movie.DirectorName;
                existing.DateOfRelease = movie.DateOfRelease;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var movie = db.Movies.Find(id);
            if (movie != null)
            {
                db.Movies.Remove(movie);
                db.SaveChanges();
            }
        }

        public IEnumerable<Movie> GetByYear(int year)
        {
            return db.Movies.Where(m => m.DateOfRelease.Year == year).ToList();
        }

        public IEnumerable<Movie> GetByDirector(string directorName)
        {
            return db.Movies.Where(m => m.DirectorName == directorName).ToList();
        }
    }
}