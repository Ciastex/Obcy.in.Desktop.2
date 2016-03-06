using System;
using System.Collections.Generic;
using System.Linq;

namespace ObcyInDesktop.Localization
{
    class LocaleManager : Dictionary<string, List<LocaleEntry>>
    {
        private string _currentLocaleIdentifier;

        public string CurrentLocaleIdentifier
        {
            get { return _currentLocaleIdentifier; }
            set
            {
                if (!ContainsKey(value))
                {
                    Add(value, new List<LocaleEntry>());
                }
                _currentLocaleIdentifier = value;
                LocaleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public new string this[string key]
        {
            get
            {
                if (Count == 0)
                {
                    return "NoTranslation";
                }
                if (base[CurrentLocaleIdentifier].Contains(base[CurrentLocaleIdentifier].FirstOrDefault(entry => entry.Key == key)))
                {
                    return base[CurrentLocaleIdentifier].First(entry => entry.Key == key).Value;
                }
                return "NoTranslation";
            }
            set
            {
                if (base[CurrentLocaleIdentifier].Contains(base[CurrentLocaleIdentifier].FirstOrDefault(entry => entry.Key == key)))
                {
                    base[CurrentLocaleIdentifier].First(entry => entry.Key == key).SetValue(value);
                }
                else
                {
                    base[CurrentLocaleIdentifier].Add(new LocaleEntry(key, value));
                }
            }
        }

        public event EventHandler LocaleChanged;

        public LocaleManager()
        {
            CurrentLocaleIdentifier = "English";
        }
    }
}
