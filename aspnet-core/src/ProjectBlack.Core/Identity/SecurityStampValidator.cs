using Abp.Authorization;
using Abp.Domain.Uow;
using ProjectBlack.Authorization.Roles;
using ProjectBlack.Authorization.Users;
using ProjectBlack.MultiTenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ProjectBlack.Identity;

public class SecurityStampValidator : AbpSecurityStampValidator<Tenant, Role, User>
{
    public SecurityStampValidator(
        IOptions<SecurityStampValidatorOptions> options,
        SignInManager signInManager,
        ILoggerFactory loggerFactory,
        IUnitOfWorkManager unitOfWorkManager)
        : base(options, signInManager, loggerFactory, unitOfWorkManager)
    {
    }
}
