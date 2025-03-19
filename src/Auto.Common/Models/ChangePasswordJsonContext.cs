using Auto.Common.Models;
using System.Text.Json.Serialization;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(ChangePasswordModel))]
internal partial class ChangePasswordJsonContext : JsonSerializerContext
{
}

