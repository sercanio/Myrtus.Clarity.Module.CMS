using Myrtus.Clarity.Module.CMS.Domain.ValueObjects;

namespace Myrtus.Clarity.Module.CMS.Domain.Tests.Unit.Entities
{
    public class SlugTests
    {
        [Fact]
        public void Constructor_ShouldCreateSlug_WhenValueIsValid()
        {
            // Arrange
            var value = "sample-slug";

            // Act
            var slug = new Slug(value);

            // Assert
            Assert.Equal(value, slug.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Constructor_ShouldThrowArgumentException_WhenValueIsInvalid(string value)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Slug(value));
        }

        [Fact]
        public void Slug_ShouldConvertSpacesToDashes()
        {
            // Arrange
            var value = "Sample Slug";

            // Act
            var slug = new Slug(value);

            // Assert
            Assert.Equal("sample-slug", slug.Value);
        }
    }
}
