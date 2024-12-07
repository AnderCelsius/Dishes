using Microsoft.AspNetCore.Authorization;

namespace Dishes.Common.Authorization;

public static class AuthorizationPolicies
{
    public static AuthorizationPolicy RequireAdminFromNigeria()
    {
        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole(Policies.Roles.Admin)
            .RequireClaim(CustomClaimTypes.Country, CustomClaimTypes.CustomClaims.Nigeria)
            .Build();
    }

}