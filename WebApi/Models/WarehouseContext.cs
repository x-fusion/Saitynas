using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class WarehouseContext : DbContext
    {
        public WarehouseContext(DbContextOptions dbOptions) : base (dbOptions)
        {

        }
        public DbSet<BicycleRack> BicycleRacks { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<RoofRack> RoofRacks { get; set; }
        public DbSet<WheelChain> WheelChains { get; set; }
    }
}
