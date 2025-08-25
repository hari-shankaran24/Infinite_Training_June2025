using Movies.Models;
using Movies.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movies.Controllers
{
     public class MovieController : Controller
    {
        private IMovieRepository repo = new MovieRepository();

        public ActionResult Index()
        {
            return View(repo.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                repo.Add(movie);
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        public ActionResult Edit(int id)
        {
            var movie = repo.GetById(id);
            return View(movie);
        }

        [HttpPost]
        public ActionResult Edit(Movie movie)
        {
            if (ModelState.IsValid)
            {
                repo.Update(movie);
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        public ActionResult Delete(int id)
        {
            var movie = repo.GetById(id);
            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            repo.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult MoviesByYear(int? year)
        {
            if (year == null)
            {
                return View(new List<Movie>()); // or return View(null);
            }

            var movies = repo.GetByYear(year.Value);
            return View(movies);
        }


        public ActionResult MoviesByDirector(string directorName)
        {
            var movies = repo.GetByDirector(directorName);
            return View(movies);
        }
    }
}