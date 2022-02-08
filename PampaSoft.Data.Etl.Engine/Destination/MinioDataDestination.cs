using System;
using System.IO;
using System.Threading.Tasks;
using Minio;
using Robin.Data.ParkingETL.Format;

namespace Robin.Data.ParkingETL.Destination
{
    public class MinioDataDestination : BaseDataDestination, IDataDestination
    {
        private string _endpoint = "";
        private string _accessKey = "";
        private string _secretKey = "";
        private string _bucket = "";
        private MinioClient _minioClient = null;
        
        public MinioDataDestination(string connectionString) : base(connectionString)
        {
        }
        
        public async Task<bool> Open()
        {
            string[] parts = _connectionString.Split(";");
            
            _endpoint = parts[0];
            _accessKey = parts[1];
            _secretKey = parts[2];
            _bucket = parts[3];
            
            try
            {
                _minioClient = new MinioClient(_endpoint, _accessKey, _secretKey);
                bool bucketExist = await _minioClient.BucketExistsAsync(_bucket);
                return bucketExist;
            }
            catch (Exception e)
            {
                _minioClient = null;
                return false;
            }
        }

        public void Close()
        {
            _endpoint = null;
            _accessKey = null;
            _secretKey = null;
            _bucket = null;
            this._minioClient = null;
        }

        public async Task<bool> Push(DataTable dataTable)
        {
            dataTable.ToCsv(Path.GetTempPath());
            
            if (_minioClient == null)
                return false;

            try
            {
                await _minioClient.PutObjectAsync(this._bucket, "input/" + dataTable.Name + ".csv", Path.Combine(Path.GetTempPath(), dataTable.Name + ".csv"), "text/csv");
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}