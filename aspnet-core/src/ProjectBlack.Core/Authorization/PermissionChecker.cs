using Abp.Authorization;
using ProjectBlack.Authorization.Roles;
using ProjectBlack.Authorization.Users;

namespace ProjectBlack.Authorization;

public class PermissionChecker : PermissionChecker<Role, User>
{
    public PermissionChecker(UserManager userManager)
        : base(userManager)
    {
    }
}
