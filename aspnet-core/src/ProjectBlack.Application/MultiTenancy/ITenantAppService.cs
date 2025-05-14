using Abp.Application.Services;
using ProjectBlack.MultiTenancy.Dto;

namespace ProjectBlack.MultiTenancy;

public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
{
}

