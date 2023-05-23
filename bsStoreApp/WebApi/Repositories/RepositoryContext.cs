using System;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Repositories
{
	public class RepositoryContext : DbContext
	{
		public RepositoryContext(DbContextOptions options) : base(options)
		{
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

		public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookConfig());
        }

    }
}

