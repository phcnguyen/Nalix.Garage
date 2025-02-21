using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Entites.Payments;

public enum TaxRateType
{
    [Display(Name = "Không áp dụng thuế VAT (0%)")]
    None = 0,

    [Display(Name = "Thuế VAT 5%")]
    VAT5 = 5,

    [Display(Name = "Thuế VAT 8% (hỗ trợ kinh tế)")]
    VAT8 = 8,

    [Display(Name = "Thuế VAT 10% (mặc định)")]
    VAT10 = 10
}