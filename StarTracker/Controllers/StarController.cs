using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StarTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace StarTracker.Controllers
{
    public class StarController : Controller
    {
        private int? uid
        {
            get
            {
                return HttpContext.Session.GetInt32("UserId");
            }
        }

        private bool isLoggedIn
        {
            get
            {
                return uid != null;
            }
        }
        private StarTrackerContext db;
        public StarController(StarTrackerContext context)
        {
            db = context;
        }

        [HttpGet("Home")]
        public IActionResult Home()
        {
            if(!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }

            List<Star> allStars = db.Stars
                .Include(m => m.CreatedBy)
                .OrderByDescending(m => m.CreatedAt).ToList();

            return View("Home", allStars);
        }

        [HttpGet("/stars/new")]
        public IActionResult New()
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("New");
        }
        
        [HttpPost("/stars/create")]
        public IActionResult Create(Star newStar, IFormFile files)
        {

            if(!ModelState.IsValid)
            {
                return View("New");
            }

            newStar.UserId = (int)uid;
            db.Stars.Add(newStar);
            db.SaveChanges();

            return RedirectToAction("Home");
        }

        [HttpGet("/stars/{starId}/details")]
        public IActionResult Details(int starId)
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }

            Star star = db.Stars.Include(m => m.CreatedBy).Include(m => m.Comments).ThenInclude(c => c.User).FirstOrDefault(m => m.StarId == starId);

            if(star == null)
            {
                return RedirectToAction("Home");
            }

            return View("Details", star);
        }

        [HttpPost("/star/{starId}/delete")]
        public IActionResult Delete(int starId)
        {
            Star star = db.Stars.FirstOrDefault(m => m.StarId == starId);
            
            if(star != null)
            {
                db.Stars.Remove(star);
                db.SaveChanges();
            }
            return RedirectToAction("Home");
        }

        [HttpGet("/star/{starId}/edit")]
        public IActionResult Edit(int starId)
        {
            Star star = db.Stars.FirstOrDefault(m => m.StarId == starId);

            if(star == null || star.UserId != uid)
            {
                return RedirectToAction("Home");
            }

            return View("Edit", star);
        }

        [HttpPost("/star/{starId}/update")]
        public IActionResult Update(Star editedStar, int starId)
        {
            if(!ModelState.IsValid)
            {
                editedStar.StarId = starId;

                return View("Edit", editedStar);
            }

            Star dbStar = db.Stars.FirstOrDefault(m => m.StarId == starId);

            if(dbStar == null)
            {
                return RedirectToAction("Home");
            }

            dbStar.UpdatedAt = DateTime.Now;
            dbStar.StarName = editedStar.StarName;
            dbStar.Description = editedStar.Description;
            dbStar.Location = editedStar.Location;
            dbStar.Time = editedStar.Time;

            db.Stars.Update(dbStar);
            db.SaveChanges();

            return RedirectToAction("Home");
        }
        
        [HttpPost("/star/{starId}/addcomment")]
        public IActionResult AddComment(int starId, Comment newComment)
        {
            if(!ModelState.IsValid)
            {
                return View("Details");
            }

            Comment comments = new Comment()
            {
                Note = newComment.Note,
                UserId = (int)uid,
                StarId = starId
            };

            db.Comments.Add(comments);
            db.SaveChanges();
        
            return RedirectToAction("Details", new {starId = starId});
        }

        [HttpPost("/star/{commentId}/{starId}/deletecomment")]
        public IActionResult DeleteComment(int commentId, int starId)
        {
            Comment dbcomment = db.Comments.FirstOrDefault(c => c.CommentId == commentId);

            db.Comments.Remove(dbcomment);
            db.SaveChanges();

            return RedirectToAction("Details", new{starId = starId});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
