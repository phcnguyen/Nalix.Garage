using System.ComponentModel;

namespace Auto.Common.Models.Payments;

public enum TaxRateType
{
    [Description("Không áp dụng thuế VAT (0%)")]
    None = 0,

    [Description("Thuế VAT 5%")]
    VAT5 = 5,

    [Description("Thuế VAT 8% (hỗ trợ kinh tế)")]
    VAT8 = 8,

    [Description("Thuế VAT 10% (mặc định)")]
    VAT10 = 10
}