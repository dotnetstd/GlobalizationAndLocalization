﻿using System;
using System.Collections.Generic;
using DbLocalizationProvider.Export;
using Xunit;

namespace DbLocalizationProvider.Tests.ExportTests
{
    public class SerializationTests
    {
        [Fact]
        public void TestSerialization()
        {
            var resources = new List<LocalizationResource>
                            {
                                new LocalizationResource
                                {
                                    Id = 1,
                                    Author = "migration-tool",
                                    ResourceKey = "test-key",
                                    ModificationDate = new DateTime(2016, 1, 1),
                                    Translations = new List<LocalizationResourceTranslation>
                                                   {
                                                       new LocalizationResourceTranslation
                                                       {
                                                           Id = 11,
                                                           Language = "en",
                                                           Value = "test value"
                                                       }
                                                   }
                                }
                            };

            var serializer = new JsonResourceExporter();
            var result = serializer.Export(resources);

            Assert.NotNull(result);
        }

        [Fact]
        public void TestDeserialization()
        {
            var input = @"[
  {
    ""id"": 1,
    ""resourceKey"": ""test-key"",
    ""modificationDate"": ""2016-01-01T00:00:00Z"",
    ""author"": ""migration-tool"",
    ""translations"": [
      {
        ""id"": 11,
        ""language"": ""en"",
        ""value"": ""test value""
      }
    ]
  }
]";

            var serializer = new JsonResourceExporter();
            var result = serializer.Deserialize<List<LocalizationResource>>(input);

            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
        }
    }
}
