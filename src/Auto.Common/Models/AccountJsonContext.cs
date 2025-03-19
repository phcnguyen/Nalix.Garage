using Auto.Common.Models;
using System.Text.Json.Serialization;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(AccountModel))]
internal partial class AccountJsonContext : JsonSerializerContext
{
}
