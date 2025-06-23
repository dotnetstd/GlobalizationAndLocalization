using System.Xml.Serialization;
using Xunit;

namespace Aurochses.Xunit.AspNetCore.Mvc.Localization.Tests
{
    public class ResxDataNodeTests
    {
        private readonly Resx.DataNode _resxDataNode;

        public ResxDataNodeTests()
        {
            _resxDataNode = new Resx.DataNode();
        }

        [Fact]
        public void Name_XmlAttributeAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<Resx.DataNode>("Name", typeof(XmlAttributeAttribute));

            AttributeAssert.ValidateProperty(propertyInfo, typeof(XmlAttributeAttribute), nameof(XmlAttributeAttribute.AttributeName), "name");
        }

        [Fact]
        public void Name_Success()
        {
            // Arrange
            const string value = "name";

            // Act
            _resxDataNode.Name = value;

            // Assert
            Assert.Equal(value, _resxDataNode.Name);
        }

        [Fact]
        public void Value_XmlElementAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<Resx.DataNode>("Value", typeof(XmlElementAttribute));

            AttributeAssert.ValidateProperty(propertyInfo, typeof(XmlElementAttribute), nameof(XmlElementAttribute.ElementName), "value");
        }

        [Fact]
        public void Value_Success()
        {
            // Arrange
            const string value = "value";

            // Act
            _resxDataNode.Value = value;

            // Assert
            Assert.Equal(value, _resxDataNode.Value);
        }
    }
}