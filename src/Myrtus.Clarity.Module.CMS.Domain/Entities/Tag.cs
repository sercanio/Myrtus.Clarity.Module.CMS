using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Module.CMS.Domain.ValueObjects;

namespace Myrtus.Clarity.Module.CMS.Domain.Entities;

public class Tag : Entity
{
    public string Name { get; private set; }
    public Slug Slug { get; private set; }

    private Tag() { }

    public Tag(Guid id, string name, Slug slug) : base(id)
    {
        Name = name;
        Slug = slug;
    }
}
