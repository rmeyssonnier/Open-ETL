using System.Collections.Generic;
using System.Threading.Tasks;
using Robin.Data.ParkingETL.Format;

namespace Robin.Data.ParkingETL.Source
{
    public interface IDataSource
    {
        Task<bool> Open();
        void Close();
        ICollection<DataTable> ReadAll(IDataFormat dataFormat);
    }
}