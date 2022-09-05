using Microsoft.AspNetCore.Identity;
using ProductGallary.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductGallary.Models
{
    [Table("User")]
    public class ApplicationUser:IdentityUser
    {
        [UniqueUserName] // this is custome username used to validate username is unique
        public override string UserName { get; set; }

        [Required]
        [MaxLength(40),MinLength(10)]
        public string Name { get; set; }

        [Required,MinLength(5),MaxLength(30)]
        public string Address { get; set; }
        public virtual List<Cart> Carts { get; set; }

        public virtual List<Product>Products { get; set; }
        public virtual List<Gallary> Gallaries { get; set; }

        public virtual List<Category> Categories { get; set; }
        public virtual List<Order> Orders  { get; set; }


    }
}
