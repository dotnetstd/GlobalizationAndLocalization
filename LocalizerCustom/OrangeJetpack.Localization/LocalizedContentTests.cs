﻿using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OrangeJetpack.Localization.Tests
{
    // ReSharper disable InconsistentNaming
    // ReSharper disable PossibleNullReferenceException

    public class LocalizedContentTests
    {
        public class TestClassA : ILocalizable
        {
            [Localized]
            public string PropertyA { get; set; }

            [Localized]
            public string PropertyB { get; set; }

            public string PropertyC { get; set; } = "NOT LOCALIZED";

            public TestClassA ChildA { get; set; }
            public TestClassB ChildB { get; set; }

            public ICollection<TestClassA> ChildrenA { get; set; }
            public ICollection<TestClassB> ChildrenB { get; set; }
            public ICollection<object> NonLocalizedCollection { get; set; } = new object[] { "I'M", "NOT", "LOCALIZED" };
        }

        public class TestClassB : ILocalizable
        {
            [Localized]
            public string PropertyA { get; set; }

            [Localized]
            public string PropertyB { get; set; }

            public string PropertyC { get; set; } = "NOT LOCALIZED";
        }

        private const string DEFAULT_LANGUAGE = "en";
        private const string OTHER_LANGUAGE = "zz";
        private const string ANY_STRING_1 = "ABC123";
        private const string ANY_STRING_2 = "XYZ789";

        private static string GetLocalizedContent()
        {
            var localizedContent = new[]
            {
                new LocalizedContent(DEFAULT_LANGUAGE, ANY_STRING_1),
                new LocalizedContent(OTHER_LANGUAGE, ANY_STRING_2)
            };
            return localizedContent.Serialize();
        }

        [Fact]
        public void LocalizeItem_NoPropertiesSpecified_LocalizesAllProperties()
        {
            var localizedContent = GetLocalizedContent();

            var testClass1 = new TestClassA
            {
                PropertyA = localizedContent,
                PropertyB = localizedContent
            };

            var testClass2 = new TestClassA
            {
                PropertyA = localizedContent,
                PropertyB = localizedContent
            };

            testClass1.Localize(DEFAULT_LANGUAGE);

            Assert.Equal(ANY_STRING_1, testClass1.PropertyA);
            Assert.Equal(ANY_STRING_1, testClass1.PropertyB);

            testClass2.Localize(OTHER_LANGUAGE);

            Assert.Equal(ANY_STRING_2, testClass2.PropertyA);
            Assert.Equal(ANY_STRING_2, testClass2.PropertyB);
        }

        [Fact]
        public void LocalizeItem_WithLocalizableChildren_LocalizesChildren()
        {
            var localizedContent = GetLocalizedContent();

            var testClass = new TestClassA
            {
                ChildA = new TestClassA
                {
                    PropertyA = localizedContent,
                    ChildA = new TestClassA
                    {
                        PropertyA = localizedContent
                    }
                }
            };

            var localized = testClass.Localize(DEFAULT_LANGUAGE, LocalizationDepth.Deep);

            Assert.Equal(ANY_STRING_1, localized.ChildA.PropertyA);
            Assert.Equal(ANY_STRING_1, localized.ChildA.ChildA.PropertyA);
        }

        [Fact]
        public void LocalizeItem_LocalizationDepthIsShallow_DoesNotLocalizeChildred()
        {
            var localizedContent = GetLocalizedContent();

            var testClass = new TestClassA
            {
                ChildA = new TestClassA
                {
                    PropertyA = localizedContent,
                    PropertyB = localizedContent
                }
            };

            var localized = testClass.Localize(DEFAULT_LANGUAGE);

            Assert.NotEqual(ANY_STRING_1, localized.ChildA.PropertyA);
            Assert.NotEqual(ANY_STRING_1, localized.ChildA.PropertyB);
        }

        [Fact]
        public void LocalizeItem_LocalizationDepthIsOneLevel_OnlyImmediateChildrenLocalized()
        {
            var localizedContent = GetLocalizedContent();

            var testClass = new TestClassA
            {
                ChildA = new TestClassA
                {
                    PropertyA = localizedContent,
                    ChildA = new TestClassA
                    {
                        PropertyA = localizedContent
                    }
                }
            };

            var localized = testClass.Localize(DEFAULT_LANGUAGE, LocalizationDepth.OneLevel);

            Assert.Equal(ANY_STRING_1, localized.ChildA.PropertyA);
            Assert.NotEqual(ANY_STRING_1, localized.ChildA.ChildA.PropertyA);
        }

        [Fact]
        public void LocalizeItem_HasCollectionOfLocalizableChildren_LocalizesCollection()
        {
            var localizedContent = GetLocalizedContent();

            var testClass = new TestClassA
            {
                ChildrenA = new[]
                {
                    new TestClassA
                    {
                        PropertyA = localizedContent
                    },
                    new TestClassA
                    {
                        PropertyA = localizedContent
                    }
                },
                ChildrenB = new[]
                {
                    new TestClassB
                    {
                        PropertyA = localizedContent
                    }
                }
            };

            var localized = testClass.Localize(DEFAULT_LANGUAGE, LocalizationDepth.Deep);

            Assert.Equal(ANY_STRING_1, localized.ChildrenA.ElementAt(0).PropertyA);
            Assert.Equal(ANY_STRING_1, localized.ChildrenA.ElementAt(1).PropertyA);
            Assert.Equal(ANY_STRING_1, localized.ChildrenB.ElementAt(0).PropertyA);
        }

        [Fact]
        public void LocalizeItem_PropertyHasDifferentCasesAndLowerCaseIsRequested_LocalizesAllProperties()
        {
            const string firstLanguageCode = "EN";
            const string secondLanguageCode = "En";
            const string thirdLanguageCode = "eN";
            const string fourthLanguageCode = "en";

            var firstContentsArray = new[]
            {
                new LocalizedContent(firstLanguageCode, "this is english content"),
                new LocalizedContent(secondLanguageCode, "this is another content")
            };

            var secondContentsArray = new[]
            {
                new LocalizedContent(thirdLanguageCode, "this is english content"),
                new LocalizedContent(fourthLanguageCode, "this is another content")
            };

            var firstObj = new TestClassB
            {
                PropertyA = firstContentsArray.First().Serialize(),
                PropertyB = firstContentsArray.Last().Serialize()
            };

            var secondObj = new TestClassB
            {
                PropertyA = secondContentsArray.First().Serialize(),
                PropertyB = secondContentsArray.Last().Serialize()
            };

            const string localizationRequestLanguage = "en";
            firstObj.Localize(localizationRequestLanguage);
            secondObj.Localize(localizationRequestLanguage);

            Assert.Equal("this is english content", firstObj.PropertyA);
            Assert.Equal("this is another content", firstObj.PropertyB);
            Assert.Equal("this is english content", secondObj.PropertyA);
            Assert.Equal("this is another content", secondObj.PropertyB);
        }

        [Fact]
        public void LocalizeItem_PropertyHasDifferentCasesAndUpperCaseIsRequested_LocalizesAllProperties()
        {
            const string firstLanguageCode = "EN";
            const string secondLanguageCode = "En";
            const string thirdLanguageCode = "eN";
            const string fourthLanguageCode = "en";

            var contentsOne = new[]
            {
                new LocalizedContent(firstLanguageCode, "this is english content"),
                new LocalizedContent(secondLanguageCode, "this is another content")
            };

            var contentsTwo = new[]
            {
                new LocalizedContent(thirdLanguageCode, "this is english content"),
                new LocalizedContent(fourthLanguageCode, "this is another content")
            };

            var firstObj = new TestClassB
            {
                PropertyA = contentsOne.First().Serialize(),
                PropertyB = contentsOne.Last().Serialize()
            };

            var secondObj = new TestClassB
            {
                PropertyA = contentsTwo.First().Serialize(),
                PropertyB = contentsTwo.Last().Serialize()
            };

            const string localizationRequestLanguage = "EN";
            firstObj.Localize(localizationRequestLanguage);
            secondObj.Localize(localizationRequestLanguage);

            Assert.Equal("this is english content", firstObj.PropertyA);
            Assert.Equal("this is another content", firstObj.PropertyB);
            Assert.Equal("this is english content", secondObj.PropertyA);
            Assert.Equal("this is another content", secondObj.PropertyB);
        }

        [Fact]
        public void LocalizeCollection_MultiplePropertiesNoParamsSpecified_LocalizesCorrectly()
        {
            var localizedField = GetLocalizedContent();

            var testClasses = new[]
            {
                new TestClassA {PropertyA = localizedField, PropertyB = localizedField},
                new TestClassA {PropertyA = localizedField, PropertyB = localizedField}
            };

            var localized = testClasses.Localize<TestClassA>(DEFAULT_LANGUAGE).ToList();

            Assert.Equal(ANY_STRING_1, localized.ElementAt(0).PropertyA);
            Assert.Equal(ANY_STRING_1, localized.ElementAt(0).PropertyB);
            Assert.Equal(ANY_STRING_1, localized.ElementAt(1).PropertyA);
            Assert.Equal(ANY_STRING_1, localized.ElementAt(1).PropertyB);
        }

        [Fact]
        public void LocalizeCollection_SinglePropertyWithDefaultLanguage_LocalizesCorrectly()
        {
            var localizedField = GetLocalizedContent();

            var testClasses = new[]
            {
                new TestClassA {PropertyA = localizedField}
            };

            var localized = testClasses.Localize(DEFAULT_LANGUAGE, i => i.PropertyA);

            Assert.Equal(ANY_STRING_1, localized.Single().PropertyA);
        }

        [Fact]
        public void LocalizeCollection_SinglePropertyWithTwoLanguages_DefaultLanguageLocalizesCorrectly()
        {
            var localizedContent = GetLocalizedContent();

            var testClasses = new[]
            {
                new TestClassA {PropertyA = localizedContent}
            };

            var localized = testClasses.Localize(DEFAULT_LANGUAGE, i => i.PropertyA);

            Assert.Equal(ANY_STRING_1, localized.Single().PropertyA);
        }

        [Fact]
        public void LocalizeCollection_SinglePropertyWithTwoLanguages_NonDefaultLanguageLocalizesCorrectly()
        {
            var localizedContent = GetLocalizedContent();

            var testClasses = new[]
            {
                new TestClassA {PropertyA = localizedContent}
            };

            var localized = testClasses.Localize(OTHER_LANGUAGE, i => i.PropertyA);

            Assert.Equal(ANY_STRING_2, localized.Single().PropertyA);
        }

        [Fact]
        public void LocalizeCollection_MultiplePropertiesWithTwoLanguages_NonDefaultLanguageLocalizesCorrectly()
        {
            var localizedContent = GetLocalizedContent();

            var testClasses = new[]
            {
                new TestClassA
                {
                    PropertyA = localizedContent,
                    PropertyB = localizedContent
                }
            };

            var localized = testClasses.Localize(OTHER_LANGUAGE, i => i.PropertyA, i => i.PropertyB).ToList();

            Assert.Equal(ANY_STRING_2, localized.Single().PropertyA);
            Assert.Equal(ANY_STRING_2, localized.Single().PropertyB);
        }

        [Fact]
        public void LocalizeCollection_SinglePropertyRequestedLanguageNotFound_LocalizesToDefaultLanguage()
        {
            var localizedFields = new[]
            {
                new LocalizedContent(DEFAULT_LANGUAGE, ANY_STRING_1)
            };

            var testClasses = new[]
            {
                new TestClassA {PropertyA = localizedFields.Serialize()}
            };

            var localized = testClasses.Localize(OTHER_LANGUAGE, i => i.PropertyA);

            Assert.Equal(ANY_STRING_1, localized.Single().PropertyA);
        }

        [Fact]
        public void LocalizeCollection_SinglePropertyRequestedLanguageNotFoundAndNoDefaultLanguage_LocalizesToFirst()
        {
            var localizedFields = new[]
            {
                new LocalizedContent(OTHER_LANGUAGE, ANY_STRING_2)
            };

            var testClasses = new[]
            {
                new TestClassA {PropertyA = localizedFields.Serialize()}
            };

            const string someOtherLanguage = OTHER_LANGUAGE + "yy";
            var localized = testClasses.Localize(someOtherLanguage, i => i.PropertyA);

            Assert.Equal(ANY_STRING_2, localized.Single().PropertyA);
        }

        [Fact]
        public void LocalizeCollection_NonserializedProperty_ReturnsOriginalValueWithoutThrowingException()
        {
            const string notSerializedJson = "1";

            var testClasses = new[]
            {
                new TestClassA {PropertyA = notSerializedJson}
            };

            var localized = testClasses.Localize(DEFAULT_LANGUAGE, i => i.PropertyA);

            Assert.Equal(notSerializedJson, localized.Single().PropertyA);
        }

        [Fact]
        public void LocalizeCollection_ObjectsHaveDifferentLanguageCodeCases_LocalizesCorrectly()
        {
            const string firstLanguageCode = "eN";
            const string secondLanguageCode = "En";

            var contents = new[]
            {
                new LocalizedContent(firstLanguageCode, "this is english content"),
                new LocalizedContent(secondLanguageCode, "this is another content")
            };

            var testClasses = new[]
            {
                new TestClassA {PropertyA = contents.First().Serialize(), PropertyB = contents.First().Serialize()},
                new TestClassA {PropertyA = contents.Last().Serialize(), PropertyB = contents.Last().Serialize()}
            };

            var localized = testClasses.Localize<TestClassA>(firstLanguageCode.ToUpper()).ToList();

            Assert.Equal("this is english content", localized.ElementAt(0).PropertyA);
            Assert.Equal("this is english content", localized.ElementAt(0).PropertyB);
            Assert.Equal("this is another content", localized.ElementAt(1).PropertyA);
            Assert.Equal("this is another content", localized.ElementAt(1).PropertyB);
        }

        [Fact]
        public void Set_ProvideLocalizedContentArray_SetsProperty()
        {
            var testClass = new TestClassA();

            var localizedContent = new[]
            {
                new LocalizedContent(DEFAULT_LANGUAGE, ANY_STRING_1),
                new LocalizedContent(OTHER_LANGUAGE, ANY_STRING_2)
            };

            testClass.Set(i => i.PropertyA, localizedContent);

            var localized = testClass.Localize(DEFAULT_LANGUAGE);

            Assert.Equal(ANY_STRING_1, localized.PropertyA);
        }

        [Fact]
        public void Set_ProvideLocalizedContentDictionary_SetsProperty()
        {
            var testClass = new TestClassA();

            var localizedContent = new Dictionary<string, string>
            {
                {DEFAULT_LANGUAGE, ANY_STRING_1},
                {OTHER_LANGUAGE, ANY_STRING_2}
            };

            testClass.Set(i => i.PropertyA, localizedContent);

            var localized = testClass.Localize(DEFAULT_LANGUAGE);

            Assert.Equal(ANY_STRING_1, localized.PropertyA);
        }
    }

    // ReSharper restore InconsistentNaming
    // ReSharper restore PossibleNullReferenceException
}
