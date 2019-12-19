using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebClient.Models
{
    public class WheelChain : Inventory
    {
        /// <summary>
        /// Tire dimensions written in plain text
        /// </summary>
        [StringLength(255, ErrorMessage = "Padangų išmatavimų aprašas turi būti nuo 5 iki 255 simbolių ilgio.", MinimumLength = 0)]
        [DataType(DataType.Text, ErrorMessage = "Negalima reikšmė.")]
        [Display(Name = "Padangų išmatavimas")]
        public string TireDimensions { get; set; }
        /// <summary>
        /// Chain thickness in mm's
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Neleistina reikšmė")]
        [Display(Name = "Padangų storis")]
        public double ChainThickness { get; set; }
        /// <summary>
        /// On which vehicle type wheelchain is applicable
        /// </summary>
        [Required(ErrorMessage = "Privaloma nurodyti automobilio tipą!")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Display(Name = "Automobilio tipas")]
        public VehicleType Type { get; set; }
        /// <summary>
        /// Vehicle types
        /// </summary>
        public enum VehicleType
        {
            Car,
            SUV,
            Multivan,
            Other
        }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext);
        }
    }
}
