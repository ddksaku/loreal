using System;
namespace LorealOptimiseData.Enums
{
    [Flags]
    public enum RoleEnum
    {
        NoRole = 0,
        ClientCare = 2,
        DivisionAdmin = 4,
        Finance = 8,
        Logistics = 16,
        Marketing = 32,
        NAMs = 64,
        ReadOnly = 128,
        SystemAdmin = 256
    }
}
