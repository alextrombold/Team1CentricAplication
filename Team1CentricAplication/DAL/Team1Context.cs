using Team1CentricAplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Team1CentricAplication.DAL
{
    public class Team1Context : DbContext    
    {
        public Team1Context() : base("name=DefaultConnection")    
        {

        }
        public DbSet<Profiles> Profiles { get; set; }
    }
}