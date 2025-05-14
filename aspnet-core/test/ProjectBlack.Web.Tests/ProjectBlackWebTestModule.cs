using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ProjectBlack.EntityFrameworkCore;
using ProjectBlack.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace ProjectBlack.Web.Tests;

[DependsOn(
    typeof(ProjectBlackWebMvcModule),
    typeof(AbpAspNetCoreTestBaseModule)
)]
public class ProjectBlackWebTestModule : AbpModule
{
    public ProjectBlackWebTestModule(ProjectBlackEntityFrameworkModule abpProjectNameEntityFrameworkModule)
    {
        abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
    }

    public override void PreInitialize()
    {
        Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(ProjectBlackWebTestModule).GetAssembly());
    }

    public override void PostInitialize()
    {
        IocManager.Resolve<ApplicationPartManager>()
            .AddApplicationPartsIfNotAddedBefore(typeof(ProjectBlackWebMvcModule).Assembly);
    }
}