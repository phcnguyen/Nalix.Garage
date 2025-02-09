namespace Auto.Common.Models.Part
{
    /// <summary>
    /// Lớp đại diện cho phụ tùng thay thế.
    /// </summary>
    public class ReplacementPart
    {
        /// <summary>
        /// Tên phụ tùng.
        /// </summary>
        public string PartName { get; set; }

        /// <summary>
        /// Số lượng phụ tùng.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Đơn giá của phụ tùng.
        /// </summary>
        public decimal UnitPrice { get; set; }
    }
}