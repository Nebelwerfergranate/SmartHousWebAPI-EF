using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartHouse;

namespace SmartHouseWF.Models.DeviceManager
{
    public class MicrowaveInfo
    {
        public Lamp Lamp { get; set; }
        public int Volume { get; set; }

        public MicrowaveInfo()
        {
            
        }

        public MicrowaveInfo(int volume, Lamp lamp)
        {
            Lamp = lamp;
            Volume = volume;
        }
    }
}