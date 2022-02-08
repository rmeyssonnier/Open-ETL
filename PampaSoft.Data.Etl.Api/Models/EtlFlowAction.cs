namespace PampaSoft.Data.Etl.Api.Models
{
    public class EtlFlowAction
    {
        public string Type { get; set; }
        public int ActionId { get; set; }

        public override string ToString()
        {
            return $"[{ActionId}]{Type}";
        }
    }
}