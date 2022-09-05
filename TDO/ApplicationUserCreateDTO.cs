using Microsoft.AspNetCore.Identity;
using ProductGallary.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ProductGallary.TDO
{
    public class ApplicationUserCreateDTO
    {
        [UniqueUserName] // this is custome username used to validate username is unique
        [Display(Name="اسم المستخدم :")]
        [Required(ErrorMessage ="اسم المستخدم مطلوب *")]
        public  string UserName { get; set; }

        [Required(ErrorMessage ="الاسم مطلوب *")]
        [MaxLength(40), MinLength(10)]
        [Display(Name = "اسم بالكامل :")]
        public string Name { get; set; }

        [Required(ErrorMessage ="العنوان مطلوب *"), MinLength(5), MaxLength(30)]
        [Display(Name = "العنوان :")]
        public string Address { get; set; }
        //
        // Summary:
        //     Gets or sets a telephone number for the user.
        [Required(ErrorMessage = "رقم التليفون مطلوب *")]
        [Display(Name = "التليفون :")]
        public  string PhoneNumber{get;set;}

        [Required(ErrorMessage = "البريد الالكترونى مطلوب *")]
        [Display(Name = "البريد الالكترونى :")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "كلمه المرور مطلوبه *")]
        [Display(Name = "كلمه المرور :")]
        public string  Password{ get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "اعد كتابه كلمه المرور *"), Compare("Password",ErrorMessage ="  كلمات المرور غير متطابقه *")]
        [Display(Name = "اعاده كلمه المرور :")]
        public string ConfirmPassword { get; set; }



    }
}
