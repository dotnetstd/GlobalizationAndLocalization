namespace MBODM.AspNetCore.SimpleLocalization
{
    public interface ILocalizer
    {
        string this[string key] { get; }

        string GetText(string key);
    }
}
