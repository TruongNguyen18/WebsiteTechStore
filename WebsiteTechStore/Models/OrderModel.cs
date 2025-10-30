using System.ComponentModel.DataAnnotations;

namespace WebsiteTechStore.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public string UserName { get; set; }
        public DateTime CreateDate { get; set; }
        public int Status { get; set; }

        // Địa chỉ giao hàng
        [Required] public string Province { get; set; } = string.Empty;   // Tỉnh/Thành phố
        [Required] public string District { get; set; } = string.Empty;   // Quận/Huyện
        [Required] public string Ward { get; set; } = string.Empty;       // Phường/Xã
        [Required] public string AddressLine { get; set; } = string.Empty; // Số nhà, đường

        // Thanh toán: 0 = COD (Thanh toán khi nhận hàng), có thể mở rộng sau
        public int PaymentMethod { get; set; } = 0;

        // Tổng tiền (nếu muốn lưu tổng)
        public decimal Total { get; set; }
    }
}
