using RocketseatAuction.API.Contracts;
using RocketseatAuction.API.Entities;

namespace RocketseatAuction.API.Services;

public class LoggedUser : ILoggedUser
{
    private readonly IHttpContextAccessor _httpcontextAccessor;
    private readonly IUserRepository _repository;

    public LoggedUser(IHttpContextAccessor httpContext, IUserRepository repository)
    {
        _httpcontextAccessor = httpContext;
        _repository = repository;
    }

    public User User()
    {
        var token = TokenOnRequest();
        var email = FromBase64String(token);

        return _repository.GetUserByEmail(email);
    }

    private string TokenOnRequest()
    {
        var authentication = _httpcontextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        return authentication["Bearer ".Length..];
    }

    private string FromBase64String(string base64)
    {
        var data = Convert.FromBase64String(base64);

        return System.Text.Encoding.UTF8.GetString(data);
    }
}
