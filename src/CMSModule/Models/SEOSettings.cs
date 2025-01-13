using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Myrtus.Clarity.Core.Domain.Abstractions;

namespace CMSModule.Models;

public class SEOSettings
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } // Add this property

    public string DefaultMetaTitle { get; set; }
    public string DefaultMetaDescription { get; set; }
    public string DefaultMetaKeywords { get; set; }
}