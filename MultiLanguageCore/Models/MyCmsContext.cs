using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MultiLanguageCore.Models
{
    public class MyCmsContext:DbContext
    {
        public MyCmsContext(DbContextOptions<MyCmsContext> options) : base(options)
        {

        }

        public DbSet<Language> Languages { get; set; }
        public DbSet<News> Newses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
          //  modelBuilder.Entity<News>().HasQueryFilter(n => n.Language.LanguageTitle == lang);

            base.OnModelCreating(modelBuilder);
        }
    }
}
