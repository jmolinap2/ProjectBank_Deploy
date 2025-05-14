using Abp.Modules;
using Abp.Reflection.Extensions;
using ProjectBlack.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ProjectBlack.Web.Host.Startup
{
    [DependsOn(
       typeof(ProjectBlackWebCoreModule))]
    public class ProjectBlackWebHostModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public ProjectBlackWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ProjectBlackWebHostModule).GetAssembly());
        }
    }
}
