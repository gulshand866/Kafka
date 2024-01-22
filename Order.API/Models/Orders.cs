using System.ComponentModel.DataAnnotations;

namespace Order.API.Models
{
    public class Orders
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int NumberOfItems { get; set; }

        public float TotalPrice { get; set; }
    }
}
