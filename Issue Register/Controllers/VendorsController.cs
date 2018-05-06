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
using System.Net.Mail;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Issue_Register.Models.ViewModel;

namespace Issue_Register.Controllers
{
    public class VendorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "SuperAdmin, Vendors")]
        public ActionResult Index()
        {
            var vendors = db.Vendors.Include(v => v.Company);
            return View(vendors.ToList());
        }

        // GET: Vendors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        public ActionResult Create()
        {
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VendorVM viewModel)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            if (ModelState.IsValid)
            {
                try
                {
                        var vendor = new Vendor
                    {
                        Name = viewModel.Name,
                        VndrCode = viewModel.VndrCode,
                        CompanyId = viewModel.CompanyId,
                        Mobile = viewModel.Mobile,
                        Email = viewModel.Email,
                        IsConfirmed = false
                    };
                    db.Vendors.Add(vendor);
                    db.SaveChanges();
                    var user = new ApplicationUser();
                    user.UserName = viewModel.Email;
                    user.Email = viewModel.Email;
                    string password = viewModel.Password;
                    var newUser = UserManager.Create(user, password);
                    if (newUser.Succeeded)
                    {
                        var result = UserManager.AddToRole(user.Id, "Vendor");
                    }

                    using (MailMessage emailMessage = new MailMessage("ishtyaq.du@gmail.com", "ishtyaq11121058@gmail.com"))
                    {
                        emailMessage.Subject = "Varifiy new user";
                        string body = "Hello Admin,";
                        body += "<br /><br />Please click the following link to varified new user";
                        body += "<br /><a href = '" + string.Format("{0}://{1}/Vendors/Index", Request.Url.Scheme, Request.Url.Authority) + "'>Click here to varifying Page.</a>";
                        body += "<br /><br />Thanks";
                        emailMessage.Body = body;
                        emailMessage.IsBodyHtml = true;

                        SmtpClient MailClient = new SmtpClient();
                        MailClient.Send(emailMessage);
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error");
                }
               
                return RedirectToAction("Message");
            }

            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", viewModel.CompanyId);
            return View(viewModel);
        }

        public ActionResult Message()
        {
            return View();
        }

        // GET: Vendors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", vendor.CompanyId);
            return View(vendor);
        }

        // POST: Vendors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,VndrCode,CompanyId,Mobile,Email")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", vendor.CompanyId);
            return View(vendor);
        }

        // GET: Vendors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // POST: Vendors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vendor vendor = db.Vendors.Find(id);
            db.Vendors.Remove(vendor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public JsonResult ApproveVendor(int Id, bool Status)
        {
            bool flag = false;

            try
            {

                var old_vnd = db.Vendors.Where(x => x.Id == Id).FirstOrDefault();
                var vnd = db.Vendors.Where(x => x.Id == Id).FirstOrDefault();
                vnd.IsConfirmed = Status;
                db.Entry(old_vnd).CurrentValues.SetValues(vnd);
                db.SaveChanges();

                var vendEmail = db.Vendors.Where(x => x.Id == Id).Select(x => x.Email).FirstOrDefault();

                try
                {
                    using (MailMessage emailMessage = new MailMessage("ishtyaq.du@gmail.com", vendEmail))
                    {
                        emailMessage.Subject = "Confirmation from Admin";
                        string body = "Congratulations,";
                        body += "<br /><br />You are varified by admin";
                        body += "<br /><br />Please click the following link to login";
                        body += "<br /><a href = '" + string.Format("{0}://{1}/Account/Login", Request.Url.Scheme, Request.Url.Authority) + "'>Click here to varifying Page.</a>";
                        body += "<br /><br />Thanks";
                        emailMessage.Body = body;
                        emailMessage.IsBodyHtml = true;

                        SmtpClient MailClient = new SmtpClient();
                        MailClient.Send(emailMessage);
                    }
                }
                catch (Exception ex)
                {

                }
                flag = true;

            }
            catch (Exception ex)
            {

            }


            return Json(flag, JsonRequestBehavior.AllowGet);


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
