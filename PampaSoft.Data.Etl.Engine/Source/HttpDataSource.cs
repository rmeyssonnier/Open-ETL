using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Robin.Data.ParkingETL.Format;
using Robin.Data.ParkingETL.Source;

namespace Robin.Data.ParkingETL
{
    public class HttpDataSource : BaseDataSource, IDataSource
    {
        private WebClient _webClient = new WebClient();
        private Stream _readStream = null;
        
        public HttpDataSource(string connectionString) : base(connectionString)
        {
        }
        
        public async Task<bool> Open()
        {
            try
            {
                _readStream = await this._webClient.OpenReadTaskAsync(new Uri(_connectionString));
                return _readStream?.CanRead ?? false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void Close()
        {

            if (_readStream != null)
            {
                _readStream.Close();
            }
            _webClient = new WebClient();
        }

        public ICollection<DataTable> ReadAll(IDataFormat dataFormat)
        {
            dataFormat.Parse(this._readStream);

            return new List<DataTable>() { dataFormat.GetTable() };
        }
    }
}