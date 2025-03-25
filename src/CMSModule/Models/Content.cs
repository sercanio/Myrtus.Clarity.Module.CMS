using MongoDB.Bson;

using MongoDB.Bson.Serialization.Attributes;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Entity = Myrtus.Clarity.Core.Domain.Abstractions.Entity;

namespace CMSModule.Models;

public class Content
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string ContentType { get; set; } // e.g., "Page", "Post", "Article"

    public string Title { get; set; }

    public string Slug { get; set; }

    public string Body { get; set; }

    public List<string> Tags { get; set; } = new List<string>();

    public ContentStatus Status { get; set; } // Draft, Published, Archived, ReviewRequired

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public List<ContentVersion> Versions { get; set; } = new List<ContentVersion>();

    public string Language { get; set; } // For localization
    public string CoverImageUrl { get; set; }

    // SEO Fields
    public string MetaTitle { get; set; }
    public string MetaDescription { get; set; }
    public string MetaKeywords { get; set; }
}

public class ContentVersion
{
    public int VersionNumber { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string CoverImageUrl { get; set; }
    public DateTime ModifiedAt { get; set; }
    public string ModifiedBy { get; set; }
}

public enum ContentStatus
{
    Draft,
    Published,
    Archived,
    ReviewRequired
}
