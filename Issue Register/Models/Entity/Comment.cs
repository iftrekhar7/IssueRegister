using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Issue_Register.Models.Entity
{
    public class Comment
    {
        
        public int Id { get; set; }

        public string Message { get; set; }

        public DateTime? CommentDate { get; set; }

        public int PostId { get; set; }
        public virtual Post Post { get; set; }

        public string UserName { get; set; }
    }
}