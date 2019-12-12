using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Articles_UserBased.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [StringLength(200, ErrorMessage = "Comments can't be more than 200 characters.")]
        public string Text { get; set; }

        public DateTime CreationTime { get; set; }

        public int ArticleId { get; set; }
        
        public string UserId { get; set; }

        [ForeignKey(nameof(ArticleId))]
        public virtual Article Article { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }
    }
}