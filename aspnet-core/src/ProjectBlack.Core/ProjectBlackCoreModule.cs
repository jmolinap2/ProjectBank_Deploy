using Abp.Localization;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Security;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using ProjectBlack.Authorization.Roles;
using ProjectBlack.Authorization.Users;
using ProjectBlack.Configuration;
using ProjectBlack.Localization;
using ProjectBlack.MultiTenancy;
using ProjectBlack.Timing;

namespace ProjectBlack;

[DependsOn(typeof(AbpZeroCoreModule))]
public class ProjectBlackCoreModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.Auditing.IsEnabledForAnonymousUsers = true;

        // Declare entity types
        Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
        Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
        Configuration.Modules.Zero().EntityTypes.User = typeof(User);

        ProjectBlackLocalizationConfigurer.Configure(Configuration.Localization);

        // Enable this line to create a multi-tenant application.
        Configuration.MultiTenancy.IsEnabled = ProjectBlackConsts.MultiTenancyEnabled;

        // Configure roles
        AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

        Configuration.Settings.Providers.Add<AppSettingProvider>();

        Configuration.Localization.Languages.Add(new LanguageInfo("fa", "فارسی", "famfamfam-flags ir"));

        Configuration.Settings.SettingEncryptionConfiguration.DefaultPassPhrase = ProjectBlackConsts.DefaultPassPhrase;
        SimpleStringCipher.DefaultPassPhrase = ProjectBlackConsts.DefaultPassPhrase;
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(ProjectBlackCoreModule).GetAssembly());
    }

    public override void PostInitialize()
    {
        IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
    }
}
