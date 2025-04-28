using NalixGarage.Common.Dto;
using NalixGarage.Common.Entities.Customers;
using NalixGarage.Common.Entities.Vehicles;
using System.Text.Json.Serialization;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(Vehicle))]
[JsonSerializable(typeof(Customer))]
[JsonSerializable(typeof(SearchDto))]
[JsonSerializable(typeof(AccountDto))]
[JsonSerializable(typeof(PasswordChangeDto))]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design",
    "CA1050:Declare types in namespaces", Justification = "<Pending>")]
public partial class JsonContext : JsonSerializerContext
{
}