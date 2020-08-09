﻿using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.Entity;

namespace InstaEthereum.Models
{
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

        public DbSet<SetPrice> SetPrices { get; set; }
    }
}