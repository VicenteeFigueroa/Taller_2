using Shortly.Domain.Entities;

namespace Shortly.Application.Interfaces;

public interface ILinkWriteRepository
{
    Task<Link?> GetByIdAsync(long id);
    Task<Link?> GetByShortUrlAsync(string shortUrl);
    Task AddAsync(Link link);
    Task SaveChangesAsync();
}
