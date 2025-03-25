using CMSModule.Models;

namespace CMSModule.DTOs;

public class ContentDto
{
    public string ContentType { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Body { get; set; }
    public List<string> Tags { get; set; }
    public ContentStatus Status { get; set; }
    public string Language { get; set; }
    public string CoverImageUrl { get; set; }

    // SEO Fields
    public string MetaTitle { get; set; }
    public string MetaDescription { get; set; }
    public string MetaKeywords { get; set; }
}
