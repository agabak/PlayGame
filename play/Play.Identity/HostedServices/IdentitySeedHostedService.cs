using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Play.Identity.Entities;
using Play.Identity.Settings;
using System.Threading;
using System.Threading.Tasks;

namespace Play.Identity.HostedServices
{
    public class IdentitySeedHostedService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IdentitySettings _identitySettings;

        public IdentitySeedHostedService(
            IServiceScopeFactory serviceScopeFactory,
            IOptions<IdentitySettings> options)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _identitySettings = options.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await CreateUserRole(_identitySettings.AdminUserEmail,
                                 _identitySettings.AdminUserPassword,
                                 Roles.Admin);  
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
       
        private  async Task CreateUserRole(string email,string password, string role)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var roleManager = scope.ServiceProvider
                                   .GetRequiredService<RoleManager<ApplicationRole>>();

            var userManager = scope.ServiceProvider
                                   .GetRequiredService<UserManager<ApplicationUser>>();

            await CreateRoleIfNotExistAsnyc(role, roleManager);

            // to be able to create player without creating user
            await CreateRoleIfNotExistAsnyc(Roles.Player, roleManager);

            var user = await userManager.FindByEmailAsync(email);

            if(user is null)
            {
                user = new ApplicationUser
                {
                    UserName = _identitySettings.AdminUserEmail,
                    Email = _identitySettings.AdminUserEmail
                };
              var  result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded) return;
                await userManager.AddToRoleAsync(user, role);
            }

            if(await userManager.IsInRoleAsync(user, role)) return;

                await userManager.AddToRoleAsync(user, role);
        }

        private static async Task CreateRoleIfNotExistAsnyc(string role,
            RoleManager<ApplicationRole> roleManager)
        {
            if(await roleManager.RoleExistsAsync(role)) return;

            await roleManager.CreateAsync(new ApplicationRole { Name = role });
        }
    }
}
