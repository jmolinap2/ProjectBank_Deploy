using Abp.Authorization;
using Abp.Runtime.Session;
using ProjectBlack.Configuration.Dto;
using System.Threading.Tasks;

namespace ProjectBlack.Configuration;

[AbpAuthorize]
public class ConfigurationAppService : ProjectBlackAppServiceBase, IConfigurationAppService
{
    public async Task ChangeUiTheme(ChangeUiThemeInput input)
    {
        await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
    }
}
