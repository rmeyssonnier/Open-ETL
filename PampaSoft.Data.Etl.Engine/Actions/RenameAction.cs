using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Robin.Data.ParkingETL.Format;

namespace Robin.Data.ParkingETL.Actions
{
    public class RenameAction : Action, IAction<DataTable>
    {
        private DataTable _table;
        private ICollection<Tuple<string, string>> _toRename;
        
        public RenameAction(DataTable table, ICollection<Tuple<string, string>> toRename)
        {
            this._table = table;
            this._toRename = toRename;
        }
        
        public RenameAction(ICollection<Tuple<string, string>> toRename)
        {
            this._toRename = toRename;
        }

        public void SetTable(DataTable table)
        {
            this._table = table;
        }
        
        public Task<bool> Execute()
        {
            foreach (var rename in _toRename)
            {
                this._table.RenameColumn(rename.Item1, rename.Item2);
            }

            return Task.FromResult(true);
        }

        public DataTable GetResult()
        {
            return this._table;
        }
        
        public override bool IsNextCompatible(Type type)
        {
            ICollection<Type> compatibles = new List<Type>()
            {
                typeof(RenameAction),
                typeof(ReshapeAction),
                typeof(LoadAction)
            };

            return compatibles.Contains(type);
        }
    }
}