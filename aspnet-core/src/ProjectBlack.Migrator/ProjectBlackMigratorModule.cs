using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ProjectBlack.Configuration;
using ProjectBlack.EntityFrameworkCore;
using ProjectBlack.Migrator.DependencyInjection;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;

namespace ProjectBlack.Migrator;

[DependsOn(typeof(ProjectBlackEntityFrameworkModule))]
public class ProjectBlackMigratorModule : AbpModule
{
    private readonly IConfigurationRoot _appConfiguration;

    public ProjectBlackMigratorModule(ProjectBlackEntityFrameworkModule abpProjectNameEntityFrameworkModule)
    {
        abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

        _appConfiguration = AppConfigurations.Get(
            typeof(ProjectBlackMigratorModule).GetAssembly().GetDirectoryPathOrNull()
        );
    }

    public override void PreInitialize()
    {
        Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
            ProjectBlackConsts.ConnectionStringName
        );

        Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        Configuration.ReplaceService(
            typeof(IEventBus),
            () => IocManager.IocContainer.Register(
                Component.For<IEventBus>().Instance(NullEventBus.Instance)
            )
        );
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(ProjectBlackMigratorModule).GetAssembly());
        ServiceCollectionRegistrar.Register(IocManager);
    }
}
