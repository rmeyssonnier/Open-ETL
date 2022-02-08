using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Robin.Data.ParkingETL.Format;
using Robin.Data.ParkingETL.Source;

namespace Robin.Data.ParkingETL.Actions
{
    public class ExtractAction : Action, IAction<ICollection<DataTable>>
    {
        private ICollection<DataTable> result = new List<DataTable>();
        private IDataSource _source;
        private IDataFormat _format;
        
        public ExtractAction(IDataSource source, IDataFormat format)
        {
            _source = source;
            _format = format;
        }
        
        public async Task<bool> Execute()
        {
            if (!await this._source.Open())
                return false;

            this.result = this._source.ReadAll(this._format);
            return true;
        }

        public ICollection<DataTable> GetResult()
        {
            return this.result;
        }

        public override bool IsNextCompatible(Type type)
        {
            ICollection<Type> compatibles = new List<Type>()
            {
                typeof(RenameAction),
                typeof(ReshapeAction),
                typeof(LoadAction),
                null
            };

            return compatibles.Contains(type);
        }
    }
}