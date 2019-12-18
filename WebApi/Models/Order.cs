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
        public int WarehouseID { get; set; }
        [Display(Name = "Užsakovas")]
        [DataType(DataType.Text)]
        [StringLength(255, ErrorMessage = "Užsakovo ilgis turi būti tarp 5 ir 255 simbolių.", MinimumLength = 5)]
        public string Customer { get; set; }
        [Display(Name = "Užsakymo sukūrimo data")]
        public DateTime CreationDate { get; set; }
        [Display(Name = "Užsakymo pradžios data")]
        public DateTime OrderStartDate { get; set; }
        public int? RoofRackID { get; set; }
        [ForeignKey("RoofRackID")]
        public virtual RoofRack RoofRack { get; set; }
        public int? BicycleRackID { get; set; }
        [ForeignKey("BicycleRackID")]
        public virtual BicycleRack BicycleRack { get; set; }
        public int? InventoryID { get; set; }
        [ForeignKey("InventoryID")]
        public virtual Inventory Inventory { get; set; }
        public int? WheelChainID { get; set; }
        [ForeignKey("WheelChainID")]
        public virtual WheelChain WheelChain { get; set; }
        [Display(Name = "Užsakymo pabaigos data")]
        public DateTime OrderEndDate { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            List<string> members = new List<string>();
            if(OrderEndDate.Date < OrderStartDate.Date)
            {
                members.Add(nameof(OrderEndDate));
                results.Add(new ValidationResult("Užsakymo pradžia negali būti vėliau už pabaigą.", members));
            }
            return results;
        }
    }
}
