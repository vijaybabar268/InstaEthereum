using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.Entity;

namespace InstaEthereum.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string WalletAddress { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("MySql_CS", throwIfV1Schema: false)
        {
            this.Configuration.ValidateOnSaveEnabled = false;
        }

        public static ApplicationDbContext Create()
        {
            DbConfiguration.SetConfiguration(new MySqlEFConfiguration());
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("aspnetroles");
            //modelBuilder.Entity<ApplicationUser>().ToTable("aspnetuserroles");
            //modelBuilder.Entity<ApplicationUser>().ToTable("aspnetusers");
            //modelBuilder.Entity<ApplicationUser>().ToTable("aspnetuserclaims");
            //modelBuilder.Entity<ApplicationUser>().ToTable("aspnetuserlogins");

            modelBuilder.Entity<ApplicationUser>().Property(t => t.Id).HasColumnName("id");
            //modelBuilder.Entity<ApplicationUser>().Property(t => t.Email).HasColumnName("email");
            //modelBuilder.Entity<ApplicationUser>().Property(t => t.EmailConfirmed).HasColumnName("emailconfirmed");
            //modelBuilder.Entity<ApplicationUser>().Property(t => t.PasswordHash).HasColumnName("passwordhash");
            //modelBuilder.Entity<ApplicationUser>().Property(t => t.SecurityStamp).HasColumnName("securitystamp");
            //modelBuilder.Entity<ApplicationUser>().Property(t => t.PhoneNumber).HasColumnName("phonenumber");
            //modelBuilder.Entity<ApplicationUser>().Property(t => t.PhoneNumberConfirmed).HasColumnName("phonenumberconfirmed");
            //modelBuilder.Entity<ApplicationUser>().Property(t => t.TwoFactorEnabled).HasColumnName("twofactorenabled");
            //modelBuilder.Entity<ApplicationUser>().Property(t => t.LockoutEndDateUtc).HasColumnName("lockoutenddateutc");
            //modelBuilder.Entity<ApplicationUser>().Property(t => t.LockoutEnabled).HasColumnName("lockoutenabled");
            //modelBuilder.Entity<ApplicationUser>().Property(t => t.AccessFailedCount).HasColumnName("accessfailedcount");
            //modelBuilder.Entity<ApplicationUser>().Property(t => t.UserName).HasColumnName("username");
            modelBuilder.Entity<ApplicationUser>().Property(t => t.WalletAddress).HasColumnName("WalletAddress");
            modelBuilder.Entity<ApplicationUser>().Property(t => t.WalletAddress).HasColumnName("Extent1.walletaddress");

            //modelBuilder.Properties().Where(x =>
            //        x.PropertyType.FullName != null &&
            //        (x.PropertyType.FullName.Equals("System.String") &&
            //        !x.GetCustomAttributes(false).OfType<ColumnAttribute>().Any(q => q.TypeName != null &&
            //        q.TypeName.Equals("varchar(max)", StringComparison.InvariantCultureIgnoreCase)))).Configure(c =>
            //        c.HasColumnType("varchar(65000)"));

            //modelBuilder.Properties().Where(x =>
            //        x.PropertyType.FullName != null &&
            //        (x.PropertyType.FullName.Equals("System.String") &&
            //        !x.GetCustomAttributes(false).OfType<ColumnAttribute>().Any(q => q.TypeName != null &&
            //        q.TypeName.Equals("nvarchar", StringComparison.InvariantCultureIgnoreCase)))).Configure(c =>
            //        c.HasColumnType("varchar"));
        }

        
    }


}