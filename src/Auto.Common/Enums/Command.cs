namespace Auto.Common.Enums;

public enum Command : ushort
{
    Error = 000,
    Success = 001,

    InitiateSecureConnection = 010,
    FinalizeSecureConnection = 020,

    AddCustomer = 100,
    UpdateCustomer = 101,
    RemoveCustomer = 102,
    SearchCustomer = 103,
    GetIdByCustomer = 104,

    AddVehicle = 201,
    UpdateVehicle = 202,
    RemoveVehicle = 203,

    RegisterAccount = 301,
    LoginAccount = 302,
    UpdatePassword = 303,
    DeleteAccount = 304,
}
