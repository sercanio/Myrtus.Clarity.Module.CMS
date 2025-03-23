using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Myrtus.Clarity.Core.Application.Abstractions.Module;
using CMSModule.Repositories.ContentRepository;
using CMSModule.Repositories.MediaRepository;
using CMSModule.Repositories.SeoRepository;
using CMSModule.Services.ContentService;
using CMSModule.Services.MediaService;
using CMSModule.Services.SEOService;
using MongoDB.Driver;
using System.Reflection;
using FluentValidation.AspNetCore;
using EcoFind.Infrastructure;
using EcoFind.Domain.Roles;

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
        services.AddScoped<ISeoRepository, SeoRepository>();

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
        // 1) We only do this if we know we have EF or an ApplicationDbContext in the main app:
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
        if (dbContext == null)
            return; // If your context isn't available, skip

        // 2) Our new CMS permissions
        var cmsPermissions = new[]
        {
        CMSPermissions.ContentRead,
        CMSPermissions.ContentCreate,
        CMSPermissions.ContentUpdate,
        CMSPermissions.ContentDelete,
        CMSPermissions.MediaRead,
        CMSPermissions.MediaCreate,
        CMSPermissions.MediaUpdate,
        CMSPermissions.MediaDelete,
        CMSPermissions.SEORead,
        CMSPermissions.SEOCreate,
        CMSPermissions.SEOUpdate,
        CMSPermissions.SEODelete,
    };

        // 3) For each new permission, check if it’s already in the DB
        foreach (var perm in cmsPermissions)
        {
            bool exists = dbContext.Set<Permission>().Any(x => x.Id == perm.Id);
            if (!exists)
            {
                dbContext.Set<Permission>().Add(perm);
            }
        }

        dbContext.SaveChanges();
    }
}

