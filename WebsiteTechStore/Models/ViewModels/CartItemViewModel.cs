namespace WebsiteTechStore.Models.ViewModels
{
    public class CartItemViewModel
    {
        public List<CartItemModel> CartItems { get; set; } 
        public decimal CartTotal { get;set;}
    }
}
