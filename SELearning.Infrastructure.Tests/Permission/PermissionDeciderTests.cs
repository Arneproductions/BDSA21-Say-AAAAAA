using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SELearning.Core.Permission;
using static SELearning.Core.Permission.Permission;
using System.Security.Claims;

namespace SELearning.Infrastructure.Tests;

public class PermissionDeciderTests
{
    public static TheoryData<IEnumerable<Rule>> PermissionDeciderFalseScenarios => new TheoryData<IEnumerable<Rule>> {
        { new List<Rule>{ o => Task.Run<bool>(() => false)} },
        { new List<Rule>{ o => Task.Run<bool>(() => true), o => Task.Run<bool>(() => false)} },
        { new List<Rule>{ o => Task.Run<bool>(() => true), o => Task.Run<bool>(() => false), o => Task.Run<bool>(() => true)} }
    };

    static ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, "brunkage.pebern�d") }));

    [Fact]
    public async Task IsAllowed_NoRulesForRequestedPermission_ReturnTrue()
    {
        // Arrange
        Dictionary<Permission, IEnumerable<Rule>> permissions = new();
        PermissionDecider permissionDecider = new PermissionDecider(permissions);

        // Act
        bool result = await permissionDecider.IsAllowed(user, CreateComment);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(PermissionDeciderFalseScenarios))]
    public async Task IsAllowed_OneOrMoreRulesEvaluatedToFalse_ReturnFalse(IEnumerable<Rule> rules)
    {
        // Arrange
        Dictionary<Permission, IEnumerable<Rule>> permissions = new();
        permissions.Add(CreateComment, rules);
        PermissionDecider permissionDecider = new PermissionDecider(permissions);

        // Act
        bool result = await permissionDecider.IsAllowed(user, CreateComment);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task IsAllowed_AllRulesEvaluatedToTrue_ReturnTrue()
    {
        // Arrange
        Dictionary<Permission, IEnumerable<Rule>> permissions = new();
        List<Func<object, Task<bool>>> rules = new List<Func<object, Task<bool>>>();
        rules.Add(o => Task.Run<bool>(() => true));
        rules.Add(o => Task.Run<bool>(() => true));
        rules.Add(o => Task.Run<bool>(() => true));
        rules.Add(o => Task.Run<bool>(() => true));

        PermissionDecider permissionDecider = new PermissionDecider(permissions);

        // Act
        bool result = await permissionDecider.IsAllowed(user, CreateComment);

        // Assert
        Assert.True(result);
    }
}