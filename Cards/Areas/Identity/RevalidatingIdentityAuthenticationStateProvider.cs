using System.Security.Claims;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Cards.Areas.Identity
{
    public class RevalidatingIdentityAuthenticationStateProvider<TUser>
        : RevalidatingServerAuthenticationStateProvider where TUser : class
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IdentityOptions _options;

        public RevalidatingIdentityAuthenticationStateProvider(
            ILoggerFactory loggerFactory,
            IServiceScopeFactory scopeFactory,
            IOptions<IdentityOptions> optionsAccessor)
            : base(loggerFactory)
        {
            this._scopeFactory = scopeFactory;
            this._options = optionsAccessor.Value;
        }

        protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

        protected override async Task<bool> ValidateAuthenticationStateAsync(
            AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            // Get the user manager from a new scope to ensure it fetches fresh data
            IServiceScope? scope = this._scopeFactory.CreateScope();
            try
            {
                UserManager<TUser>? userManager = scope.ServiceProvider.GetRequiredService<UserManager<TUser>>();
                return await this.ValidateSecurityStampAsync(userManager, authenticationState.User);
            } finally
            {
                if (scope is IAsyncDisposable asyncDisposable)
                {
                    await asyncDisposable.DisposeAsync();
                } else
                {
                    scope.Dispose();
                }
            }
        }

        private async Task<bool> ValidateSecurityStampAsync(UserManager<TUser> userManager, ClaimsPrincipal principal)
        {
            TUser? user = await userManager.GetUserAsync(principal);
            if (user == null)
            {
                return false;
            } else if (!userManager.SupportsUserSecurityStamp)
            {
                return true;
            } else
            {
                string? principalStamp = principal.FindFirstValue(this._options.ClaimsIdentity.SecurityStampClaimType);
                string? userStamp = await userManager.GetSecurityStampAsync(user);
                return principalStamp == userStamp;
            }
        }
    }
}