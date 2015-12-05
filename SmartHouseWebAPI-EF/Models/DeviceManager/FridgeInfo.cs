using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartHouse;

namespace SmartHouseWF.Models.DeviceManager
{
    public class FridgeInfo
    {
        // Constructors
        public FridgeInfo()
        {

        }
        public FridgeInfo(Coldstore coldstore, Freezer freezer)
        {
            Coldstore = coldstore;
            Freezer = freezer;
        }


        // Properties
        public Coldstore Coldstore { get; set; }
        public Freezer Freezer { get; set; }
    }
}