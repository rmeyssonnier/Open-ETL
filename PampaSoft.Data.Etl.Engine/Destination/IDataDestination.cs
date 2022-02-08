using System.Threading.Tasks;
using Robin.Data.ParkingETL.Format;

namespace Robin.Data.ParkingETL.Destination
{
    public interface IDataDestination
    {
        Task<bool> Open();
        void Close();
        Task<bool> Push(DataTable dataTable);
    }
}