using System;

namespace Nalix.Garage.Common.Attributes;

/// <summary>
/// Thuộc tính dùng để xác định mã màu HEX cho các trường.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class HexColorAttribute(string hexCode) : Attribute
{
    /// <summary>
    /// Lấy mã màu HEX.
    /// </summary>
    public string HexCode { get; } = hexCode;
}
