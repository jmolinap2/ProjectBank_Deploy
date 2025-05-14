using ProjectBlack.Configuration.Dto;
using System.Threading.Tasks;

namespace ProjectBlack.Configuration;

public interface IConfigurationAppService
{
    Task ChangeUiTheme(ChangeUiThemeInput input);
}
