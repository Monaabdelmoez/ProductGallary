using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductGallary.Models
{
    [Table("Bill")]
    public class Bill
    {
        [Key]
        public Guid Id{ get; set; }

        public float Price{ get; set; }

        [ForeignKey("Order")]
        public Guid? OrderId { get; set; }
        public Order Order { get; set; }

    }
}
