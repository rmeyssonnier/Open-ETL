using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Robin.Data.ParkingETL.Format;

namespace Robin.Data.ParkingETL.Actions
{
    public class ActionFlow
    {
        private ICollection<Action> _actions = new List<Action>();
        private Type firstType = typeof(ExtractAction);
        
        public bool AddAction(Action toAdd)
        {
            if (_actions.Count == 0)
            {
                if (toAdd.GetType() != firstType)
                    return false;
            }
            else
            {
                if (!this._actions.Last().IsNextCompatible(toAdd.GetType()))
                {
                    return false;
                }
            }
            
            this._actions.Add(toAdd);
            return true;
        }

        public async Task Execute()
        {
            DataTable currentTable = null;
            bool currentResult = false;
            
            foreach (var action in _actions)
            {
                if (action.GetType() == typeof(ExtractAction))
                {
                    if (!await ((ExtractAction) action).Execute())
                        throw new Exception("Error during extract stage");
                    
                    currentTable = ((ExtractAction) action).GetResult().First();
                    currentResult = true;
                }
                
                else if (action.GetType() == typeof(RenameAction))
                {
                    if (currentTable == null)
                        throw new Exception("Current data table is null during rename stage");
                    
                    ((RenameAction) action).SetTable(currentTable);
                    
                    if (!await ((RenameAction) action).Execute())
                        throw new Exception("Error during extract stage");
                    
                    currentTable = ((RenameAction) action).GetResult();
                    currentResult = true;
                }
                
                else if (action.GetType() == typeof(ReshapeAction))
                {
                    if (currentTable == null)
                        throw new Exception("Current data table is null during reshape stage");
                    
                    ((ReshapeAction) action).SetTable(currentTable);

                    if (!await ((ReshapeAction) action).Execute())
                        throw new Exception("Error during reshape stage");
                    
                    currentTable = ((ReshapeAction) action).GetResult();
                    currentResult = true;
                }
                
                else if (action.GetType() == typeof(LoadAction))
                {
                    if (currentTable == null)
                        throw new Exception("Current data table is null during reshape stage");
                    
                    ((LoadAction) action).SetTable(currentTable);

                    if (!await ((LoadAction) action).Execute())
                        throw new Exception("Error during reshape stage");
                    
                    currentResult = ((LoadAction) action).GetResult();
                }

                else
                {
                    throw new InvalidOleVariantTypeException($"Invalid action type {action.GetType()}");
                }
            }
        }
    }
}