using System;
using System.Collections.Generic;

namespace Ini
{
    public class IniSection : Dictionary<string, string>
    {
        public void Add(string line)
        {
            if (line.Length != 0)
            {
                int index = line.IndexOf('=');
                if (index != -1)
                    base.Add(line.Substring(0, index), line.Substring(index + 1, line.Length - index - 1));
                else
                    throw new Exception("Keys must have an equal sign.");
            }
        }

        public string ToString(string key)
        {
            return key + "=" + this[key];
        }

        public string[] GetKeys()
        {
            string[] output = new string[this.Count];
            byte i = 0;
            foreach (KeyValuePair<string, string> item in this)
            {
                output[i] = item.Key;
                i++;
            }
            return output;
        }
        public bool HasKey(string key)
        {
            foreach (KeyValuePair<string, string> item in this)
            {
                if (item.Key == key)
                    return true;
            }
            return false;
        }
    }
}
