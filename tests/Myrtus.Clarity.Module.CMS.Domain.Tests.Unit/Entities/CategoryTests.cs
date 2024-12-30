using System;
using Myrtus.Clarity.Module.CMS.Domain.Entities;
using Myrtus.Clarity.Module.CMS.Domain.ValueObjects;
using Xunit;

namespace Myrtus.Clarity.Module.CMS.Domain.Tests.Unit.Entities
{
    public class CategoryTests
    {
        [Fact]
        public void Constructor_ShouldInitializeCategory_WithValidData()
        {
            // Arrange
            var id = Guid.NewGuid();
            var name = "Technology";
            var slug = new Slug("technology");

            // Act
            var category = new Category(id, name, slug);

            // Assert
            Assert.Equal(id, category.Id);
            Assert.Equal(name, category.Name);
            Assert.Equal(slug.Value, category.Slug.Value);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenNameIsEmpty()
        {
            // Arrange
            var id = Guid.NewGuid();
            var slug = new Slug("empty-name");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Category(id, string.Empty, slug));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenSlugIsNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var name = "Technology";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Category(id, name, null));
        }
    }
}
