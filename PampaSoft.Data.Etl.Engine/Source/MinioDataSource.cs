using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Minio;
using Minio.DataModel;
using Robin.Data.ParkingETL.Format;

namespace Robin.Data.ParkingETL.Source
{
    public class MinioDataSource : BaseDataSource, IDataSource
    {
        private string _endpoint = "";
        private string _accessKey = "";
        private string _secretKey = "";
        private string _bucket = "";
        private string _path = "";
        
        private MinioClient _minioClient = null;
        
        public MinioDataSource(string connectionString) : base(connectionString)
        {
        }
        
        public async Task<bool> Open()
        {
            string[] parts = _connectionString.Split(";");
            
            _endpoint = parts[0];
            _accessKey = parts[1];
            _secretKey = parts[2];
            _bucket = parts[3];

            if (parts.Length > 4)
                _path = parts[4];
            
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
        
        public ICollection<DataTable> ReadAll(IDataFormat dataFormat)
        {
            ICollection<DataTable> res = new List<DataTable>();
            ICollection<Item> itemFounds = new List<Item>();
            
            if (this._minioClient == null)
                return res;

            Task load = new Task(() =>
            {
                bool loadedEnd = false;
                
                IObservable<Item> observable = this._minioClient.ListObjectsAsync(this._bucket, _path, true);
                IDisposable subscription = observable.Subscribe(
                    item => itemFounds.Add(item),
                    ex => Console.WriteLine("OnError: {0}", ex.Message),
                    async () =>
                    {
                        res = await this.LoadItemContent(itemFounds, dataFormat);
                        loadedEnd = true;
                    });

                while (!loadedEnd)
                {
                    Task.Delay(500);
                }
            });

            load.Start();
            load.Wait();

            return res;
        }

        private async Task<ICollection<DataTable>> LoadItemContent(ICollection<Item> items, IDataFormat dataFormat)
        {
            ICollection<DataTable> res = new List<DataTable>();
            
            foreach (var item in items)
            {
                if (!item.IsDir)
                    await this._minioClient.GetObjectAsync(this._bucket, item.Key, 
                        stream =>
                        {
                            dataFormat.Parse(stream);
                            res.Add(dataFormat.GetTable());
                        });
            }

            return res;
        }
    }
}