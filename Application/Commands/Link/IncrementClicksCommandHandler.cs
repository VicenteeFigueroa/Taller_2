using Microsoft.Extensions.Logging;
using Shortly.Application.DTOs;
using Shortly.Application.Interfaces;

namespace Shortly.Application.Commands.Link;

public class IncrementClicksCommandHandler
{
    private readonly ILinkWriteRepository _repository;
    private readonly ILogger<IncrementClicksCommandHandler> _logger;

    public IncrementClicksCommandHandler(ILinkWriteRepository repository, ILogger<IncrementClicksCommandHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<LinkResponse> Handle(IncrementClicksCommand command)
    {
        _logger.LogDebug("Incrementing clicks for linkId: {LinkId}", command.LinkId);

        var link = await _repository.GetByIdAsync(command.LinkId);
        if (link is null)
        {
            _logger.LogWarning("IncrementClicks failed: No link found with id {LinkId}.", command.LinkId);
            throw new KeyNotFoundException($"No link found with id '{command.LinkId}'.");
        }

        link.IncrementClicks();
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Clicks incremented for linkId: {LinkId}. Total clicks: {Clicks}.", link.Id, link.Clicks);
        return LinkResponse.From(link);
    }
}
