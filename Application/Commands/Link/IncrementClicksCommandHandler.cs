using Microsoft.Extensions.Logging;
using Shortly.Application.DTOs;
using Shortly.Application.Interfaces;

namespace Shortly.Application.Commands.Link;

public class IncrementClicksCommandHandler
{
    private readonly ILinkWriteRepository _writeRepository;
    private readonly ILinkReadRepository _readRepository;
    private readonly ILogger<IncrementClicksCommandHandler> _logger;

    public IncrementClicksCommandHandler(ILinkWriteRepository writeRepository, ILinkReadRepository readRepository, ILogger<IncrementClicksCommandHandler> logger)
    {
        _writeRepository = writeRepository ?? throw new ArgumentNullException(nameof(writeRepository));
        _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<LinkResponse> Handle(IncrementClicksCommand command)
    {
        _logger.LogDebug("Incrementing clicks for linkId: {LinkId}", command.LinkId);

        var link = await _writeRepository.GetByIdAsync(command.LinkId);
        if (link is null)
        {
            _logger.LogWarning("IncrementClicks failed: No link found with id {LinkId}.", command.LinkId);
            throw new KeyNotFoundException($"No link found with id '{command.LinkId}'.");
        }

        link.IncrementClicks();
        await _writeRepository.SaveChangesAsync();

        // Syncs the updated click count into the read-optimized model
        await _readRepository.SyncAsync(link);

        _logger.LogInformation("Clicks incremented for linkId: {LinkId}. Total clicks: {Clicks}.", link.Id, link.Clicks);
        return LinkResponse.From(link);
    }
}