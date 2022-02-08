using Robin.Data.ParkingETL.Actions;
using Robin.Data.ParkingETL.Destination;
using Robin.Data.ParkingETL.Source;

namespace PampaSoft.Data.Etl.Api.Models.ActionConfigurations
{
    public class LoadMinioConfiguration : IActionConfiguration
    {
        public string Url { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string Bucket { get; set; }
        public string Path { get; set; }

        public Action ToFlowAction()
        {
            string connectionString = $"{Url};{AccessKey};{SecretKey};{Bucket}";
            if (!string.IsNullOrEmpty(Path))
                connectionString += $";{Path}";
            
            IDataDestination dataDestination = new MinioDataDestination(connectionString);

            return new LoadAction(dataDestination);
        }
    }
}