using System;
using System.Collections.Generic;
using System.IO;

namespace Ini
{
    public class IniFile : Dictionary<string, IniSection>
    {
        private string _FileName;
        public string FileName
        {
            get { return this._FileName; }
        }

        public IniFile(string file)
        {
            this._FileName = file;
        }

        public string Add(string line)
        {
            if (line.StartsWith("["))
            {
                line = line.TrimStart('[');
            }
            if (line.EndsWith("]"))
            {
                line = line.TrimEnd(']');
            }
            this.Add(line, new IniSection());

            return line;
        }

        public bool Load()
        {
            // Nothing to load, get out.
            if (!this.Exists())
            {
                return true;
            }

            try
            {
                StreamReader sr = new StreamReader(this._FileName);
                string section = "";
                while (sr.Peek() != -1)
                {
                    string read = sr.ReadLine();
                    if (read.StartsWith("[") && read.EndsWith("]"))
                    {
                        section = this.Add(read);
                    }
                    else
                    {
                        if (section.Length != 0)
                        {
                            this[section].Add(read);
                        }
                        else
                        {
                            throw new Exception("Ini file must start with a section.");
                        }
                    }
                }
                sr.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Save()
        {
            try
            {
                StreamWriter sw = new StreamWriter(this._FileName);
                foreach (string section in this.Keys)
                {
                    sw.WriteLine("[" + section + "]");
                    foreach (string key in this[section].Keys)
                    {
                        sw.WriteLine(key + "=" + this[section][key]);
                    }
                    sw.WriteLine();
                    sw.Flush();
                }
                sw.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Exists()
        {
            return File.Exists(this._FileName);
        }

        public string[] GetSections()
        {
            string[] output = new string[this.Count];
            byte i = 0;
            foreach (KeyValuePair<string, IniSection> item in this)
            {
                output[i] = item.Key;
                i++;
            }
            return output;
        }
        public bool HasSection(string section)
        {
            foreach (KeyValuePair<string, IniSection> item in this)
            {
                if (item.Key == section)
                    return true;
            }
            return false;
        }
    }
}
