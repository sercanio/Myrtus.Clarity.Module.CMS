using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CMSModule.Models;

public class Media
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string FileName { get; set; }

    public string BlobUri { get; set; } // Azure Blob URI

    public string ContentType { get; set; } // e.g., "image/png"

    public long Size { get; set; }

    public DateTime UploadedAt { get; set; }

    public string UploadedBy { get; set; }
}
