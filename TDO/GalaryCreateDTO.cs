using System.ComponentModel.DataAnnotations;

namespace ProductGallary.TDO
{
    public class GalaryCreateDTO
    {
        
        [Required(ErrorMessage = "من فضلك ادخل اسم المعرض")]
        [MinLength(3, ErrorMessage = "يجب ان يكون الاسم اكثر من حرفين")]
        [MaxLength(20, ErrorMessage = "يجب ان يكون الاسم اقل من  عشرون حرفا")]
        public string name { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل لوجو المعرض")]
        public IFormFile Logo { get; set; }


    }
}
