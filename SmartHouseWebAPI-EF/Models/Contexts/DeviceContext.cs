using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SmartHouse;
using SmartHouseMVC.Models.DeviceManager;

namespace SmartHouseMVC.Models.Contexts
{
    public class DeviceContext : DbContext
    {
        static DeviceContext()
        {
            Database.SetInitializer(new DeviceContextInitializer());
        }
        public DeviceContext() : base("DeviceConnection") { }

        public DbSet<DatabaseMapping> DatabaseMappings { get; set; }
        public DbSet<Clock> Clocks { get; set; }
        public DbSet<Microwave> Microwaves { get; set; }
        public DbSet<Oven> Ovens { get; set; }
        public DbSet<Fridge> Fridges { get; set; }
    }
}