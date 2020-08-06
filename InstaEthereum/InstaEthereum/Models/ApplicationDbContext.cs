using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.Entity;

namespace InstaEthereum.Models
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("MySql_CS")
        {            
        }

        public static ApplicationDbContext Create()
        {            
            return new ApplicationDbContext();
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }
        
        public DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
    }
}