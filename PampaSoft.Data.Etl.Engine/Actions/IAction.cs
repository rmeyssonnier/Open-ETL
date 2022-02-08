using System;
using System.Threading.Tasks;

namespace Robin.Data.ParkingETL.Actions
{
    public interface IAction<T>
    {
        public Task<bool> Execute();
        public T GetResult();
        public bool IsNextCompatible(Type type);
    }
}