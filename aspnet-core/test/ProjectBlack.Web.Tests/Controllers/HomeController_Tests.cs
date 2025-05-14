using ProjectBlack.Models.TokenAuth;
using ProjectBlack.Web.Controllers;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace ProjectBlack.Web.Tests.Controllers;

public class HomeController_Tests : ProjectBlackWebTestBase
{
    [Fact]
    public async Task Index_Test()
    {
        await AuthenticateAsync(null, new AuthenticateModel
        {
            UserNameOrEmailAddress = "admin",
            Password = "123qwe"
        });

        //Act
        var response = await GetResponseAsStringAsync(
            GetUrl<HomeController>(nameof(HomeController.Index))
        );

        //Assert
        response.ShouldNotBeNullOrEmpty();
    }
}