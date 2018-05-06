using Issue_Register.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Issue_Register.Models.ViewModel
{
    public class VendorVM
    {
        public int Id { get; set; }

        [Display(Name = "Full Name")]
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Vendor ID")]
        public string VndrCode { get; set; }
        
        public int CompanyId { get; set; }
        public IEnumerable<Company> Company { get; set; }

        [Required(ErrorMessage = "You must provide a phone number")]
        [Display(Name = "Personal Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^([0-9]{11})$", ErrorMessage = "Invalid Mobile Number.")]
        public string Mobile { get; set; }

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