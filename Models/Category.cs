using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Articles_UserBased.Models
{
    public class Category
    {
        public int Id { get; set; }

        [DisplayName("Category name")]
        [Required(ErrorMessage = "Numele categoriei este obligatoriu.")]
        public string Name { get; set; }
    }
}