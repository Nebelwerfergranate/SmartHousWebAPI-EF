using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartHouse;

namespace SmartHouseWF.Models.DeviceManager
{
    public class MicrowaveFabricatorInfo
    {
        public Lamp Lamp { get; set; }
        public int Volume { get; set; }

        public MicrowaveFabricatorInfo()
        {
            
        }

        public MicrowaveFabricatorInfo(int volume, Lamp lamp)
        {
            Lamp = lamp;
            Volume = volume;
        }
    }
}