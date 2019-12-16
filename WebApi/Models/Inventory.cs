using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Inventory : IValidatableObject
    {
        /// <summary>
        /// Identification key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// Item name
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Pavadinimas yra privalomas laukas!")]
        [StringLength(255, ErrorMessage = "Pavadinimo ilgis turi būti tarp 5 ir 255 simbolių.", MinimumLength = 5)]
        [DataType(DataType.Text, ErrorMessage = "Neleistinas pavadinimas.")]
        [Display(Name = "Pavadinimas")]
        public string Title { get; set; }
        /// <summary>
        /// Item count
        /// </summary>
        [Required(ErrorMessage = "Kiekis yra privalomas laukas!")]
        [Range(0, int.MaxValue, ErrorMessage = "Neleistina reikšmė.")]
        [Display(Name = "Kiekis")]
        public int Amount { get; set; }
        /// <summary>
        /// Revenue generated during rentals
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Neleistina reikšmė.")]
        [DataType(DataType.Currency, ErrorMessage = "Neleistinas duomenų tipas.")]
        [Display(Name = "Uždirbta")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Revenue { get; set; }
        /// <summary>
        /// Days spent at rent
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Neleistina reikšmė.")]
        [Display(Name = "Nuomos trukmė")]
        public int TotalRentDuration { get; set; }
        /// <summary>
        /// Price used for selling purposes
        /// </summary>
        [Required(ErrorMessage = "Vertė yra privaloma reikšmė!")]
        [Range(0, double.MaxValue, ErrorMessage = "Neleistina reikšmė.")]
        [DataType(DataType.Currency, ErrorMessage = "Neleistinas duomenų tipas.")]
        [Display(Name = "Kaina")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal MonetaryValue { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            List<string> members = new List<string>();
            if (MonetaryValue < 0)
            {
                members.Add(nameof(MonetaryValue));
                results.Add(new ValidationResult("Kaina negali būti neigiama.", members));
            }
            if (Amount < 0)
            {
                members.Add(nameof(Amount));
                results.Add(new ValidationResult("Kiekis negali būti neigiamas.", members));
            }
            if (Revenue < 0)
            {
                members.Add(nameof(Revenue));
                results.Add(new ValidationResult("Uždarbis negali būti neigiamas.", members));
            }
            if (TotalRentDuration < 0)
            {
                members.Add(nameof(Revenue));
                results.Add(new ValidationResult("Nuomos trukmė negali būti neigiama.", members));
            }
            return results;
        }
    }
}
