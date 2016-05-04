using System;
using System.Collections.Generic;
using System.IO;

namespace ObcyInDesktop.Settings
{
    class SettingsManager : Dictionary<string, string>
    {
        public new string this[string key]
        {
            get
            {
                if (ContainsKey(key))
                {
                    return base[key];
                }
                return string.Empty;
            }
            set
            {
                if (ContainsKey(key))
                {
                    base[key] = value;
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        public void Save(string filePath)
        {
            if (App.CanWriteToCurrentFolder)
            {
                using (var sw = new StreamWriter(filePath, false))
                {
                    foreach (var kvp in this)
                    {
                        sw.WriteLine($"{kvp.Key}={kvp.Value}");
                    }
                }
            }
        }

        public T GetConfigurationValue<T>(string key)
        {
            try
            {
                return (T)Convert.ChangeType(this[key], typeof(T));
            }
            catch
            {
                return default(T);
            }
        }
    }
}
