using System.ComponentModel.DataAnnotations;

namespace WebsiteTechStore.Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }
        [Required, MinLength(4,ErrorMessage ="Yêu cầu nhập lại")]
        public string Name { get; set; }
       
        [Required, MinLength(4, ErrorMessage = "Yêu cầu nhập lại mô tả")]
        public string Description { get; set; }
        [Required]
        public string Slug { get; set; }

        public int Status { get; set; }
    }
}
