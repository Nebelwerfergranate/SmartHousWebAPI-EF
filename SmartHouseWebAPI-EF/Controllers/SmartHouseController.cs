using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using SmartHouse;
using SmartHouseMVC.Models.DeviceManager;
using SmartHouseWebAPI_EF.Models.DeviceManager;
using SmartHouseWF.Models.DeviceManager;

namespace SmartHouseWebAPI_EF.Controllers
{
    public class SmartHouseController : ApiController
    {
        // Fields
        private DatabaseDeviceManager deviceManager = new DatabaseDeviceManager();

        // GET: api/SmartHouse
        public ICollection<DeviceWrapper> Get()
        {
            ICollection devices = deviceManager.GetDevices();
            ICollection<DeviceWrapper> wrappers = new List<DeviceWrapper>();
            if (devices.Count > 0)
            {
                foreach (KeyValuePair<int, Device> pair in devices)
                {
                    wrappers.Add(new DeviceWrapper(pair.Value, pair.Key));
                }
            }
            return wrappers;
        }

        // GET: api/SmartHouse/5
        public DeviceWrapper Get(int id)
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
            deviceManager.AddDevice(value);
        }

        // PUT: api/SmartHouse/5
        [HttpPut]
        [Route("api/SmartHouse/RenameDevice/{id}")]
        public void Put(int id, [FromBody]string value)
        {
            if (value == null)
            {
                value = "";
            }
            Device device = deviceManager.GetDeviceById(id);
            if (device != null)
            {
                device.Name = value;
                deviceManager.UpdateDeviceById(id, device);
            }
        }

        [HttpPut]
        [Route("api/SmartHouse/ToogleDevice/{id}")]
        public void ToogleDevice(int id, [FromBody] string value)
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
                deviceManager.UpdateDeviceById(id, device);
            }
        }

        // IClock
        [HttpPut]
        [Route("api/SmartHouse/SetTime/{id}")]
        public void SetTime(int id, [FromBody] TimeInfo value)
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
                deviceManager.UpdateDeviceById(id, device);
            }
        }

        // IOpenable
        [HttpPut]
        [Route("api/SmartHouse/ToogleDoor/{id}")]
        public void ToogleDoor(int id, [FromBody] string value)
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
                deviceManager.UpdateDeviceById(id, device);
            }
        }

        // ITemperature
        [HttpPut]
        [Route("api/SmartHouse/SetTemperature/{id}")]
        public void SetTemperature(int id, [FromBody] double value)
        {
            Device device = deviceManager.GetDeviceById(id);
            if (device != null)
            {
                ((ITemperature)device).Temperature = value;
                deviceManager.UpdateDeviceById(id, device);
            }
        }

        //ITimer
        [HttpPut]
        [Route("api/SmartHouse/TimerSet/{id}")]
        public void TimerSet(int id, [FromBody] TimeInfo value)
        {
            if (!value.IsValid)
            {
                return;
            }
            Device device = deviceManager.GetDeviceById(id);
            if (device != null)
            {
                ((ITimer)device).SetTimer(new TimeSpan(value.Hours, value.Minutes, value.Seconds));
                deviceManager.UpdateDeviceById(id, device);
            }
        }
        
        [HttpPut]
        [Route("api/SmartHouse/StartTimer/{id}")]
        public void StartTimer(int id, [FromBody] string value)
        {
            Device device = deviceManager.GetDeviceById(id);
            if (device != null)
            {
                ((ITimer)device).Start();
                
                deviceManager.UpdateDeviceById(id, device);
            }
        }

        [HttpPut]
        [Route("api/SmartHouse/PauseTimer/{id}")]
        public void PauseTimer(int id, [FromBody] string value)
        {
            Device device = deviceManager.GetDeviceById(id);
            if (device != null)
            {
                ((ITimer)device).Pause();

                deviceManager.UpdateDeviceById(id, device);
            }
        }

        [HttpPut]
        [Route("api/SmartHouse/StopTimer/{id}")]
        public void StopTimer(int id, [FromBody] string value)
        {
            Device device = deviceManager.GetDeviceById(id);
            if (device != null)
            {
                ((ITimer)device).Stop();

                deviceManager.UpdateDeviceById(id, device);
            }
        }

        // Fridge
        [HttpPut]
        [Route("api/SmartHouse/ToogleColdstoreDoor/{id}")]
        public void ToogleColdstoreDoor(int id, [FromBody] string value)
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
                deviceManager.UpdateDeviceById(id, device);
            }
        }

        [HttpPut]
        [Route("api/SmartHouse/ToogleFreezerDoor/{id}")]
        public void ToogleFreezerDoor(int id, [FromBody] string value)
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
                deviceManager.UpdateDeviceById(id, device);
            }
        }

        [HttpPut]
        [Route("api/SmartHouse/SetColdstoreTemperature/{id}")]
        public void SetColdstoreTemperature(int id, [FromBody] double value)
        {
            Device device = deviceManager.GetDeviceById(id);
            if (device != null)
            {
                Fridge fridge = (Fridge)device;
                fridge.ColdstoreTemperature = value;
                deviceManager.UpdateDeviceById(id, device);

            }
        }

        [HttpPut]
        [Route("api/SmartHouse/SetFreezerTemperature/{id}")]
        public void SetFreezerTemperature(int id, [FromBody] double value)
        {
            Device device = deviceManager.GetDeviceById(id);
            if (device != null)
            {
                Fridge fridge = (Fridge)device;
                fridge.FreezerTemperature = value;
                deviceManager.UpdateDeviceById(id, device);
            }
        }

        // DELETE: api/SmartHouse/5
        public void Delete(int id)
        {
            deviceManager.RemoveById(id);
        }
    }
}
