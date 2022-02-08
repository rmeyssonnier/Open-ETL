namespace Robin.Data.ParkingETL.Destination
{
    public class BaseDataDestination
    {
        protected string _connectionString;
        protected BaseDataDestination(string connectionString)
        {
            this._connectionString = connectionString;
        }
    }
}