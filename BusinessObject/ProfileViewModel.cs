using System.ComponentModel.DataAnnotations;

namespace BusinessObject
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Tên công ty không được để trống")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Thành phố không được để trống")]
        public string City { get; set; }

        [Required(ErrorMessage = "Quốc gia không được để trống")]
        public string Country { get; set; }
    }
}
