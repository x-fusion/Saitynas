using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required(ErrorMessage = "Privaloma nurodyti vardą")]
        [StringLength(45, ErrorMessage = "Ilgis privalo būti tarp 5 ir 45 simbolių", MinimumLength = 5)]
        [DataType(DataType.Text, ErrorMessage = "Netinkamas vardas")]
        [Display(Name = "Vardas")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Privaloma nurodyti pavardę")]
        [StringLength(45, ErrorMessage = "Ilgis privalo būti tarp 5 ir 45 simbolių", MinimumLength = 5)]
        [DataType(DataType.Text, ErrorMessage = "Netinkamas vardas")]
        [Display(Name = "Pavardė")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Privaloma nurodyti pašto adresą")]
        [StringLength(255, ErrorMessage = "Length cannot exceed 45 characters")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Netinkamas paštas")]
        [Display(Name = "Paštas")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Slaptažodis")]
        [Required(ErrorMessage = "Privaloma nurodyti slaptažodį")]
        public string Password { get; set; }
        [DataType(DataType.Text)]
        public string Role { get; set; }
        [field: NonSerialized]
        public string Token { get; set; }
    }
}
