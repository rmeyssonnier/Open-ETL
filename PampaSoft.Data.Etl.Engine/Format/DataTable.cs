using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using RestSharp.Serialization.Json;

namespace Robin.Data.ParkingETL.Format
{
    public class DataTable
    {
        private IDictionary<string, List<object>> _dictionary = new Dictionary<string, List<object>>();
        public string Name { get; }

        public DataTable()
        {
            this.Name = Guid.NewGuid().ToString();
        }
        
        public DataTable(string name)
        {
            this.Name = name;
        }
        
        public int Count
        {
            get
            {
                if (_dictionary.Keys.Count == 0)
                    return 0;

                return _dictionary[_dictionary.First().Key].Count;
            }
        }

        public string[] Columns => this._dictionary.Keys.ToArray();
        

        public void SetHeader(string[] columns)
        {
            _dictionary = new Dictionary<string, List<object>>();
            foreach (var col in columns)
            {
                _dictionary[col] = new List<object>();
            }
        }

        public bool InsertLine(KeyValuePair<string, object>[] values)
        {
            if (values.Length != _dictionary.Keys.Count)
                return false;

            foreach (var value in values)
            {
                _dictionary[value.Key].Add(value.Value);
            }

            return true;
        }
        
        public bool InsertLine(object[] values)
        {
            if (values.Length != _dictionary.Keys.Count)
                return false;

            int i = 0;
            foreach (var key in _dictionary.Keys)
            {
                _dictionary[key].Add(this.ConvertValue(values[i]));
                i++;
            }

            return true;
        }

        private dynamic ConvertValue(object value)
        {
            int resultInt = 0;
            DateTime dateTimeResult = default;

            if (DateTime.TryParse(value.ToString(), out dateTimeResult))
                return dateTimeResult;

            if (int.TryParse(value.ToString(), out resultInt))
                return resultInt;

            return value.ToString();
        }

        public ICollection<object> Column(string key)
        {
            if (this._dictionary.ContainsKey(key))
                return this._dictionary[key];
            else
            {
                return null;
            }
        }

        public ICollection<KeyValuePair<string, object>[]> Iterrows()
        {
            ICollection<KeyValuePair<string, object>[]> result = new List<KeyValuePair<string, object>[]>();
            
            for (int i = 0; i < this.Count; i++)
            {
                KeyValuePair<string, object>[] line = new KeyValuePair<string, object>[_dictionary.Keys.Count];

                int c = 0;
                foreach (var key in _dictionary.Keys)
                {
                    line[c] = new KeyValuePair<string, object>(key, _dictionary[key][i]);
                    c++;
                }
                
                result.Add(line);
                i++;
            }

            return result;
        }

        public void ReshapeKeep(string[] toKeep)
        {
            foreach (var key in _dictionary.Keys)
            {
                if (!toKeep.Contains(key))
                    _dictionary.Remove(key);
            }
        }
        
        public void ReshapeRemove(string[] toRemove)
        {
            foreach (var key in _dictionary.Keys)
            {
                if (toRemove.Contains(key))
                    _dictionary.Remove(key);
            }
        }
        
        public void RenameColumn(string oldName, string newName)
        {
            if (_dictionary.Keys.Contains(oldName) && !_dictionary.Keys.Contains(newName))
            {
                _dictionary[newName] = _dictionary[oldName];
                _dictionary.Remove(oldName);
            }
        }

        public void Append(DataTable toAppend)
        {
            DataTable tmp = toAppend;
            tmp.ReshapeKeep(_dictionary.Keys.ToArray());
            
            ICollection<KeyValuePair<string, object>> linePrototype = _dictionary.Keys
                .Select(k => new KeyValuePair<string, object>(k, null))
                .ToList();

            for (int i = 0; i < toAppend.Count; i++)
            {
                ICollection<KeyValuePair<string, object>> newLine = new List<KeyValuePair<string, object>>();
                foreach (var key in toAppend._dictionary.Keys)
                {
                    newLine.Add(new KeyValuePair<string, object>(key, toAppend._dictionary[key][i]));
                }

                foreach (var line in linePrototype)
                {
                    if (newLine.Count(l => l.Key == line.Key) <= 0)
                    {
                        newLine.Add(new KeyValuePair<string, object>(line.Key, null));
                    }
                }

                this.InsertLine(newLine.ToArray());
                i++;
            }
        }

        public void ToCsv(string path)
        {
            ICollection<string> toWrite = new List<string>();
            string header = this._dictionary.Keys.Aggregate((c, n) => c + ";" + n);
            toWrite.Add(header);

            for (int i = 0; i < this.Count; i++)
            {
                string line = "";
                foreach (var key in this._dictionary.Keys)
                {
                    //line += Newtonsoft.Json.JsonConvert.SerializeObject(this._dictionary[key][i]) + ";";
                    if (this._dictionary[key][i] == null)
                        line += "null;";
                    else
                        line += this._dictionary[key][i] + ";";
                }

                toWrite.Add(line.Remove(line.Length - 1, 1));
            }
            
            File.WriteAllText(Path.Combine(path, this.Name + ".csv"), toWrite.Aggregate((c, n) => c + "\n" + n));
        }
    }
}