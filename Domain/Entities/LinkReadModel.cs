using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shortly.Domain.Entities;

/// <summary>
/// Read-optimized projection of a Link, kept in sync by the write-side Command Handlers.
/// ILinkReadRepository must read exclusively from this model, never from Links.
/// </summary>
[Table("links_read")]
[Index(nameof(ShortUrl), IsUnique = true)]
public class LinkReadModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; private set; }

    [Required]
    [MaxLength(20248)]
    public string Url { get; private set; } = null!;

    [Required]
    [MaxLength(32)]
    public string ShortUrl { get; private set; } = null!;

    [Required]
    public int Clicks { get; private set; }

    public long UserId { get; private set; }

    private LinkReadModel()
    {
    }

    public LinkReadModel(long id, string url, string shortUrl, int clicks, long userId)
    {
        Id = id;
        Url = url;
        ShortUrl = shortUrl;
        Clicks = clicks;
        UserId = userId;
    }

    public void UpdateClicks(int clicks) => Clicks = clicks;
}