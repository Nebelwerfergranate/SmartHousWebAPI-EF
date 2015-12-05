using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
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
        public void Post([FromBody]string value)
        {
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
