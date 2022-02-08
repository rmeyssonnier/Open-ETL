using Robin.Data.ParkingETL.Actions;
using Robin.Data.ParkingETL.Destination;
using Robin.Data.ParkingETL.Source;

namespace PampaSoft.Data.Etl.Api.Models.ActionConfigurations
{
    public class LoadPostgresConfiguration : IActionConfiguration
    {
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }

        public Action ToFlowAction()
        {
            string connectionString = $"Host={Url};Username={Username};Password={Password};Database={Database}";
            IDataDestination dataDestination = new PostgreDataDestination(connectionString);

            return new LoadAction(dataDestination);
        }
    }
}