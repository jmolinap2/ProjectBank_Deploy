using Xunit;

namespace ProjectBlack.Tests;

public sealed class MultiTenantFactAttribute : FactAttribute
{
    public MultiTenantFactAttribute()
    {
        if (!ProjectBlackConsts.MultiTenancyEnabled)
        {
            Skip = "MultiTenancy is disabled.";
        }
    }
}
