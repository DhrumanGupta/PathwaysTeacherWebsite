using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class ResetPasswordModel
    {
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Token { get; set; }
    }
}
