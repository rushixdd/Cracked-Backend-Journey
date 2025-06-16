using BlogApi.Extensions;
using BlogApp.Interfaces;
using BlogApp.Services;
using BlogInfrastructure.Data;
using BlogInfrastructure.Logging;
using Microsoft.EntityFrameworkCore;
using Serilog;
using FluentValidation;
using FluentValidation.AspNetCore;
using BlogApp.Validators;

try
{
    SerilogLogger.Configure();
    Log.Information(LogMessages.AppStarting);
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

    builder.Services.AddDbContext<BlogDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddScoped<IBlogPostService, BlogPostService>();
    builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
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

public partial class Program
{
}