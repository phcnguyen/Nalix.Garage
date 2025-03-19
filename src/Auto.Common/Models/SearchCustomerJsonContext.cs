using Auto.Common.Models;
using System.Text.Json.Serialization;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(SearchCustomer))]
internal partial class SearchCustomerJsonContext : JsonSerializerContext
{
}
