using Myrtus.Clarity.Module.CMS.Domain.Entities;
using Myrtus.Clarity.Module.CMS.Domain.ValueObjects;
using Xunit;

namespace Myrtus.Clarity.Module.CMS.Domain.Tests.Unit.Entities
{
    public class TagTests
    {
        [Fact]
        public void Constructor_ShouldInitializeTag_WithValidData()
        {
            // Arrange
            var id = Guid.NewGuid();
            var name = "Technology";
            var slug = new Slug("technology");

            // Act
            var tag = new Tag(id, name, slug);

            // Assert
            Assert.Equal(id, tag.Id);
            Assert.Equal(name, tag.Name);
            Assert.Equal(slug.Value, tag.Slug.Value);
        }
    }
}
