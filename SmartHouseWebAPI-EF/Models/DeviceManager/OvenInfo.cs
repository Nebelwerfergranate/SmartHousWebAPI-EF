using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using SmartHouse;

namespace SmartHouseWF.Models.DeviceManager
{
    public class OvenInfo
    {
        public Lamp Lamp { get; set; }
        public int Volume { get; set; }

        public OvenInfo()
        {
            
        }

        public OvenInfo(int volume, Lamp lamp)
        {
            Lamp = lamp;
            Volume = volume;
        }
    }
}