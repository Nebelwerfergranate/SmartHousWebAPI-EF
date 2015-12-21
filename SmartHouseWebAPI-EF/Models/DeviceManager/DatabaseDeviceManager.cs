using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SmartHouse;
using SmartHouseMVC.Models.Contexts;
using SmartHouseWebAPI_EF.Models.DeviceManager;
using SmartHouseWF.Models.DeviceManager;

namespace SmartHouseMVC.Models.DeviceManager
{
    public class DatabaseDeviceManager : IDisposable
    {

        private DeviceContext _deviceContext = new DeviceContext();

        private Dictionary<string, MicrowaveFabricatorInfo> microwaveFabricatorInfo = new Dictionary<string, MicrowaveFabricatorInfo>();
        private Dictionary<string, OvenFabricatorInfo> ovenFabricatorInfo = new Dictionary<string, OvenFabricatorInfo>();
        private Dictionary<string, FridgeFabricatorInfo> fridgeFabricatorInfo = new Dictionary<string, FridgeFabricatorInfo>();

        public DatabaseDeviceManager()
        {
            InitMicrowaveFabricatorInfo();
            InitOvenFabricatorInfo();
            InitFridgeFabricatorInfo();
        }

        public Device GetDeviceById(int id)
        {
            DatabaseMapping databaseMapping = _deviceContext.DatabaseMappings.Find(id);
            if (databaseMapping == null)
            {
                return null;
            }
            Device device = null;
            switch (databaseMapping.DeviceTypeId)
            {
                case 1:
                    device = databaseMapping.Clock;
                    break;
                case 2:
                    device = databaseMapping.Microwave;
                    ((ITimer)device).CheckIsReady();
                    break;
                case 3:
                    device = databaseMapping.Oven;
                    ((ITimer)device).CheckIsReady();
                    break;
                case 4:
                    device = databaseMapping.Fridge;
                    break;
            }
            return device;
        }

        public void UpdateDeviceById(int id, Device device)
        {
            DatabaseMapping databaseMapping = _deviceContext.DatabaseMappings.Find(id);
            if (databaseMapping == null)
            {
                return;
            }
            switch (databaseMapping.DeviceTypeId)
            {
                case 1:
                    if (device is Clock)
                    {
                        _deviceContext.Entry((Clock)device).State = EntityState.Modified;
                        _deviceContext.SaveChanges();
                    }
                    break;
                case 2:
                    if (device is Microwave)
                    {
                        _deviceContext.Entry((Microwave)device).State = EntityState.Modified;
                        _deviceContext.SaveChanges();
                    }
                    break;
                case 3:
                    if (device is Oven)
                    {
                        _deviceContext.Entry((Oven)device).State = EntityState.Modified;
                        _deviceContext.SaveChanges();
                    }
                    break;
                case 4:
                    if (device is Fridge)
                    {
                        _deviceContext.Entry((Fridge)device).State = EntityState.Modified;
                        _deviceContext.SaveChanges();
                    }
                    break;
            }
        }

        public SortedDictionary<int, Device> GetDevices()
        {
            SortedDictionary<int, Device> devicesToReturn = new SortedDictionary<int, Device>();
            var databaseMappings = _deviceContext.DatabaseMappings;
            foreach (DatabaseMapping item in databaseMappings)
            {
                Device device = GetDeviceById(item.Id);
                devicesToReturn.Add(item.Id, device);
            }
            return devicesToReturn;
        }

