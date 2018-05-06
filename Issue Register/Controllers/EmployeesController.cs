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
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net.Mail;
using Issue_Register.Models.ViewModel;

namespace Issue_Register.Controllers
{

    public class EmployeesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "SuperAdmin, Employee")]
        public ActionResult Index()
        {
            var employees = db.Employees.Include(e => e.Branch).Include(e => e.Department);
            return View(employees.ToList());
        }

        [Authorize(Roles = "SuperAdmin, Employee")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }


        public ActionResult Create()
        {
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name");
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeVM viewModel)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            if (ModelState.IsValid)
            {
                try
                {
                        var employee = new Employee
                    {
                        Name = viewModel.Name,
                        EmpCode = viewModel.EmpCode,
                        BranchId = viewModel.BranchId,
                        DepartmentId = viewModel.DepartmentId,
                        Mobile = viewModel.Mobile,
                        Email = viewModel.Email,
                        IsConfirmed = false
                    };
                    db.Employees.Add(employee);
                    db.SaveChanges();

                    var user = new ApplicationUser();
                    user.UserName = viewModel.Email;
                    user.Email = viewModel.Email;
                    string password = viewModel.Password;
                    var newUser = UserManager.Create(user, password);
                    if (newUser.Succeeded)
                    {
                        var result = UserManager.AddToRole(user.Id, "Employee");
                    }
                
                    using (MailMessage emailMessage = new MailMessage("ishtyaq.du@gmail.com", "ishtyaq11121058@gmail.com"))
                    {
                        emailMessage.Subject = "Varifiy new user";
                        string body = "Hello Admin,";
                        body += "<br /><br />Please click the following link to varified new user";
                        body += "<br /><a href = '" + string.Format("{0}://{1}/Employees/Index", Request.Url.Scheme, Request.Url.Authority) + "'>Click here to varifying Page.</a>";
                        body += "<br /><br />Thanks";
                        emailMessage.Body = body;
                        emailMessage.IsBodyHtml = true;

                        SmtpClient MailClient = new SmtpClient();
                        MailClient.Send(emailMessage);
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index","Error");
                }
               
                return RedirectToAction("Message");
            }

            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name", viewModel.BranchId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", viewModel.DepartmentId);
            return View("Create",viewModel);
        }

        public ActionResult Message()
        {
            return View();
        }

        [Authorize(Roles = "SuperAdmin, Employee")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name", employee.BranchId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", employee.DepartmentId);
            return View(employee);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,EmpCode,BranchId,DepartmentId,Mobile,Email")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(employee).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error");
                }
                
            }
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name", employee.BranchId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", employee.DepartmentId);
            return View(employee);
        }

        [Authorize(Roles = "SuperAdmin, Employee")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult ApproveEmployee(int Id, bool Status)
        {
            bool flag = false;

            try
            {

                var old_emp = db.Employees.Where(x => x.Id == Id).FirstOrDefault();
                var emp = db.Employees.Where(x => x.Id == Id).FirstOrDefault();
                emp.IsConfirmed = Status;
                db.Entry(old_emp).CurrentValues.SetValues(emp);
                db.SaveChanges();
                var empEmail = db.Employees.Where(x => x.Id == Id).Select(x => x.Email).FirstOrDefault();
                try
                {
                    using (MailMessage emailMessage = new MailMessage("ishtyaq.du@gmail.com", empEmail))
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
