using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductGallary.Models
{
    [Table("Gallary")]
    public class Gallary
    {

        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage="من فضلك ادخل اسم المعرض")]
        [MinLength(3,ErrorMessage ="يجب ان يكون الاسم اكثر من حرفين")]
        [MaxLength(20,ErrorMessage = "يجب ان يكون الاسم اقل من  عشرون حرفا")]
        public string Name { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل لوجو المعرض")]
        public string Logo { get; set; }

        [Required,DataType(DataType.DateTime)]
        public DateTime Created_Date { get; set; }

        [ForeignKey("User")]
        public string? User_Id { get; set; }
        public ApplicationUser? User { get; set; }
        public virtual List<Product>? Products { get; set; }

    }
}
