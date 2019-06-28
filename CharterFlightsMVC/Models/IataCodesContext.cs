using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharterFlightsMVC.Models
{
    public class IataCodesContext : DbContext
    {
        public class BloggingContext : DbContext
        {
            public BloggingContext(DbContextOptions<BloggingContext> options)
                : base(options)
            { }

            public DbSet<IataCode> Codes { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite("Data Source=iatacodes.db");
            }
        }

        public class IataCode
        {
            public string id { get; set; }
            public string text { get; set; }

        }

        
    }
}

