using System.ComponentModel.DataAnnotations;

namespace Product.API.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public int Quantity { get; set; }

        public float Price { get; set; }
    }
}
