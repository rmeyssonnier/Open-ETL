using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Robin.Data.ParkingETL.Format
{
    public class XmlDataFormat : DataFormatBase, IDataFormat
    {
        public void Parse(Stream stream)
        {
            using (StreamReader streamReader = new StreamReader(stream))
            {
                XDocument data = XDocument.Parse(streamReader.ReadToEnd());

                if (data.Elements().FirstOrDefault() != null)
                {
                    ICollection<string> header = new List<string>();
                    foreach (var element in data.Elements().First().Elements())
                    {
                        header.Add(element.Name.ToString());
                    }

                    this._dataTable.SetHeader(header.ToArray());
                }
                
                foreach (var node in data.Elements())
                {
                    ICollection<KeyValuePair<string, object>> row = new List<KeyValuePair<string, object>>();
                    foreach (var element in data.Elements().First().Elements())
                    {
                        row.Add(new KeyValuePair<string, object>(element.Name.ToString(), element.Value));
                    }

                    this._dataTable.InsertLine(row.ToArray());
                }
            }
        }
    }
}