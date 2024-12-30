using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.Clarity.Module.CMS.Domain.ValueObjects
{
    public class MediaUrl : ValueObject
    {
        public string Value { get; }

        public MediaUrl(string value)
        {
            if (!Uri.IsWellFormedUriString(value, UriKind.Absolute))
                throw new ArgumentException("Invalid URL format.", nameof(value));

            Value = value;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
