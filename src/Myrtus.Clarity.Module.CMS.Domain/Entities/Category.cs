using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Module.CMS.Domain.ValueObjects;

namespace Myrtus.Clarity.Module.CMS.Domain.Entities;

public class Category : Entity
{
    public string Name { get; private set; }
    public Slug Slug { get; private set; }

    public Category(Guid id, string name, Slug slug) : base(id)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Category name cannot be empty or whitespace.", nameof(name));
        }

        Name = name ?? throw new ArgumentNullException(nameof(name));
        Slug = slug ?? throw new ArgumentNullException(nameof(slug));
    }
}
