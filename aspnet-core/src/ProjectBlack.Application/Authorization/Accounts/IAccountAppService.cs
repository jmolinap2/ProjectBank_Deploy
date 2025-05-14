using Abp.Application.Services;
using ProjectBlack.Authorization.Accounts.Dto;
using System.Threading.Tasks;

namespace ProjectBlack.Authorization.Accounts;

public interface IAccountAppService : IApplicationService
{
    Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

    Task<RegisterOutput> Register(RegisterInput input);
}
