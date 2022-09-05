using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductGallary.Models
{   
    [Table("Category")]
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        [Required,MaxLength(40),MinLength(3)]
        public string? Name { get; set; }

        [ForeignKey("User")]
        public string? User_Id { get; set; }

        public ApplicationUser? User { get; set; }

        public virtual List<Product>? Products { get; set; }
    }
}
