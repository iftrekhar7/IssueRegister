using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Issue_Register.Models.Entity
{
    public class SubComment
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public DateTime? CommentDate { get; set; }

        public int CommentId { get; set; }
        public virtual Comment Comment { get; set; }

        public string UserName { get; set; }
    }
}