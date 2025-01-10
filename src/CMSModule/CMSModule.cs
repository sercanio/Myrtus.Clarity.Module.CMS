using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Myrtus.Clarity.Core.Application.Abstractions.Module;
using CMSModule.Repositories.ContentRepository;
using CMSModule.Repositories.MediaRepository;
using CMSModule.Repositories.SEORepository;
using CMSModule.Services.ContentService;
using CMSModule.Services.MediaService;
using CMSModule.Services.SEOService;
using MongoDB.Driver;
using System.Reflection;
using FluentValidation.AspNetCore;

namespace CMSModule;

public class CMSModule : IClarityModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // MongoDB Setup
        services.AddSingleton<IMongoClient>(sp =>
        {
            var connectionString = configuration.GetConnectionString("MongoDB");
            return new MongoClient(connectionString);
        });

        services.AddScoped<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            var databaseName = configuration.GetValue<string>("MongoDB:Database");
            return client.GetDatabase(databaseName);
        });

        // Register Repositories
        services.AddScoped<IContentRepository, ContentRepository>();
        services.AddScoped<IMediaRepository, MediaRepository>();
        services.AddScoped<ISEORepository, SEORepository>();

        // Register Services
        services.AddScoped<IContentService, ContentService>();
        services.AddScoped<IMediaService, MediaService>();
        services.AddScoped<ISEOService, SEOService>();

        // Register FluentValidation
        services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

        // Register MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }

    public void Configure(IApplicationBuilder app)
    {
        // Configure middleware specific to CMS if any
    }
}

