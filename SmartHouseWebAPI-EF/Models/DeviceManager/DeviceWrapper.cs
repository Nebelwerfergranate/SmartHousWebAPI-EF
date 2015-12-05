using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartHouse;

namespace SmartHouseWebAPI_EF.Models.DeviceManager
{
    public class DeviceWrapper
    {
        // Fields
        private Device device;
        private string deviceType;
        private ICollection<string> interfaces;


        // Constructors
        public DeviceWrapper(Device device)
        {
            Device = device;
        }

        public DeviceWrapper(Device device, uint id)
        {
            Device = device;
            Id = id;
        }


        // Properties
        public Device Device
        {
            get { return device; }
            set
            {
                device = value;

                DeviceType = device.GetType().FullName;

                Type[] interfacesTypes = device.GetType().GetInterfaces();
                Interfaces = new List<string>();
                foreach (Type type in interfacesTypes)
                {
                    Interfaces.Add(type.FullName);
                }
            }
        }
        
        public string DeviceType
        {
            get { return deviceType; }
            private set { deviceType = value; }
        }

        public ICollection<string> Interfaces
        {
            get { return interfaces; }
            private set { interfaces = value; }
        }
        public uint Id { get; set; }
    }
}