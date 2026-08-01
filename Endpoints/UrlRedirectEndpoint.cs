using Shortly.Application.Commands.Link;
using Shortly.Application.Queries.Link;

namespace Shortly.Endpoints;

public static class UrlRedirectEndpoint
{
    public static void MapUrlRedirect(this WebApplication app)
    {
        app.MapGet("/{shortUrl}", async (string shortUrl, GetLinkQueryHandler getLinkHandler, IncrementClicksCommandHandler incrementClicksHandler) =>
        {
            try
            {
                var link = await getLinkHandler.Handle(new GetLinkQuery(shortUrl));
                await incrementClicksHandler.Handle(new IncrementClicksCommand(link.Id));
                return Results.Redirect(link.Url);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        });
    }
}