using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Module.CMS.Domain.ValueObjects;

namespace Myrtus.Clarity.Module.CMS.Domain.Entities;

public class Media : Entity
{
    public MediaUrl Url { get; private set; }
    public string Type { get; private set; } // e.g., "image/png", "video/mp4"

    private Media() { }

    public Media(Guid id, MediaUrl url, string type) : base(id)
    {
        Url = url;
        Type = type;
    }
}
