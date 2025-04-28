using Nalix.Garage.Common.Dto;
using Nalix.Garage.Common.Entities.Customers;
using Nalix.Garage.Common.Entities.Vehicles;
using System.Text.Json.Serialization;

namespace Nalix.Garage.Common;

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