using System;
using System.Collections.Generic;
using System.Linq;
using Robin.Data.ParkingETL.Actions;
using Action = Robin.Data.ParkingETL.Actions.Action;

namespace PampaSoft.Data.Etl.Api.Models.ActionConfigurations
{
    public class TransformKeepColumnsConfiguration : IActionConfiguration
    {
        public ICollection<string> ToKeep { get; set; }

        public Action ToFlowAction()
        {
            return new ReshapeAction(ToKeep.ToArray(), false);
        }
    }
}