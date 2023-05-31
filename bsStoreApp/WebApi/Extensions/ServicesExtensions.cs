using System;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFCore;
using Services;
using Services.Contracts;

namespace WebApi.Extensions
{
	public static class ServicesExtensions
	{
		//Veritabanı bağlantı kaydı
		public static void ConfigureSqlContext(this IServiceCollection services,
			IConfiguration configuration) => services.AddDbContext<RepositoryContext>(options =>
				options.UseNpgsql(configuration.GetConnectionString("sqlConnection")));

		//Repository Kaydı
		public static void ConfigureRepositoryManager(this IServiceCollection services) =>
			services.AddScoped<IRepositoryManager, RepositoryManager>();

		//ServisManager kaydı
		public static void ConfigureServiceManager(this IServiceCollection services) =>
			services.AddScoped<IServiceManager, ServiceManager>();
    }
}

