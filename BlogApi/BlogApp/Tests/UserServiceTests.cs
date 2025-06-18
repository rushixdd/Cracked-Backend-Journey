using BlogApp.Services;
using BlogDomain.Entities;
using BlogInfrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace BlogApp.Tests;

public class UserServiceTests
{
    private readonly DbContextOptions<BlogDbContext> _dbOptions;
    private readonly Mock<IConfiguration> _mockConfig;

    public UserServiceTests()
    {
        _dbOptions = new DbContextOptionsBuilder<BlogDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _mockConfig = new Mock<IConfiguration>();
        _mockConfig.Setup(c => c["Jwt:Key"]).Returns("YourSuperSecretJwtKeyMustBeAtLeast256BitsLong");
    }

    [Fact]
    public async Task RegisterAsync_ShouldRegisterUser_WhenUsernameIsUnique()
    {
        using var context = new BlogDbContext(_dbOptions);
        var service = new UserService(context, _mockConfig.Object);

        var response = await service.RegisterAsync("newuser", "password");

        response.IsAuthenticated.Should().BeTrue();
        response.Message.Should().Be("Registration successful.");
        context.Users.Should().ContainSingle(u => u.Username == "newuser");
    }

    [Fact]
    public async Task RegisterAsync_ShouldFail_WhenUsernameAlreadyExists()
    {
        using var context = new BlogDbContext(_dbOptions);
        context.Users.Add(new User { Username = "existing", PasswordHash = "hashed" });
        context.SaveChanges();

        var service = new UserService(context, _mockConfig.Object);
        var response = await service.RegisterAsync("existing", "password");

        response.IsAuthenticated.Should().BeFalse();
        response.Message.Should().Be("Username already exists.");
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {
        using var context = new BlogDbContext(_dbOptions);
        var password = "password123";
        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        context.Users.Add(new User { Username = "validuser", PasswordHash = hash });
        context.SaveChanges();

        var service = new UserService(context, _mockConfig.Object);
        var response = await service.LoginAsync("validuser", password);

        response.IsAuthenticated.Should().BeTrue();
        response.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task LoginAsync_ShouldFail_WhenPasswordIsIncorrect()
    {
        using var context = new BlogDbContext(_dbOptions);
        var hash = BCrypt.Net.BCrypt.HashPassword("correctpassword");
        context.Users.Add(new User { Username = "user", PasswordHash = hash });
        context.SaveChanges();

        var service = new UserService(context, _mockConfig.Object);
        var response = await service.LoginAsync("user", "wrongpassword");

        response.IsAuthenticated.Should().BeFalse();
        response.Token.Should().BeNull();
        response.Message.Should().Be("Invalid credentials.");
    }

    [Fact]
    public async Task LoginAsync_ShouldFail_WhenUserDoesNotExist()
    {
        using var context = new BlogDbContext(_dbOptions);
        var service = new UserService(context, _mockConfig.Object);

        var response = await service.LoginAsync("nosuchuser", "any");

        response.IsAuthenticated.Should().BeFalse();
        response.Token.Should().BeNull();
        response.Message.Should().Be("Invalid credentials.");
    }
}
