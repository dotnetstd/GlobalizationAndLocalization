﻿using System.Linq;
using DbLocalizationProvider.Queries;
using DbLocalizationProvider.Sync;
using Xunit;

namespace DbLocalizationProvider.Tests.ForeignKnownResources
{
    public class ForeignResourceScannerTests
    {
        public ForeignResourceScannerTests()
        {
            ConfigurationContext.Current.TypeFactory.ForQuery<DetermineDefaultCulture.Query>().SetHandler<DetermineDefaultCulture.Handler>();
            ConfigurationContext.Current.ForeignResources.Add(new ForeignResourceDescriptor(typeof(ResourceWithNoAttribute)));
        }

        [Fact]
        public void DiscoverForeignResourceClass_SingleProperty()
        {
            var sut = new TypeDiscoveryHelper();

            var resources = sut.ScanResources(typeof(ResourceWithNoAttribute));

            Assert.True(resources.Any());

            var resource = resources.First();

            Assert.Equal("Default resource value", resource.Translations.DefaultTranslation());
            Assert.Equal("DbLocalizationProvider.Tests.ForeignKnownResources.ResourceWithNoAttribute.SampleProperty", resource.Key);
        }

        [Fact]
        public void DiscoverForeignResourceNestedClass()
        {
            var sut = new TypeDiscoveryHelper();

            var resources = sut.ScanResources(typeof(ResourceWithNoAttribute.NestedResource));

            Assert.True(resources.Any());

            var resource = resources.First();

            Assert.Equal("NestedProperty", resource.Translations.DefaultTranslation());
            Assert.Equal("DbLocalizationProvider.Tests.ForeignKnownResources.ResourceWithNoAttribute+NestedResource.NestedProperty", resource.Key);
        }

        [Fact]
        public void DiscoverForeignResource_Enum()
        {
            var sut = new TypeDiscoveryHelper();

            var resources = sut.ScanResources(typeof(SomeEnum));

            Assert.True(resources.Any());
            Assert.Equal(3, resources.Count());

            var resource = resources.First();

            Assert.Equal("None", resource.Translations.DefaultTranslation());
            Assert.Equal("DbLocalizationProvider.Tests.ForeignKnownResources.SomeEnum.None", resource.Key);
        }
    }

    public class ResourceWithNoAttribute
    {
        public static string SampleProperty => "Default resource value";

        public class NestedResource
        {
            public static string NestedProperty { get; set; }
        }
    }


    public enum SomeEnum
    {
        None,
        Some,
        Another
    }
}
