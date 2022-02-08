
using Robin.Data.ParkingETL.Actions;

namespace PampaSoft.Data.Etl.Api.Models.ActionConfigurations
{
    public interface IActionConfiguration
    {
        public Action ToFlowAction();
    }
}