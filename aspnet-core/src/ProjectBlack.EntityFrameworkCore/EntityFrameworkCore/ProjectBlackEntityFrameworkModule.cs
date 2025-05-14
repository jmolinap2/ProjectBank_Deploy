using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using ProjectBlack.EntityFrameworkCore.Seed;

namespace ProjectBlack.EntityFrameworkCore;

[DependsOn(
    typeof(ProjectBlackCoreModule),
    typeof(AbpZeroCoreEntityFrameworkCoreModule))]
public class ProjectBlackEntityFrameworkModule : AbpModule
{
    /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
    public bool SkipDbContextRegistration { get; set; }

    public bool SkipDbSeed { get; set; }

    public override void PreInitialize()
    {
        if (!SkipDbContextRegistration)
        {
            Configuration.Modules.AbpEfCore().AddDbContext<ProjectBlackDbContext>(options =>
            {
                if (options.ExistingConnection != null)
                {
                    ProjectBlackDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                }
                else
                {
                    ProjectBlackDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                }
            });
        }
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(ProjectBlackEntityFrameworkModule).GetAssembly());
    }

    public override void PostInitialize()
    {
        if (!SkipDbSeed)
        {
            SeedHelper.SeedHostDb(IocManager);
        }
    }
}
