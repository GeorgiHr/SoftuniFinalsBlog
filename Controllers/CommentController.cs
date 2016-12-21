using SoftuniFinalsBlog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SoftuniFinalsBlog.Controllers
{
    public class CommentController : Controller
    {

        public ActionResult Index()
        {
            using (var db = new BlogDbContext())
            {
                var comments = db.Comments
                    .Include(a => a.Article);

                return View(comments.ToList());

            }
        }
        //
        // GET: Comment/List
        public ActionResult List(int? id)
        {
            using (var db = new BlogDbContext())
            {
                var article = db.Articles
                    .Include(a => a.Author)
                    .Include(t => t.Tags)
                    .Include(c => c.Comments)
                    .Where(d => d.Id == id)
                    .FirstOrDefault();

                return View(article);
            }
        }
        //
        //GET: Comment/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var db = new BlogDbContext())
            {
                var comment = db.Comments
                    .FirstOrDefault(c => c.Id == id);

                if (comment == null)
                {
                    return HttpNotFound();
                }
                return View(comment);
            }
        }
        //
        //GET:Comment/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var comment = new Comment();
            comment.ArticleId = (int)id;
            return View(comment);
        }
        //
        //POST: Comment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,VisitorsComment,ArticleId")]Comment comment)
        {
            using (var db = new BlogDbContext())
            {
                if (ModelState.IsValid)
                {
                    db.Comments.Add(comment);
                    db.SaveChanges();
                    return RedirectToAction("List", new { id = comment.ArticleId });
                }

                ViewBag.ArticleId = new SelectList(db.Articles, "Id", "Title", comment.ArticleId);
                return View(comment);
            }
        }
        //
        //GET: Comment/Edit
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var db = new BlogDbContext())
            {
                var comment = db.Comments
                    .FirstOrDefault(c => c.Id == id);

                if (comment == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ArticleId = new SelectList(db.Articles, "Id", "Title", comment.ArticleId);
                return View(comment);
            }
        }
        //
        //POST: Comment/Edit
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,VisitorsComment,ArticleId")]Comment comment)
        {
            using (var db = new BlogDbContext())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(comment).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.ArticleId = new SelectList(db.Articles, "Id", "Title", comment.ArticleId);
                return View(comment);
            }
        }
        //
        //GET: Comment/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var db = new BlogDbContext())
            {
                var comment = db.Comments
                    .FirstOrDefault(c => c.Id == id);

                if (comment == null)
                {
                    return HttpNotFound();
                }

                return View(comment);
            }
        }
        //
        //POST: Comment/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            using (var db = new BlogDbContext())
            {
                var comment = db.Comments
                    .FirstOrDefault(c => c.Id == id);

                db.Comments.Remove(comment);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
        }
    }
}