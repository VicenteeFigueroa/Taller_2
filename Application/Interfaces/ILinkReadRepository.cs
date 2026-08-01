using Shortly.Domain.Entities;

namespace Shortly.Application.Interfaces;

public interface ILinkReadRepository
{
    Task<Link?> GetByIdAsync(long id);
    Task<Link?> GetByShortUrlAsync(string shortUrl);
    Task<List<Link>> GetAllAsync();
    Task<List<Link>> GetByUserIdAsync(long userId);
}
