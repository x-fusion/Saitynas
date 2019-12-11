using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class RoofRack : Inventory
    {
        /// <summary>
        /// Describes roof rack way of opening
        /// </summary>
        [Required(ErrorMessage = "Privaloma nurodyti atidarimo tipą!")]
        [Display(Name = "Atidarimo tipas")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OpeningType Opening { get; set; }
        /// <summary>
        /// Weight limit (kg)
        /// </summary>
        [Required(ErrorMessage = "Privaloma nurodyti keliamają galią!")]
        [Range(0, double.MaxValue, ErrorMessage = "Neleistina reikšmė.")]
        [Display(Name = "Keliamoji galia")]
        public double LiftPower { get; set; }
        /// <summary>
        /// Indicates if roof rack has lock
        /// </summary>
        [Required(ErrorMessage = "Privaloma nurodyti ar bagažinė rakinama!")]
        [Display(Name = "Spyna")]
        public bool IsLockable { get; set; }
        /// <summary>
        /// Describes weight of roof rack itself (kg)
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Neleistina reikšmė.")]
        [Display(Name = "Svoris")]
        public double Weight { get; set; }
        /// <summary>
        /// Describes exterior of roof rack
        /// </summary>
        [StringLength(255, ErrorMessage = "Aprašymo ilgis turi būti tarp 5 ir 255 simbolių.", MinimumLength = 0)]
        [DataType(DataType.Text, ErrorMessage = "Neleistina reikšmė.")]
        [Display(Name = "Išvaizdos aprašas")]
        public string AppearenceDescription { get; set; }
        /// <summary>
        /// Custom property and object level validation
        /// </summary>
        /// <param name="validationContext">Properties and their values</param>
        /// <returns>Fields and their's errors</returns>
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
        /// Ways of how Roof rack can be opened
        /// </summary>
        public enum OpeningType
        {
            TwoSided,
            OneSided,
            RemovableTop
        }
    }
}
