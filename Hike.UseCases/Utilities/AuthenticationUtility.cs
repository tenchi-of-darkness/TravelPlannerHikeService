using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Hike.UseCases.Utilities;

public class AuthenticationUtility : IAuthenticationUtility
{
    private readonly HttpContext? _context;
    private readonly bool _isTest;

    public AuthenticationUtility(IHttpContextAccessor contextAccessor, IWebHostEnvironment environment)
    {
        _context = contextAccessor.HttpContext;
        _isTest = environment.IsEnvironment("IntegrationTest");
    }

    public string? GetUserId()
    {
        return _isTest ? "test" : _context?.User.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;
    }
}