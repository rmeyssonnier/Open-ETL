using System;
using System.Collections.Generic;
using System.Linq;
using Robin.Data.ParkingETL.Actions;
using Action = Robin.Data.ParkingETL.Actions.Action;

namespace PampaSoft.Data.Etl.Api.Models.ActionConfigurations
{
    public class RenameModel
    {
        public string OldName { get; set; }
        public string NewName { get; set; }
    }
    public class TransformRenameConfiguration : IActionConfiguration
    {
        public ICollection<RenameModel> NewNames { get; set; }

        public Action ToFlowAction()
        {
            ICollection<Tuple<string, string>> toRenameAsTuple = NewNames
                .Select(l => new Tuple<string, string>(l.OldName, l.NewName))
                .ToList();
            return new RenameAction(toRenameAsTuple);
        }
    }
}