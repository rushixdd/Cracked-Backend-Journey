using BlogApp.DTOs;

public interface IUserService
{
    Task<AuthResponse> RegisterAsync(string username, string password);
    Task<AuthResponse> LoginAsync(string username, string password);
}
