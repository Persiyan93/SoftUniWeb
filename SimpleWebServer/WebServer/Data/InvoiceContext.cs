using Microsoft.EntityFrameworkCore;
using System;

namespace Data
{
    public class InvoiceContext:DbContext
    {
        public InvoiceContext()
        {

        }
        public InvoiceContext(DbContextOptions options)
            : base(options)
        {


        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Filename=MyDatabase.db");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

         
        }
    }
}
