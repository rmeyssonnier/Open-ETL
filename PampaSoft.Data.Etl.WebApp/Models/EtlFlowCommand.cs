using System.Collections.Generic;

namespace PampaSoft.Data.Etl.Api.Models
{
    public class EtlFlowCommand
    {
        public ICollection<string> Stages { get; set; }
    }
}