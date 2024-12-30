using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.Clarity.Module.CMS.Domain.ValueObjects;

public class ArticleTitle : ValueObject
{
    public string Value { get; }

    public ArticleTitle(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Title cannot be empty.", nameof(value));

        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
