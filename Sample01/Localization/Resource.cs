namespace MainApp.Models.Localization
{
    public class Resource
    {
        public Resource(string name, string key, string value)
        {
            Name = name;
            Key = key;
            Value = value;
        }
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public virtual Culture Culture { get; set; }
    }
}
