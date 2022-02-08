using System;

namespace Robin.Data.ParkingETL.Actions
{
    public abstract class Action
    {
        public abstract bool IsNextCompatible(Type type);
    }
}