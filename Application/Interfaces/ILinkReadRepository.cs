using Shortly.Domain.Entities;

namespace Shortly.Application.Interfaces;

public interface ILinkReadRepository
{
    Task<LinkReadModel?> GetByIdAsync(long id);
    Task<LinkReadModel?> GetByShortUrlAsync(string shortUrl);
    Task<List<LinkReadModel>> GetAllAsync();
    Task<List<LinkReadModel>> GetByUserIdAsync(long userId);

    /// <summary>
    /// Upserts the read-optimized projection from a write-side Link entity.
    /// Called from the Command Handlers right after a write succeeds.
    /// </summary>
    Task SyncAsync(Link link);
}