namespace BlogApp.DTOs;

public class AuthResponse
{
    public bool IsAuthenticated { get; set; }
    public string? Token { get; set; }
    public string? Message { get; set; }
}
