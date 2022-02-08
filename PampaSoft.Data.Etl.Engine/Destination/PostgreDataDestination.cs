using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Robin.Data.ParkingETL.Source;
using DataTable = Robin.Data.ParkingETL.Format.DataTable;

namespace Robin.Data.ParkingETL.Destination
{
    public class PostgreDataDestination : BaseDataSource, IDataDestination, IDisposable
    {
        public PostgreDataDestination(string connectionString) : base(connectionString)
        {
        }
        
        private NpgsqlConnection _npgsqlConnection = null;
        public async Task<bool> Open()
        {
            try
            {
                _npgsqlConnection = new NpgsqlConnection(_connectionString);
                await _npgsqlConnection.OpenAsync();
                return !(_npgsqlConnection.State == ConnectionState.Closed || _npgsqlConnection.State == ConnectionState.Broken);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void Close()
        {
            _npgsqlConnection?.Dispose();
        }

        public async Task<bool> Push(DataTable dataTable)
        {
            string uuid = "_" + Guid.NewGuid().ToString().Replace("-", "_");

            ICollection<string> columns = new List<string>();
            columns.Add("id SERIAL PRIMARY KEY");
            
            foreach (var column in dataTable.Columns)
            {
                var valueToTest = dataTable.Column(column).First();
                columns.Add($"{column.Replace(" ", "_")} {this.VarTypeToDbType(valueToTest.GetType())}");
            }
            
            string createTableScript = $"CREATE TABLE {uuid} ({columns.Aggregate((c, n) => c + "," + n)})";
            
            await using (var cmd = new NpgsqlCommand(createTableScript, this._npgsqlConnection))
            {
                await cmd.ExecuteNonQueryAsync();
            }

            foreach (var row in dataTable.Iterrows())
            {
                string header = row.Select(r => r.Key).Aggregate((c, n) => c + "," + n).Replace(" ", "_");
                string values = "'" + row
                    .Select(r => r.Value?.ToString()?.Replace("'", "''"))
                    .Aggregate((c, n) =>  c + "','" + n).ToString() + "'";
                string command = $"INSERT INTO {uuid} ({header}) VALUES ({values})";
                try
                {
                    await using (var cmd = new NpgsqlCommand(command, this._npgsqlConnection))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return true;
        }

        private string VarTypeToDbType(Type varType)
        {
            if (varType == typeof(int))
            {
                return "INT";
            }
            
            if (varType == typeof(string))
            {
                return "VARCHAR(255)";
            }
            
            if (varType == typeof(DateTime))
            {
                return "TIMESTAMP";
            }

            throw new InvalidDataException($"Invalid type cast for db type : {varType}");
        }

        public void Dispose()
        {
            _npgsqlConnection?.Dispose();
        }
    }
}