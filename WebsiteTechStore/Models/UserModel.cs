using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Antiforgery;

namespace WebsiteTechStore.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Làm ơn nhập tên đăng nhập")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Làm ơn nhập email"),EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password),Required(ErrorMessage = "Làm ơn nhập mật khẩu")]
        public string Password { get; set; }


    }
}
