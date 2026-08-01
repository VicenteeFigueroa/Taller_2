using Microsoft.Extensions.Logging;
using Shortly.Application.DTOs;
using Shortly.Application.Interfaces;

namespace Shortly.Application.Commands.Link;

public class CreateLinkCommandHandler
{
    private readonly ILinkWriteRepository _repository;
    private readonly ILogger<CreateLinkCommandHandler> _logger;

    public CreateLinkCommandHandler(ILinkWriteRepository repository, ILogger<CreateLinkCommandHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<LinkResponse> Handle(CreateLinkCommand command)
    {
        _logger.LogDebug("Creating link for URL: {Url} and userId: {UserId}", command.Url, command.UserId);

        var shortUrl = NUlid.Ulid.NewUlid().ToString()[..12].ToLowerInvariant();
        var link = new Shortly.Domain.Entities.Link(command.Url, shortUrl, command.UserId);

        await _repository.AddAsync(link);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Link created successfully with shortUrl: {ShortUrl} and id: {Id}.", link.ShortUrl, link.Id);
        return LinkResponse.From(link);
    }
}
