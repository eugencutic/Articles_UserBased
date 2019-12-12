using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Articles_UserBased.DataAccess;
using Articles_UserBased.Models;
using Microsoft.AspNet.Identity;

namespace Articles_UserBased.Controllers
{
    [Authorize]
    public class ArticlesController : Controller
    {
        private ArticlesDbContext db = new ArticlesDbContext();

        // GET: Articles
        public ActionResult Index()
        {
            var articles = db.Articles.Include(a => a.Category).Include(a => a.Author);
            ViewData["CurrentUserId"] = User.Identity.GetUserId();
            ViewBag.CurrentUserId = User.Identity.GetUserId();
            return View(articles.ToList());
        }

        // GET: Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Include(a => a.Category).Include(a => a.Comments).Where(a => a.Id == id).First();
            ViewBag.CurrentUserId = User.Identity.GetUserId();
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // GET: Articles/Create
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            var article = new Article();
            article.Date = DateTime.Now;
            return View(article);
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Create([Bind(Include = "Id,Title,Content,Date,CategoryId")] Article article)
        {
            if (ModelState.IsValid)
            {
                if (!article.Date.HasValue)
                {
                    article.Date = DateTime.Now;
                }
                article.UserId = User.Identity.GetUserId();
                db.Articles.Add(article);
                db.SaveChanges();
                TempData["message"] = "Articol added.";
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", article.CategoryId);
            return View(article);
        }

        // GET: Articles/Edit/5
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", article.CategoryId);
            if (User.IsInRole("Administrator") || (article.UserId == User.Identity.GetUserId()))
            {
                return View(article);
            }
            else
            {
                TempData["message"] = "Warning: You do not own this article!";
                return RedirectToAction("Index");
            }
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Edit([Bind(Include = "Id,Title,Content,Date,CategoryId")] Article article)
        {
            if (ModelState.IsValid)
            {
                if (article.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
                {
                    db.Entry(article).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["message"] = "Article modified.";
                }
                else
                {
                    TempData["message"] = "Warning: You do not own this article!";
                }
                    
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", article.CategoryId);
            return View(article);
        }

        // GET: Articles/Delete/5
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Articles.Find(id);
            if (article.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                db.Articles.Remove(article);
                db.SaveChanges();
                TempData["message"] = "Article deleted.";
            }
            else
            {
                TempData["message"] = "Warning: You do not own this article!";
            }
               
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
