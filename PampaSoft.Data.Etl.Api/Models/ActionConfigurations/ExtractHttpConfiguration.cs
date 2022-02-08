using Robin.Data.ParkingETL;
using Robin.Data.ParkingETL.Actions;
using Robin.Data.ParkingETL.Format;
using Robin.Data.ParkingETL.Source;

#nullable enable
namespace PampaSoft.Data.Etl.Api.Models.ActionConfigurations
{
    public class ExtractHttpConfiguration : IActionConfiguration
    {
        public string Url { get; set; }
        public DataFormatEnum Format { get; set; }
        public CsvFormat? CsvFormat { get; set; }

        public Action ToFlowAction()
        {
            IDataSource source = new HttpDataSource(Url);
            IDataFormat? format = null;

            switch (Format)
            {
                case DataFormatEnum.Json:
                    format = new JsonDataFormat();
                    break;
                case DataFormatEnum.Xml:
                    format = new XmlDataFormat();
                    break;
                case DataFormatEnum.Csv:
                    if (CsvFormat.LineDelimiter.Contains("n"))
                        format = new CsvDataFormat(true, CsvFormat.ColDelimiter[0], '\n');
                    else
                    {
                        format = new CsvDataFormat(true, CsvFormat.ColDelimiter[0], CsvFormat.LineDelimiter[0]);
                    }
                    break;
            }
            
            return new ExtractAction(source, format);
        }
    }
}