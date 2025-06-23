namespace MainApp.Models.Localization
{
    public class Resource
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string CultureInfo { get; set; }
        public virtual Culture Culture { get; set; }
    }
}
