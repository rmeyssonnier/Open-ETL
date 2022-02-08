using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Robin.Data.ParkingETL.Format;

namespace Robin.Data.ParkingETL
{
    public class JsonDataFormat : DataFormatBase, IDataFormat
    {
        
        public void Parse(Stream stream)
        {
            using (StreamReader streamReader = new StreamReader(stream))
            {
                var o = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(streamReader.ReadToEnd());

                if (o.Count > 0)
                {
                    int i = 0;
                    var entryFirst = o.First();
                    string[] header = new string[entryFirst.Value.Properties().Count() + 1];
                    header[i++] = "key";
                    foreach (var property in entryFirst.Value.Properties())
                    {
                        header[i++] = property.Name;
                    }
                    this._dataTable.SetHeader(header);
                }
                
                foreach (var line in o)
                {
                    var cols = line.Value.Properties()
                        .Select(p => new KeyValuePair<string, object>(p.Name, p.Value.ToObject<object>()))
                        .ToList();
                    cols.Add(new KeyValuePair<string, object>("key", line.Key));
                    this._dataTable.InsertLine(cols.ToArray());
                }
            }
        }
    }
}