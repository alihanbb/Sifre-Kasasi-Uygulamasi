using System.ComponentModel.DataAnnotations;

namespace AppWeb.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Kullanıcı adı alanı boş bırakılamaz")]
        [Display(Name ="Kullanıcı Adı:")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="Şifre alanı boş bırakılamaz")]
        [DataType(DataType.Password)]
        [Display(Name ="Şifre")]
        public string Password { get; set; }
    }
}
