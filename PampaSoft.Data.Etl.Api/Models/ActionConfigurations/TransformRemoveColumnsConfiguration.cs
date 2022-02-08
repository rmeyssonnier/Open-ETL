using System;
using System.Collections.Generic;
using System.Linq;
using Robin.Data.ParkingETL.Actions;
using Action = Robin.Data.ParkingETL.Actions.Action;

namespace PampaSoft.Data.Etl.Api.Models.ActionConfigurations
{
    public class TransformRemoveColumnsConfiguration : IActionConfiguration
    {
        public ICollection<string> ToRemove { get; set; }

        public Action ToFlowAction()
        {
            return new ReshapeAction(ToRemove.ToArray(), true);
        }
    }
}