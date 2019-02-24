using System;

namespace WcfBusinessLogic.Core.Helpers.Parsers
{
    internal class SeatStateEnumParser
    {
        public static Contracts.Data.SeatState? ToSeatStateContract(BusinessLogic.Services.SeatState from)
        {
            if (Enum.IsDefined(typeof(Contracts.Data.SeatState), from.ToString()))
                return (Contracts.Data.SeatState)from;
            else
                return null;
        }

        public static BusinessLogic.Services.SeatState? ToSeatStateBLL(Contracts.Data.SeatState from)
        {
            if (Enum.IsDefined(typeof(BusinessLogic.Services.SeatState), from.ToString()))
                return (BusinessLogic.Services.SeatState)from;
            else
                return null;
        }
    }
}
