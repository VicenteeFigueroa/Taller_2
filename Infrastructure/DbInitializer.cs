using Microsoft.EntityFrameworkCore;
using Shortly.Domain.Entities;
using Shortly.Infrastructure.Persistence;

namespace Shortly.Infrastructure;

public static class DbInitializer
{
    public static async Task InitializeAsync(AppDbContext db, string adminPassword)
    {
        if (await db.Users.AnyAsync())
            return;

        var user = new User("admin@shortly.disc.cl", adminPassword);

        db.Users.Add(user);
        await db.SaveChangesAsync();

        var aspnetLink = new Link("https://learn.microsoft.com/aspnet/core", "aspnet", user.Id);
        var efCoreLink = new Link("https://learn.microsoft.com/ef/core", "efcore", user.Id);
        var githubLink = new Link("https://github.com", "github", user.Id);

        db.Links.AddRange(aspnetLink, efCoreLink, githubLink);
        await db.SaveChangesAsync();

        // Syncs the seeded links into the read-optimized model
        db.LinkReadModels.AddRange(
            new LinkReadModel(aspnetLink.Id, aspnetLink.Url, aspnetLink.ShortUrl, aspnetLink.Clicks, aspnetLink.UserId),
            new LinkReadModel(efCoreLink.Id, efCoreLink.Url, efCoreLink.ShortUrl, efCoreLink.Clicks, efCoreLink.UserId),
            new LinkReadModel(githubLink.Id, githubLink.Url, githubLink.ShortUrl, githubLink.Clicks, githubLink.UserId)
        );
        await db.SaveChangesAsync();
    }
}