using Articles_UserBased.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Articles_UserBased.DataAccess
{
    public class ArticlesDbContext : IdentityDbContext<ApplicationUser>
    {
        public ArticlesDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {

        }

        public static ArticlesDbContext Create()
        {
            return new ArticlesDbContext();
        }

        public virtual DbSet<Article> Articles { get; set; }

        public virtual DbSet<Category> Categories { get; set; }
    }
}