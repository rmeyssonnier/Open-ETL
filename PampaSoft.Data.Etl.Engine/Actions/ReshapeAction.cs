using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Robin.Data.ParkingETL.Format;

namespace Robin.Data.ParkingETL.Actions
{
    public class ReshapeAction : Action, IAction<DataTable>
    {
        private DataTable _table;
        private ICollection<string> _toModify;
        private bool _remove;

        public ReshapeAction(DataTable table, ICollection<string> toModify, bool remove)
        {
            this._table = table;
            this._toModify = toModify;
            this._remove = remove;
        }
        
        public ReshapeAction(ICollection<string> toModify, bool remove)
        {
            this._toModify = toModify;
            this._remove = remove;
        }

        public void SetTable(DataTable table)
        {
            this._table = table;
        }
        
        public Task<bool> Execute()
        {
            if (this._remove)
                this._table.ReshapeRemove(this._toModify.ToArray());
            else
            {
                this._table.ReshapeKeep(this._toModify.ToArray());
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