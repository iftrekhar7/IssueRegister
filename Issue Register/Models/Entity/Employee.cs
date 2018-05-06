using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Issue_Register.Models.Entity
{
    public class Employee
    {
        public int Id { get; set; }

        [Display(Name = "Full Name")]
        [Required]
        public string  Name { get; set; }

        [Required]
        [Display(Name = "Employee ID")]
        public string EmpCode { get; set; }

        [Required]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }
        public virtual Branch Branch { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        [Required]
        public string Mobile { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public bool IsConfirmed { get; set; }
    }
}