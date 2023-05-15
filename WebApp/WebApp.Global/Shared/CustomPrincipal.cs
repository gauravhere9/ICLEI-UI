using System.Security.Claims;
using System.Security.Principal;

namespace WebApp.Global.Shared
{
    public class CustomPrincipal : ClaimsPrincipal
    {
        public override IIdentity Identity { get; }

        public CustomPrincipal(string userName)
        {
            this.Identity = new GenericIdentity(userName);
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Token { get; set; }
    }
}
