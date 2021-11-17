using System.Security.Principal;

namespace Cards.Services
{
    public class HttpContextService
    {
        public HttpContextService(IHttpContextAccessor httpContextAccessor)
        {
            this.HttpContext = httpContextAccessor.HttpContext!;
        }

        public IIdentity CurrentUser => this.HttpContext.User.Identity!;

        public HttpContext HttpContext { get; init; }
    }
}
