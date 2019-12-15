using Articles_UserBased.DataAccess;
using Articles_UserBased.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Articles_UserBased.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private ArticlesDbContext db = new ArticlesDbContext();

        // GET: Comments/Create
        public ActionResult Create(int articleId)
        {
            var newComment = new Comment();
            newComment.ArticleId = articleId;

            return View(newComment);
        }

        // POST: Comments/Create
        [HttpPost]
        public ActionResult Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.UserId = User.Identity.GetUserId();
                comment.CreationTime = DateTime.Now;

                db.Comments.Add(comment);
                db.SaveChanges();
                TempData["message"] = "Comment added.";
                return RedirectToAction("Details", "Articles", new { id = comment.ArticleId } );
            }

            return View(comment);
        }

        // GET: Comments/Edit/5
        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }

            if (comment.UserId == User.Identity.GetUserId())
            {
                return View(comment);
            }
            else
            {
                TempData["message"] = "Warning: You do not own this comment!";
                return RedirectToAction("Details", "Articles", new { id = comment.ArticleId });
            }
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult Edit(Comment comment)
        {
            if (ModelState.IsValid)
            {
                if (comment.UserId == User.Identity.GetUserId())
                {
                    db.Entry(comment).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["message"] = "Comment modified.";
                }
                else
                {
                    TempData["message"] = "Warning: You do not own this comment!";
                }

                return RedirectToAction("Details", "Articles", new { id = comment.ArticleId });
            }

            return View(comment);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            var articleId = comment.ArticleId;
            if (comment.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                db.Comments.Remove(comment);
                db.SaveChanges();
                TempData["message"] = "Comment deleted.";
            }
            else
            {
                TempData["message"] = "Warning: You do not own this comment!";
            }

            return RedirectToAction("Details", "Articles", new { id = articleId });
        }
    }
}
