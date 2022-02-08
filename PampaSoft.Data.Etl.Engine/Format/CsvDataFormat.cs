using System.IO;

namespace Robin.Data.ParkingETL.Format
{
    public class CsvDataFormat : DataFormatBase, IDataFormat
    {
        private bool _haveHeader;
        private char _columnSeparator;
        private char _lineSeparator;

        public CsvDataFormat(bool haveHeader, char columnSeparator, char lineSeparator)
        {
            this._haveHeader = haveHeader;
            this._columnSeparator = columnSeparator;
            this._lineSeparator = lineSeparator;
        }

        public void Parse(Stream stream)
        {
            this._dataTable = new DataTable();
            
            using (StreamReader streamReader = new StreamReader(stream))
            {
                string[] lines = streamReader.ReadToEnd().Replace("\r", "").Split(this._lineSeparator);
                int i = 0;
                
                if (this._haveHeader)
                {
                    this._dataTable.SetHeader(lines[i].Split(this._columnSeparator));
                    i++;
                }
                
                for (; i < lines.Length; i++)
                {
                    this._dataTable.InsertLine(lines[i].Split(this._columnSeparator));
                }
            }
        }
    }
}