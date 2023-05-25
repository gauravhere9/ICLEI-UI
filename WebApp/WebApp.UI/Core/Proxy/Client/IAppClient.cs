using WebApp.UI.Core.Proxy.Contracts;

namespace WebApp.UI.Core.Proxy.Client
{
    public interface IAppClient : IAuthService, ICompanyService, IBranchService, IDesignationService, IMasterService, IUserService
    {
    }
}
