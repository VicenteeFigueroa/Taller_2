using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shortly.Application.Commands.Link;
using Shortly.Application.DTOs;
using Shortly.Application.Queries.Link;

namespace Shortly.Pages;

public class IndexModel : PageModel
{
    private readonly CreateLinkCommandHandler _createLinkHandler;
    private readonly GetLinksByUserIdQueryHandler _getLinksByUserIdHandler;

    public IndexModel(CreateLinkCommandHandler createLinkHandler, GetLinksByUserIdQueryHandler getLinksByUserIdHandler)
    {
        _createLinkHandler = createLinkHandler;
        _getLinksByUserIdHandler = getLinksByUserIdHandler;
    }

    [BindProperty]
    [Required]
    [Url]
    public string OriginalUrl { get; set; } = null!;

    public List<LinkResponse> Links { get; set; } = new();

    public async Task OnGetAsync()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim is not null && long.TryParse(userIdClaim, out var userId))
            {
                Links = await _getLinksByUserIdHandler.Handle(new GetLinksByUserIdQuery(userId));
            }
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (User.Identity?.IsAuthenticated != true)
            return Challenge();

        if (!ModelState.IsValid)
            return Page();

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim is null || !long.TryParse(userIdClaim, out var userId))
            return Challenge();

        await _createLinkHandler.Handle(new CreateLinkCommand(OriginalUrl, userId));
        return RedirectToPage();
    }
}