using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ProjectBlack.Authorization;
using ProjectBlack.CreditRequests.Dtos;
using ProjectBlack.CreditRequests;

namespace ProjectBlack;

[DependsOn(
    typeof(ProjectBlackCoreModule),
    typeof(AbpAutoMapperModule))]
public class ProjectBlackApplicationModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.Authorization.Providers.Add<ProjectBlackAuthorizationProvider>();
    }

    public override void Initialize()
    {
        var thisAssembly = typeof(ProjectBlackApplicationModule).GetAssembly();

        IocManager.RegisterAssemblyByConvention(thisAssembly);

        Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg =>
        {
            // Scan the assembly for classes which inherit from AutoMapper.Profile
            cfg.AddMaps(thisAssembly);

            // Entidad → DTO (lectura): todo mapeado
            cfg.CreateMap<CreditRequest, CreditRequestDto>();

            // DTO → Entidad (update): proteger campos auditables
            cfg.CreateMap<CreditRequestDto, CreditRequest>()
               .ForMember(dest => dest.CreationTime, opt => opt.Ignore())
               .ForMember(dest => dest.CreatorUserId, opt => opt.Ignore());
        });
    }
}
