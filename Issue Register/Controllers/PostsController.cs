using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Issue_Register.Models;
using Issue_Register.Models.Entity;
using Microsoft.AspNet.Identity;

namespace Issue_Register.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        [Authorize(Roles = "SuperAdmin, Employee, Vendor")]
        public ActionResult Index()
        {

            var posts = db.Posts.Include(p => p.Company).Include(p => p.Employee);

            var userName = User.Identity.GetUserName();
            bool Ehas = db.Employees.Any(x => x.Email == userName);
            if (Ehas)
            {
                var branch = db.Employees.Where(x => x.Email == userName).Select(b => b.Branch.Name).FirstOrDefault();
                posts = db.Posts.Where(x => x.Branch == branch).Include(p => p.Company).Include(p => p.Employee);
            }
            else
            {
               bool vHas = db.Vendors.Any(x => x.Email == userName);
                if (vHas)
                {
                    var companyId = db.Vendors.Where(x => x.Email == userName).Select(c => c.CompanyId).FirstOrDefault();
                    posts = db.Posts.Where(x => x.CompanyId == companyId).Include(p => p.Company).Include(p => p.Employee);
                }
            }

            var Statuslist = new List<SelectListItem>();

            Statuslist = new List<SelectListItem>
                            {
                                new SelectListItem{ Text="Posted", Value = "Posted" },
                                new SelectListItem{ Text="Done", Value = "Done" }
                            };

            ViewBag.Statuslist = Statuslist;
            ViewBag.Status = Statuslist;



            return View(posts.ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        [Authorize(Roles = "SuperAdmin, Employee")]
        public ActionResult Create()
        {
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name");
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post, HttpPostedFileBase upload)
        {
            var userName = User.Identity.GetUserName();
            var userId = db.Employees.Where(x => x.Email == userName).Select(x => x.Id).FirstOrDefault();
            if (upload != null && upload.ContentLength > 0)
            {
                using (var reader = new System.IO.BinaryReader(upload.InputStream))
                {
                    post.Photo = reader.ReadBytes(upload.ContentLength);
                }

            }
            if (ModelState.IsValid)
            {
                try
                {
                    post.DateTime = DateTime.Now;
                    post.Status = "Posted";
                    post.VarrifiedBy = "Not Solved Yet";
                    post.EmployeeId = userId;
                    db.Posts.Add(post);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error");
                }
                
            }
            var Statuslist = new List<SelectListItem>
                            {
                                new SelectListItem{ Text="Posted", Value = "Posted" },
                                new SelectListItem{ Text="Done", Value = "Done" }
                            };

            ViewBag.Statuslist = Statuslist;
            ViewBag.Status = Statuslist;
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", post.CompanyId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "Name", post.EmployeeId);
            return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize(Roles = "SuperAdmin, Employee")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            var Statuslist = new List<SelectListItem>
                            {
                                new SelectListItem{ Text="Posted", Value = "Posted" },
                                new SelectListItem{ Text="Done", Value = "Done" }
                            };

            ViewBag.Statuslist = Statuslist;
            ViewBag.Status = Statuslist;
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", post.CompanyId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "Name", post.EmployeeId);
            return View(post);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post, HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                using (var reader = new System.IO.BinaryReader(upload.InputStream))
                {
                    post.Photo = reader.ReadBytes(upload.ContentLength);
                }

            }
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(post).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error");
                }
                
            }
            var Statuslist = new List<SelectListItem>
                            {
                                new SelectListItem{ Text="Posted", Value = "Posted" },
                                new SelectListItem{ Text="Done", Value = "done" }
                            };

            ViewBag.Statuslist = Statuslist;
            ViewBag.Status = Statuslist;
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", post.CompanyId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "Name", post.EmployeeId);
            return View(post);
        }

        [Authorize(Roles = "SuperAdmin, Employee")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult UpdateStatus(int Id, string Status)
        {
            bool flag = false;

            try
            {

                var old_post_Data = db.Posts.Where(x => x.Id == Id).FirstOrDefault();
                var post = db.Posts.Where(x => x.Id == Id).FirstOrDefault();
                post.Status = Status;
                db.Entry(old_post_Data).CurrentValues.SetValues(post);
                db.SaveChanges();

                flag = true;

            }
            catch (Exception ex)
            {

            }


            return Json(flag, JsonRequestBehavior.AllowGet);


        }

        public JsonResult Verification(int Id)
        {
            bool flag = false;

            try
            {
                var userName = User.Identity.GetUserName();
                var Name = db.Vendors.Where(x => x.Email == userName).Select(x => x.Name).FirstOrDefault();

                var old_post_Data = db.Posts.Where(x => x.Id == Id).FirstOrDefault();
                var post = db.Posts.Where(x => x.Id == Id).FirstOrDefault();
                post.Status = "Need Varification";
                post.VarrifiedBy = Name;
                db.Entry(old_post_Data).CurrentValues.SetValues(post);
                db.SaveChanges();

                flag = true;

            }
            catch (Exception ex)
            {

            }


            return Json(flag, JsonRequestBehavior.AllowGet);


        }

        public PartialViewResult GetComments(int postId)
        {

            /*IQueryable<Comment>*/
            IQueryable<Comment> comments = db.Comments.Where(c => c.Post.Id == postId)
             .Select(c => new
             {
                 _Id = c.Id,
                 _CommentDate = c.CommentDate.Value,
                 _Message = c.Message,
                 _UserName = c.UserName

             }).AsEnumerable().Select(x => new Comment
             {
                 Id = x._Id,
                 CommentDate = x._CommentDate,
                 Message = x._Message,
                 UserName = x._UserName
             }).AsQueryable();

            return PartialView(@"~/Views/Shared/_MyComments.cshtml", comments);
        }
        [HttpPost]
        public ActionResult AddComment( string comment, int postId)
        {
            var userName = User.Identity.GetUserName();
            var Name = db.Vendors.Where(x => x.Email == userName).Select(x => x.Name).FirstOrDefault();
            if (Name == null)
            {
                Name = db.Employees.Where(x => x.Email == userName).Select(x => x.Name).FirstOrDefault();
                if (Name == null)
                {
                    Name = "Admin";
                }
            }

            Comment commentEntity = null;
            var post = db.Posts.FirstOrDefault(p => p.Id == postId);

            if (comment != null && post != null)
            {

                commentEntity = new Comment
                {
                    Message = comment,
                    CommentDate = DateTime.Now,
                    PostId = postId,
                    UserName = Name

                };

                db.Comments.Add(commentEntity);
                db.SaveChanges();

            }

            return RedirectToAction("GetComments", "Posts", new { postId = postId });
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
