using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Chat.Service.Models.Product
{
    public class CreateProductModel
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]

        public string Description { get; set; } = null!;
        [Required]

        public decimal InitialPrice { get; set; }
        [Required]
        public decimal MinimumStep { get; set; }
        public IFormFileCollection? Files { get; set; }
    }
}
