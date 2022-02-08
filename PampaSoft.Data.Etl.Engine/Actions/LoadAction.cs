using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Robin.Data.ParkingETL.Destination;
using Robin.Data.ParkingETL.Format;

namespace Robin.Data.ParkingETL.Actions
{
    public class LoadAction : Action, IAction<bool>
    {
        private IDataDestination _destination;
        private DataTable _toLoad;
        private bool _result = false;
        
        public LoadAction(IDataDestination destination, DataTable toLoad)
        {
            this._destination = destination;
            this._toLoad = toLoad;
        }
        
        public LoadAction(IDataDestination destination)
        {
            this._destination = destination;
        }

        public void SetTable(DataTable toLoad)
        {
            this._toLoad = toLoad;
        }

        public async Task<bool> Execute()
        {
            if (!await this._destination.Open())
                return false;

            _result = await this._destination.Push(this._toLoad);
            return true;
        }

        public bool GetResult()
        {
            return _result;
        }

        public override bool IsNextCompatible(Type type)
        {
            return false;
        }
    }
}