using Microsoft.EntityFrameworkCore;
using Models;
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
        public DbSet<Company> Companies { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

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
