using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace ProjectBlack.Controllers
{
    public abstract class ProjectBlackControllerBase : AbpController
    {
        protected ProjectBlackControllerBase()
        {
            LocalizationSourceName = ProjectBlackConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
