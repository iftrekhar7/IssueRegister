﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Issue_Register.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult AssignRole()
        {
            return View();
        }
    }
}