using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebClient.Models
{
    public class BicycleRack : Inventory
    {
        /// <summary>
        /// Limit of how many bikes rack can hold
        /// </summary>
        [Required(ErrorMessage = "Talpa privalomas laukas!")]
        [Range(1, 4, ErrorMessage = "Galima talpa: 1 ir 4")]
        [Display(Name = "Talpa")]
        public int BikeLimit { get; set; }
        /// <summary>
        /// Weight limit (kg)
        /// </summary>
        [Required(ErrorMessage = "Keliamoji galia privalomas laukas!")]
        [Range(0, double.MaxValue, ErrorMessage = "Neleistina reikšmė")]
        [Display(Name = "Keliamoji galia")]
        public double LiftPower { get; set; }
        /// <summary>
        /// Rack assertion type
        /// </summary>
        [Required(ErrorMessage = "Privaloma nurodyti tvirtinimo tipą!")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Display(Name = "Keliamoji galia")]
        public AssertionType Assertion { get; set; }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = base.Validate(validationContext).ToList();
            List<string> members = new List<string>();
            if (LiftPower < 0)
            {
                members.Add(nameof(LiftPower));
                results.Add(new ValidationResult("Keliamoji galia negali būti neigiama", members));
            }
            return results;
        }
        /// <summary>
        /// Ways of how rack can be asserted
        /// </summary>
        public enum AssertionType
        {
            Roof,
            Hook,
            Other
        }
    }
}
