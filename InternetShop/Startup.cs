using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetShop.Models.Repository;
using InternetShop.Models.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

namespace InternetShop
{
	public class Startup
	{
		private IConfiguration _configuration;

		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddDbContext<ProductDbContext>(opt =>
			{
				opt.UseSqlServer(_configuration["Data:Databases:ProductDb"]);
			});

			services.AddDbContext<AuthenticationDbContext>(opt =>
			{
				opt.UseSqlServer(_configuration["Data:Databases:IdentityDb"]);
			});

			services.AddIdentity<IdentityUser, IdentityRole>()
				.AddEntityFrameworkStores<AuthenticationDbContext>()
				.AddDefaultTokenProviders();

			services.AddMemoryCache();
			services.AddSession();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseDeveloperExceptionPage();
			app.UseStatusCodePages();
			app.UseStaticFiles();
			app.UseSession();
			app.UseAuthentication();
			app.UseMvcWithDefaultRoute();
		}
	}
}