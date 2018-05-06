using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Issue_Register.Models.Entity
{
    public class Post
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTime? DateTime { get; set; }

        public byte[] Photo { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Type of Problem")]
        public Type Type { get; set; }

        public string Branch { get; set; }

        public string Status { get; set; }

        public string VarrifiedBy { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
    }
    public enum Type
    {
        Hardware = 1,
        Software = 2
    }
}