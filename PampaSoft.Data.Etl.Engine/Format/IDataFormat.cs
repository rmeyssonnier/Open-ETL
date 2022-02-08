using System.IO;

namespace Robin.Data.ParkingETL.Format
{
    public interface IDataFormat
    {
        void Parse(Stream stream);
        DataTable GetTable();
    }
}