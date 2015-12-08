using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using SmartHouse;
using SmartHouseWebAPI_EF.Models.DeviceManager;
using SmartHouseWF.Models.DeviceManager;

namespace SmartHouseWebAPI_EF.Controllers
{
    public class SmartHouseController : ApiController
    {
        // Fields
        private SessionDeviceManager deviceManager = new SessionDeviceManager();

        // GET: api/SmartHouse
        public ICollection<DeviceWrapper> Get()
        {
            ICollection devices = deviceManager.GetDevices();
            ICollection<DeviceWrapper> wrappers = new List<DeviceWrapper>();
            if (devices.Count > 0)
            {
                foreach (KeyValuePair<uint, Device> pair in devices)
                {
                    wrappers.Add(new DeviceWrapper(pair.Value, pair.Key));
                }
            }
            return wrappers;
        }

        // GET: api/SmartHouse/5
        public DeviceWrapper Get(uint id)
        {
            DeviceWrapper wrapper = null;
            Device device = deviceManager.GetDeviceById(id);
            if (device != null)
            {
                wrapper = new DeviceWrapper(device, id);
            }
            return wrapper;
        }

        // POST: api/SmartHouse
        public void Post([FromBody]NewDeviceInfo value)
        {
            if (value == null)
            {
                return;
            }


            switch (value.Device)
            {
                case "clock":
                    deviceManager.AddClock(value.Name);
                    break;
                case "microwave":
                    deviceManager.AddMicrowave(value.Name, value.Fabricator);
                    break;
                case "oven":
                    deviceManager.AddOven(value.Name, value.Fabricator);
                    break;
                case "fridge":
                    deviceManager.AddFridge(value.Name, value.Fabricator);
                    break;
            }
        }

        // PUT: api/SmartHouse/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/SmartHouse/5
        public void Delete(int id)
        {
        }
    }
}
