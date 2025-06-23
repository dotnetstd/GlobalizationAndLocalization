using OrangeJetpack.Localization;

namespace LocalizerCustom.Models
{
    public class Planet : ILocalizable
    {
        [Localized]
        public string Name { get; set; }
    }
}
