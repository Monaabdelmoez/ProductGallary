using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductGallary.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Image { get; set; }

        [Required]
        public float? Price { get; set; }

        public bool? HasDiscount { get; set; }

        public float? DiscountPercentage { get; set; }
        [Required]
        public string? Description { get; set; }

        [ForeignKey("User")]
        public string? User_Id { get; set; }

        public ApplicationUser? User { get; set; }

        [ForeignKey("Gallary")]
        public Guid? Gallary_Id { get; set; }
        [ForeignKey("Category")]
        public Guid? Category_Id { get; set; }
        public Gallary? Gallary { get; set; }
        public Category? Category { get; set; }

        public List<Cart>? carts { get; set; } 
    }
}
