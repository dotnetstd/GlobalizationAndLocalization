using System.Collections.Generic;
using System.Xml.Serialization;
using Xunit;

namespace Aurochses.Xunit.AspNetCore.Mvc.Localization.Tests
{
    public class ResxTests
    {
        private readonly Resx _resx;

        public ResxTests()
        {
            _resx = new Resx();
        }

        [Fact]
        public void XmlRootAttribute_Defined()
        {
            // Arrange & Act & Assert
            var attribute = TypeAssert.HasAttribute<Resx, XmlRootAttribute>();

            Assert.Equal("root", attribute.ElementName);
        }

        [Fact]
        public void Data_XmlElementAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<Resx>("Data", typeof(XmlElementAttribute));

            AttributeAssert.ValidateProperty(propertyInfo, typeof(XmlElementAttribute), nameof(XmlElementAttribute.ElementName), "data");
        }

        [Fact]
        public void Data_Success()
        {
            // Arrange
            var value = new List<Resx.DataNode>();

            // Act
            _resx.Data = value;

            // Assert
            Assert.Equal(value, _resx.Data);
        }
    }
}