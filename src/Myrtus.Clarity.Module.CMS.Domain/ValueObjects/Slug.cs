using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.Clarity.Module.CMS.Domain.ValueObjects;

public class Slug : ValueObject
{
    public string Value { get; }

    public Slug(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Slug cannot be empty.", nameof(value));

        Value = value.ToLower().Replace(" ", "-");
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
