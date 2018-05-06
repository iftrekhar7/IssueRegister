using Issue_Register.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Issue_Register.Models.ViewModel
{
    public class EmployeeVM
    {
        public int Id { get; set; }

        [Display(Name = "Full Name")]
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Employee ID")]
        public string EmpCode { get; set; }
        
        public int BranchId { get; set; }
        public IEnumerable<Branch> Branch { get; set; }
        

        public int DepartmentId { get; set; }
        public IEnumerable<Department> Departments { get; set; }

        [Required(ErrorMessage = "You must provide a phone number")]
        [Display(Name = "Personal Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^([0-9]{11})$", ErrorMessage = "Invalid Mobile Number.")]
        public string Mobile { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public bool IsConfirmed { get; set; }
    }
}