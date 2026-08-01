using Microsoft.EntityFrameworkCore;
using Shortly.Application.Interfaces;
using Shortly.Domain.Entities;
using Shortly.Infrastructure.Persistence;

namespace Shortly.Infrastructure.Repositories;

public sealed class LinkReadRepository : ILinkReadRepository
{
    private readonly AppDbContext _context;

    public LinkReadRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task<LinkReadModel?> GetByIdAsync(long id)
        => _context.LinkReadModels.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);

    public Task<LinkReadModel?> GetByShortUrlAsync(string shortUrl)
        => _context.LinkReadModels.AsNoTracking().FirstOrDefaultAsync(l => l.ShortUrl == shortUrl);

    public Task<List<LinkReadModel>> GetAllAsync()
        => _context.LinkReadModels.AsNoTracking().ToListAsync();

    public Task<List<LinkReadModel>> GetByUserIdAsync(long userId)
        => _context.LinkReadModels.AsNoTracking().Where(l => l.UserId == userId).ToListAsync();

    public async Task SyncAsync(Link link)
    {
        var existing = await _context.LinkReadModels.FirstOrDefaultAsync(l => l.Id == link.Id);

        if (existing is null)
        {
            _context.LinkReadModels.Add(new LinkReadModel(link.Id, link.Url, link.ShortUrl, link.Clicks, link.UserId));
        }
        else
        {
            existing.UpdateClicks(link.Clicks);
        }

        await _context.SaveChangesAsync();
    }
}