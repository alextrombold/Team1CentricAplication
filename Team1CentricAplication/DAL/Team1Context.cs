using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Team1CentricAplication.Models;

namespace Team1CentricAplication.DAL
{
    public class Team1Context : DbContext
    {
        public Team1Context() : base("name=DefaultConnection")
        {

        }
        public DbSet<Profiles> Profiles { get; set; }

        public System.Data.Entity.DbSet<Team1CentricAplication.Models.Values> Values { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();  // note: this is all one line!
        }
    }
}