using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Warehouse
    {
        public Warehouse ()
        {
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Pavadinimas yra privalomas laukas!")]
        [StringLength(255, ErrorMessage = "Pavadinimo ilgis turi būti tarp 5 ir 255 simbolių.", MinimumLength = 5)]
        [DataType(DataType.Text, ErrorMessage = "Neleistinas pavadinimas.")]
        [Display(Name = "Pavadinimas")]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Adresas yra privalomas laukas!")]
        [StringLength(255, ErrorMessage = "Adreso ilgis turi būti tarp 5 ir 255 simbolių.", MinimumLength = 5)]
        [DataType(DataType.Text, ErrorMessage = "Neleistinas adresas.")]
        [Display(Name = "Adresas")]
        public string Address { get; set; }
        public virtual List<Order> Orders { get; set; }
    }
}
