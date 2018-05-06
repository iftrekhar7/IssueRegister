using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Issue_Register.Models.Entity
{
    public class Vendor
    {
        public int Id { get; set; }

        [Display(Name = "Full Name")]
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Vendor ID")]
        public string VndrCode { get; set; }

        [Required]
        [Display(Name = "Company ID")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [Required]
        public string Mobile { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public bool IsConfirmed { get; set; }
    }
}