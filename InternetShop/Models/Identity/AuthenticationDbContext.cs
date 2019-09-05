using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InternetShop.Models.Identity
{
	public class AuthenticationDbContext : IdentityDbContext<IdentityUser>
	{
		public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> opt)
			: base(opt)
		{

		}
	}
}