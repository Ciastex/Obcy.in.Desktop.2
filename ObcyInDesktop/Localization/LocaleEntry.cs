namespace ObcyInDesktop.Localization
{
    internal class LocaleEntry
    {
        public string Key { get; private set; }
        public string Value { get; private set; }

        public LocaleEntry(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public void SetValue(string value)
        {
            Value = value;
        }
    }
}
