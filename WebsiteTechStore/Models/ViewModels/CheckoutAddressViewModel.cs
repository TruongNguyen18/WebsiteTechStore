using System.ComponentModel.DataAnnotations;
using WebsiteTechStore.Models;

namespace WebsiteTechStore.Models.ViewModels
{
    public class CheckoutAddressViewModel
    {
        [Required(ErrorMessage = "Chọn Tỉnh/Thành phố")]
        public string Province { get; set; } = "";

        [Required(ErrorMessage = "Chọn Quận/Huyện")]
        public string District { get; set; } = "";

        [Required(ErrorMessage = "Chọn Phường/Xã")]
        public string Ward { get; set; } = "";

        [Required(ErrorMessage = "Nhập địa chỉ cụ thể")]
        public string AddressLine { get; set; } = "";

        // Chỉ COD
        [Required]
        public int PaymentMethod { get; set; } = 0; // 0 = COD
    }
}