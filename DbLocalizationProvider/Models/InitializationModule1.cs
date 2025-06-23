//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DbLocalizationProvider;

//namespace DbLocalizationProvider.Models
//{
//    [InitializableModule]
//    public class InitializationModule1 : IInitializableModule
//    {
//        public void Initialize(InitializationEngine context)
//        {
//            ConfigurationContext.Setup(ctx => ... );
//        }

//        public void Uninitialize(InitializationEngine context) { }
//    }
//}

//public class MyViewModel
//{
//    [LocalizedDisplayName("/path/to/langauge/resource")]
//    public string Username { get; set; }
//}

//public class MyViewModel
//{
//    [Display(Name = "/path/to/language/resource")]
//    public string Username { get; set; }
//}

//[ModuleDependency(typeof(InitializationModule))]
//public class LocalizationInitialization : IInitializableModule
//{
//    public void Initialize(InitializationEngine context)
//    {
//        ConfigurationContext.Setup(ctx =>
//        {
//            ctx.EnableLocalization = CheckLocalization;
//        });
//    }

//    public void Uninitialize(InitializationEngine context) { }

//    private bool CheckLocalization()
//    {
//        return !FeatureContext.IsEnabled<DisableLocalization>();
//    }
//}

//[QueryString(Key = "localization-disabled")]
//public class DisableLocalization : BaseFeature { }

//[LocalizedResource(KeyPrefix = "/this/is/prefix/")]
//public enum ThisIsMyStatus
//{
//    [ResourceKey("nothing")]
//    None,

//    [ResourceKey("something")]
//    Some,

//    [ResourceKey("anything")]
//    Any
//}


//namespace My.Project.Namespace
//{

//    [VisitorGroupCriterion(..,
//        LanguagePath = "/visitorgroupscriterias/usernamecriterion")]
//    public class UsernameCriterion : CriterionBase<UsernameCriterionModel>
//    {
//    ...
//}

//    public class UsernameCriterionModel : CriteriaPackModelBase
//    {
//        [Required]
//        [DojoWidget(SelectionFactoryType = typeof(EnumSelectionFactory))]
//        public UsernameValueCondition Condition { get; set; }

//        public string Value { get; set; }
//    }

//    public enum UsernameValueCondition
//    {
//        Matches,
//        StartsWith,
//        EndsWith,
//        Contains
//    }

//    /enums/my/project/namespace/usernamecriterion/usernamevaluecondition


//namespace My.Project.Namespace
//    {
//        [LocalizedResource(KeyPrefix = "/enums/my/project/namespace/usernamecriterion/usernamevaluecondition")]
//        public enum UsernameValueCondition
//        {
//            [ResourceKey("matches")]
//            Matches,
//            [ResourceKey("startswith")]
//            StartsWith,
//            [ResourceKey("endswith")]
//            EndsWith,
//            [ResourceKey("contains")]
//            Contains
//        }
//    }

//    namespace My.Project.Namespace
//    {
//        [LocalizedResource]
//        public class MyResources
//        {
//            public static PageHeader = "This is page header";
//    }
//    }

//    namespace My.Project.Namespace
//    {
//        [LocalizedResource]
//        public class MyResources
//        {
//            public static PageHeader => "This is page header";
//    }
//    }

//    [InitializableModule]
//    [ModuleDependency(typeof(InitializationModule))]
//    public class InitLocalization : IInitializableModule
//    {
//        public void Initialize(InitializationEngine context)
//        {
//            ConfigurationContext.Setup(cfg =>
//            {
//                cfg.ModelMetadataProviders.EnableLegacyMode = () => true;
//            });
//        }

//        public void Uninitialize(InitializationEngine context) { }
//    }

//    using DbLocalizationProvider.Abstractions.Refactoring;

//namespace DbLocalizationProvider.Tests.Refactoring
//    {
//        [LocalizedModel]
//        [RenamedResource("OldModelClass")]
//        public class RenamedModelClass
//        {
//            public string NewProperty { get; set; }
//        }
//    }

//    namespace DbLocalizationProvider.Tests.Refactoring
//    {
//        [LocalizedResource]
//        [RenamedResource("OldParentContainerClassAndNamespace", OldNamespace = "In.Galaxy.Far.Far.Away")]
//        public class RenamedParentContainerClassAndNamespace
//        {
//            [LocalizedResource]
//            [RenamedResource("OldNestedResourceClass")]
//            public class RenamedNestedResourceClass
//            {
//                public static string NewResourceKey => "New Resource Key";
//            }

//            [LocalizedResource]
//            [RenamedResource("OldNestedResourceClass")]
//            public class RenamedNestedResourceClassAndProperty
//            {
//                [RenamedResource("OldResourceKey")]
//                public static string NewResourceKey => "New Resource Key";
//            }
//        }
//    }


