namespace Robin.Data.ParkingETL.Format
{
    public class DataFormatBase
    {
        protected DataTable _dataTable = new DataTable();
        public DataTable GetTable()
        {
            return this._dataTable;
        }
    }
}