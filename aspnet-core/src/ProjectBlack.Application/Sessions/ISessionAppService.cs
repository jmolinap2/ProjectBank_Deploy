using Abp.Application.Services;
using ProjectBlack.Sessions.Dto;
using System.Threading.Tasks;

namespace ProjectBlack.Sessions;

public interface ISessionAppService : IApplicationService
{
    Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
}
