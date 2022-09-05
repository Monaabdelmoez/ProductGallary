using System.ComponentModel.DataAnnotations;
namespace ProductGallary.TDO
    {
        public class GalaryInfoDTO
        {
            public Guid Id { get; set; }
            public string user_id { get; set; }
            [Required, MinLength(3), MaxLength(20)]
            public string Name { get; set; }

            [Required]
            public string Logo { get; set; }

            [Required, DataType(DataType.DateTime)]
            public DateTime Created_Date { get; set; }

        }
    }

