using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.Models
{
    public class Login
    {
        [Display(Name = "El.paštas")]
        [Required(ErrorMessage = "Privaloma nurodyti")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Slaptažodis")]
        [Required(ErrorMessage = "Privaloma nurodyti")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
