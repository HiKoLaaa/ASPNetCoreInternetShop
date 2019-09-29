using InternetShop.Models.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using InternetShop.Models.DbModels;
using InternetShop.Models.UnitOfWork;
using InternetShop.Models.ViewModels;

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
			services.AddSingleton<IUnitOfWork, UnitOfWork>();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddScoped<SessionCart>(opt => SessionCart.GetCart(opt));
			services.AddDbContext<ProductDbContext>(opt =>
			{
				opt.UseSqlServer(_configuration["Data:Databases:ProductDb"]);
			}, ServiceLifetime.Singleton);

			services.AddDbContext<AuthenticationDbContext>(opt =>
			{
				opt.UseSqlServer(_configuration["Data:Databases:IdentityDb"]);
			});

			services.AddIdentity<IdentityUser, IdentityRole>(opt => {
				opt.User.RequireUniqueEmail = true;
				opt.Password.RequiredUniqueChars = 0;
				opt.Password.RequireNonAlphanumeric = false;
			})
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
			app.UseMvc(routes =>
			{

				routes.MapRoute(
					name: "",
					template: "MyOrders/{ordNumber}",
					defaults: new { Controller = "Order", Action = "FullInfo" });

				routes.MapRoute(
					name: "",
					template: "MyOrders/",
					defaults: new { Controller = "Order", Action = "Index" });

				routes.MapRoute(
					name: "",
					template: "{controller=Shop}/{action=Index}");
			});
		}
	}
}