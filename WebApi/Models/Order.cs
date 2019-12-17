using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Order : IValidatableObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Display(Name = "Užsakovas")]
        [DataType(DataType.Text)]
        [StringLength(255, ErrorMessage = "Užsakovo ilgis turi būti tarp 5 ir 255 simbolių.", MinimumLength = 5)]
        public string Customer { get; set; }
        [Display(Name = "Užsakymo sukūrimo data")]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
        [Display(Name = "Užsakymo sukūrimo laikas")]
        [DataType(DataType.Time)]
        public TimeSpan CreationTime { get; set; }
        [Display(Name = "Užsakymo pradžios data")]
        [DataType(DataType.Date)]
        public DateTime OrderStartDate { get; set; }
        [Display(Name = "Užsakymo pabaigos data")]
        [DataType(DataType.Date)]
        public DateTime OrderEndDate { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            List<string> members = new List<string>();
            if(OrderEndDate.Date < OrderStartDate.Date)
            {
                results.Add(new ValidationResult(""))
            }
        }
    }
}
