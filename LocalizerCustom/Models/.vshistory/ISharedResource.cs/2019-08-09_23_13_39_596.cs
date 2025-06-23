namespace LocalizerCustom.Models
{
    public interface ISharedResource
    {
        string MessageOne { get; }
    }
    public class SharedResource : ISharedResource
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public SharedResource(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public string MessageOne => _localizer["MessageOne"];
    }
}
