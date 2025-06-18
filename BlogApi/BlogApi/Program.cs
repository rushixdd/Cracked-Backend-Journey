using BlogApi.Extensions;
using BlogApp.Interfaces;
using BlogApp.Services;
using BlogInfrastructure.Auth;
using BlogInfrastructure.Data;
using BlogInfrastructure.Interfaces;
using BlogInfrastructure.Logging;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

try
{
    SerilogLogger.Configure();
    Log.Information(LogMessages.AppStarting);

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

    // ------------------- Services -------------------
    builder.Services.AddDbContext<BlogDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddScoped<IBlogPostService, BlogPostService>();
    builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
    builder.Services.AddScoped<IUserService, UserService>(); // <- Add this if not already done

    builder.Services.AddFluentValidationAutoValidation()
           .AddFluentValidationClientsideAdapters();

    // ------------------- JWT Setup -------------------
    var jwtSection = builder.Configuration.GetSection("Jwt");
    var jwtSettings = jwtSection.Get<JwtSettings>() ?? throw new InvalidOperationException("JWT settings missing");
    builder.Services.Configure<JwtSettings>(jwtSection);

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
            };
        });

    builder.Services.AddAuthorization();

    // ------------------- App -------------------
    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication(); // <- Important: must come before UseAuthorization
    app.UseAuthorization();

    app.MapControllers();
    app.UseCustomNotFound();
    app.UseGlobalExceptionHandler();

    app.Run();
}
catch (Exception ex) 
{
    Log.Fatal(ex, LogMessages.AppStartupFailed);
}
finally
{
    Log.CloseAndFlush();
}

// For Integration Testing
public partial class Program { }
