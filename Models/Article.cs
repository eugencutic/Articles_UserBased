using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Articles_UserBased.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Titlul este obligatoriu.")]
        [StringLength(20, ErrorMessage = "Titlul nu poate avea mai mult de 20 de caractere.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Continutul este obligatoriu.")]
        public string Content { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.DateTime)]
        public DateTime? Date { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Categoria este obligatorie.")]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        public string UserId { get; set; }
        
        [ForeignKey(nameof(UserId))]
        [DisplayName("Author")]
        public virtual ApplicationUser Author { get; set; }

        public virtual List<Comment> Comments { get; set; }
    }
}