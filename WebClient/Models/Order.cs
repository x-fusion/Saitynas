using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.Models
{
    public class Order
    {
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
        public int? BicycleRackID { get; set; }
        public int? InventoryID { get; set; }
        public int? WheelChainID { get; set; }
        [Display(Name = "Užsakymo pabaigos data")]
        public DateTime OrderEndDate { get; set; }
    }
}
