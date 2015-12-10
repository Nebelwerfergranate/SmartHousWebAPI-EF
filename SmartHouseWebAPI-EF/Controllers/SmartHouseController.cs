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
        [HttpPut]
        [Route("api/SmartHouse/RenameDevice/{id}")]
        public void Put(uint id, [FromBody]string value)
        {
            if (value == null)
            {
                value = "";
            }
            deviceManager.RenameById(id, value);
        }

        [HttpPut]
        [Route("api/SmartHouse/ToogleDevice/{id}")]
        public void ToogleDevice(uint id, [FromBody] string value)
        {
            Device device = deviceManager.GetDeviceById(id);
            if (device != null)
            {
                if (device.IsOn)
                {
                    device.TurnOff();
                }
                else
                {
                    device.TurnOn();
                }
            }
        }

        // IClock
        [HttpPut]
        [Route("api/SmartHouse/SetTime/{id}")]
        public void SetTime(uint id, [FromBody] TimeInfo value)
        {
            if (!value.IsValid)
            {
                return;
            }
            Device device = deviceManager.GetDeviceById(id);
            if (device != null)
            {
                IClock iClockObj = (IClock)device;
                iClockObj.CurrentTime = new DateTime(1, 1, 1, value.Hours, value.Minutes, 0);
            }
        }

        // IOpenable
        [HttpPut]
        [Route("api/SmartHouse/ToogleDoor/{id}")]
        public void ToogleDoor(uint id, [FromBody] string value)
        {
            Device device = deviceManager.GetDeviceById(id);
            if (device != null)
            {
                IOpenable door = (IOpenable)device;
                if (door.IsOpen)
                {
                    door.Close();
                }
                else
                {
                    door.Open();
                }
            }
        }

        // ITemperature
        [HttpPut]
        [Route("api/SmartHouse/SetTemperature/{id}")]
        public void SetTemperature(uint id, [FromBody] double value)
        {
            Device device = deviceManager.GetDeviceById(id);
            if (device != null)
            {
                ((ITemperature)device).Temperature = value;
            }
        }

        //ITimer
        [HttpPut]
        [Route("api/SmartHouse/TimerSet/{id}")]
        public void TimerSet(uint id, [FromBody] TimeInfo value)
        {
            if (!value.IsValid)
            {
                return;
            }
            Device device = deviceManager.GetDeviceById(id);
            if (device != null)
            {
                ((ITimer)device).SetTimer(new TimeSpan(value.Hours, value.Minutes, value.Seconds));
            }
        }

        [HttpPut]
        [Route("api/SmartHouse/ToogleTimer/{id}")]
        public void ToogleTimer(uint id, [FromBody] string value)
        {
            Device device = deviceManager.GetDeviceById(id);
            if (device != null)
            {
                ITimer iTimerObj = (ITimer)device;
                if (iTimerObj.IsRunning)
                {
                    iTimerObj.Stop();
                }
                else
                {
                    iTimerObj.Start();
                }
            }
        }

        [HttpGet]
        [Route("api/SmartHouse/IsRunning/{id}")]
        public bool IsRunning(uint id)
        {
            bool isRunning = false;
            Device device = deviceManager.GetDeviceById(id);
            if (device != null)
            {
                isRunning = ((ITimer)device).IsRunning;
            }
            return isRunning;
        }

        // Fridge
        [HttpPut]
        [Route("api/SmartHouse/ToogleColdstoreDoor/{id}")]
        public void ToogleColdstoreDoor(uint id, [FromBody] string value)
        {
            Device device = deviceManager.GetDeviceById(id);
            if (device != null)
            {
                Fridge fridge = (Fridge)device;
                if (fridge.ColdstoreIsOpen)
                {
                    fridge.CloseColdstore();
                }
                else
                {
                    fridge.OpenColdstore();
                }
            }
        }

        [HttpPut]
        [Route("api/SmartHouse/ToogleFreezerDoor/{id}")]
        public void ToogleFreezerDoor(uint id, [FromBody] string value)
        {
            Device device = deviceManager.GetDeviceById(id);
            if (device != null)
            {
                Fridge fridge = (Fridge)device;
                if (fridge.FreezerIsOpen)
                {
                    fridge.CloseFreezer();
                }
                else
                {
                    fridge.OpenFreezer();
                }
            }
        }

        [HttpPut]
        [Route("api/SmartHouse/SetColdstoreTemperature/{id}")]
        public void SetColdstoreTemperature(uint id, [FromBody] double value)
        {
            Device device = deviceManager.GetDeviceById(id);
            if (device != null)
            {
                Fridge fridge = (Fridge)device;
                fridge.ColdstoreTemperature = value;

            }
        }

        [HttpPut]
        [Route("api/SmartHouse/SetFreezerTemperature/{id}")]
        public void SetFreezerTemperature(uint id, [FromBody] double value)
        {
            Device device = deviceManager.GetDeviceById(id);
            if (device != null)
            {
                Fridge fridge = (Fridge)device;
                fridge.FreezerTemperature = value;
            }
        }

        // DELETE: api/SmartHouse/5
        public void Delete(uint id)
        {
            deviceManager.RemoveById(id);
        }
    }
}
