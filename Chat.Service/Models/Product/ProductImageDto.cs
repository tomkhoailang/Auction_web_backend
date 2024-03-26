using Microsoft.AspNetCore.Mvc;

namespace Chat.Service.Models.Product
{
    public class ProductImageDto
    {
        public int ProductImageId { get; set; }
        public int ProductId { get; set; }
        public FileStreamResult Image { get; set; }
    }
}
