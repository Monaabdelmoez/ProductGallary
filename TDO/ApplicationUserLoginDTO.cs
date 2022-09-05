﻿using System.ComponentModel.DataAnnotations;

namespace ProductGallary.TDO
{
    // this class used to get user data to login
    public class ApplicationUserLoginDTO
    {
        [Required(ErrorMessage ="ادخل اسم المستخدم *")]
        [Display(Name ="اسم المستخدم : ")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="ادخل كلمه المرور *")]
        [Display(Name="كلمه المرور :")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name="تذكرنى")]
        public bool RememberMe { get; set; }
    }
}