        public void AddDevice(string device = "", string name = "", string fabricator = "")
        {
            DatabaseMapping databaseMapping = null;
            switch (device)
            {
                case "clock":
                    Clock clock = new Clock(name);
                    _deviceContext.Clocks.Add(clock);
                    databaseMapping = new DatabaseMapping { DeviceTypeId = 1, Clock = clock };
                    break;
                case "microwave":
                    MicrowaveFabricatorInfo mi = microwaveFabricatorInfo[fabricator];
                    Microwave microwave = new Microwave(name, mi.Volume, mi.Lamp);
                    _deviceContext.Microwaves.Add(microwave);
                    databaseMapping = new DatabaseMapping { DeviceTypeId = 2, Microwave = microwave };
                    break;
                case "oven":
                    OvenFabricatorInfo oi = ovenFabricatorInfo[fabricator];
                    Oven oven = new Oven(name, oi.Volume, oi.Lamp);
                    _deviceContext.Ovens.Add(oven);
                    databaseMapping = new DatabaseMapping { DeviceTypeId = 3, Oven = oven };
                    break;
                case "fridge":
                    FridgeFabricatorInfo fi = fridgeFabricatorInfo[fabricator];
                    Fridge fridge = new Fridge(name, fi.Coldstore, fi.Freezer);
                    _deviceContext.Fridges.Add(fridge);
                    databaseMapping = new DatabaseMapping { DeviceTypeId = 4, Fridge = fridge };
                    break;
                default: return;
            }
            _deviceContext.DatabaseMappings.Add(databaseMapping);
            _deviceContext.SaveChanges();
        }

        public void AddDevice(NewDeviceInfo info)
        {
            AddDevice(info.Device, info.Name, info.Fabricator);
        }

        public void RemoveById(int id)
        {
            DatabaseMapping databaseMapping = _deviceContext.DatabaseMappings.Find(id);
            if (databaseMapping == null)
            {
                return;
            }

            Device device = GetDeviceById(id);

            if (device is Clock)
            {
                _deviceContext.Clocks.Remove((Clock)device);
            }
            else if (device is Microwave)
            {
                _deviceContext.Microwaves.Remove((Microwave)device);
            }
            else if (device is Oven)
            {
                _deviceContext.Ovens.Remove((Oven)device);
            }
            else if (device is Fridge)
            {
                _deviceContext.Fridges.Remove((Fridge)device);
            }
            _deviceContext.DatabaseMappings.Remove(databaseMapping);
            _deviceContext.SaveChanges();
        }

        public string[] GetMicrowaveNames()
        {
            return microwaveFabricatorInfo.Keys.ToArray();
        }

        public string[] GetOvenNames()
        {
            return ovenFabricatorInfo.Keys.ToArray();
        }

        public string[] GetFridgeNames()
        {
            return fridgeFabricatorInfo.Keys.ToArray();
        }


        private void InitMicrowaveFabricatorInfo()
        {
            microwaveFabricatorInfo["Whirlpool"] = new MicrowaveFabricatorInfo(20, new Lamp(25));
            microwaveFabricatorInfo["Panasonic"] = new MicrowaveFabricatorInfo(19, new Lamp(20));
            microwaveFabricatorInfo["Lg"] = new MicrowaveFabricatorInfo(23, new Lamp(25));
        }

        private void InitOvenFabricatorInfo()
        {
            ovenFabricatorInfo["Siemens"] = new OvenFabricatorInfo(67, new Lamp(25));
            ovenFabricatorInfo["Electrolux"] = new OvenFabricatorInfo(74, new Lamp(25));
            ovenFabricatorInfo["Pyramida"] = new OvenFabricatorInfo(56, new Lamp(15));
        }

        private void InitFridgeFabricatorInfo()
        {
            fridgeFabricatorInfo["Samsung"] = new FridgeFabricatorInfo(new Coldstore(254, new Lamp(15)), new Freezer(92));
            fridgeFabricatorInfo["Indesit"] = new FridgeFabricatorInfo(new Coldstore(233, new Lamp(15)), new Freezer(85));
            fridgeFabricatorInfo["Atlant"] = new FridgeFabricatorInfo(new Coldstore(202, new Lamp(15)), new Freezer(70));
        }


        void IDisposable.Dispose()
        {
            _deviceContext.Dispose();
        }
    }
}