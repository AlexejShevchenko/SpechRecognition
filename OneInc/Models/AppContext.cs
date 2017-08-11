using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OneInc.Models
{
    public class AppContext : DbContext
    {

        public AppContext()
            :base("AppConnection")
        { }

        public DbSet<Question> Question { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Option> Options { get; set; }
    }
}