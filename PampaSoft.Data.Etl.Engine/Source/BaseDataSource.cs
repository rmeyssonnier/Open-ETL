namespace Robin.Data.ParkingETL.Source
{
    public class BaseDataSource
    {
        protected string _connectionString;
        protected BaseDataSource(string connectionString)
        {
            this._connectionString = connectionString;
        }
    }
}