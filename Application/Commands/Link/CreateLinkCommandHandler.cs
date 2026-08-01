using Microsoft.Extensions.Logging;
using Shortly.Application.DTOs;
using Shortly.Application.Interfaces;

namespace Shortly.Application.Commands.Link;

public class CreateLinkCommandHandler
{
    private readonly ILinkWriteRepository _writeRepository;
    private readonly ILinkReadRepository _readRepository;
    private readonly ILogger<CreateLinkCommandHandler> _logger;

    public CreateLinkCommandHandler(ILinkWriteRepository writeRepository, ILinkReadRepository readRepository, ILogger<CreateLinkCommandHandler> logger)
    {
        _writeRepository = writeRepository ?? throw new ArgumentNullException(nameof(writeRepository));
        _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<LinkResponse> Handle(CreateLinkCommand command)
    {
        _logger.LogDebug("Creating link for URL: {Url} and userId: {UserId}", command.Url, command.UserId);

        var shortUrl = Ulid.NewUlid().ToString()[..12].ToLowerInvariant();
        var link = new Shortly.Domain.Entities.Link(command.Url, shortUrl, command.UserId);

        await _writeRepository.AddAsync(link);
        await _writeRepository.SaveChangesAsync();

        // Syncs the write-side entity into the read-optimized model
        await _readRepository.SyncAsync(link);

        _logger.LogInformation("Link created successfully with shortUrl: {ShortUrl} and id: {Id}.", link.ShortUrl, link.Id);
        return LinkResponse.From(link);
    }
}