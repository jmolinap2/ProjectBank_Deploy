using Abp.Dependency;
using ProjectBlack.Identity;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectBlack.Migrator.DependencyInjection;

public static class ServiceCollectionRegistrar
{
    public static void Register(IIocManager iocManager)
    {
        var services = new ServiceCollection();

        IdentityRegistrar.Register(services);

        WindsorRegistrationHelper.CreateServiceProvider(iocManager.IocContainer, services);
    }
}
