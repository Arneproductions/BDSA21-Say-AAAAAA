using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SELearning.Core.Credibility;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

public static class AuthorizationConfiguration
{
    public static PermissionBuilder AddPermissionAuthorization(this IServiceCollection services)
    {
        // Inject asp net Authorization handler and policy implementations
        services.AddScoped<IResourceAuthorizationPermissionService, ResourcePermissionService>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, ResourcePermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddSingleton<IProvider<ICredibilityService>, Provider<ICredibilityService>>();
        services.AddSingleton<IProvider<IPermissionCredibilityService>, Provider<IPermissionCredibilityService>>();

        return new(services);
    }
}