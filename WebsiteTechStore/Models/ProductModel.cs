using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Migrations;
using WebsiteTechStore.Repository.Validation;


namespace WebsiteTechStore.Models
{

    public enum ProductStatus
    {
        OutOfStock = 0,
        InStock = 1,
        Discontinued = 2
    }

    public class ProductModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập tên sản phẩm")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập mô tả sản phẩm")]
        public string Description { get; set; }

        public string Slug { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập giá sản phẩm")]
        [Range(0.01,double.MaxValue)]
        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Status { get; set; }
        [Required,Range(1,int.MaxValue,ErrorMessage = "Yêu cầu nhập 1 danh mục")]
        public int CategoryId { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Yêu cầu nhập 1 thương hiệu")]
        public int BrandId { get; set; }
        public CategoryModel Category { get; set; }
        public BrandModel Brand { get; set; }
        public string Image { get; set; }
        [NotMapped]
        [FileExtension]
        public IFormFile? ImageUpload { get; set; }
    }
}
