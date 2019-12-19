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
        public virtual DbSet<BicycleRack> BicycleRacks { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<RoofRack> RoofRacks { get; set; }
        public virtual DbSet<WheelChain> WheelChains { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Warehouse> Warehouses { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
