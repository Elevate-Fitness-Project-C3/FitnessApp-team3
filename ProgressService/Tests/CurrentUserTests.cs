using System.Security.Claims;
using ProgressService.Feature.Shared;
using Xunit;

namespace ProgressService.Tests;

public sealed class CurrentUserTests
{
    [Fact]
    public void TryGetId_UsesSubjectClaim()
    {
        var expected = Guid.NewGuid();
        var principal = new ClaimsPrincipal(new ClaimsIdentity(
        [new Claim("sub", expected.ToString())],
        "Bearer"));

        var result = CurrentUser.TryGetId(principal, out var actual);

        Assert.True(result);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TryGetId_UsesNameIdentifierWhenSubjectIsMissing()
    {
        var expected = Guid.NewGuid();
        var principal = new ClaimsPrincipal(new ClaimsIdentity(
        [new Claim(ClaimTypes.NameIdentifier, expected.ToString())],
        "Bearer"));

        var result = CurrentUser.TryGetId(principal, out var actual);

        Assert.True(result);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TryGetId_ReturnsFalseWhenIdentityDoesNotContainGuid()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(
        [new Claim("sub", "not-a-guid")],
        "Bearer"));

        var result = CurrentUser.TryGetId(principal, out var actual);

        Assert.False(result);
        Assert.Equal(Guid.Empty, actual);
    }
}
