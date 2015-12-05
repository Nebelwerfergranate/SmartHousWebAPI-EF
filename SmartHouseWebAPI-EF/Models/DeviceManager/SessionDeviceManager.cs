using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartHouse;

namespace SmartHouseWF.Models.DeviceManager
{
    public class SessionDeviceManager
    {
        // Fields
        private SortedDictionary<uint, Device> myDevices;
        private readonly HttpContext context = HttpContext.Current;
        private uint newDeviceID;
        private Dictionary<string, MicrowaveInfo> microwaveInfo = new Dictionary<string, MicrowaveInfo>();
        private Dictionary<string, OvenInfo> ovenInfo = new Dictionary<string, OvenInfo>();
        private Dictionary<string, FridgeInfo> fridgeInfo = new Dictionary<string, FridgeInfo>();


        // Constructors
        public SessionDeviceManager()
        {
            InitMicrowaveInfo();
            InitOvenInfo();
            InitFridgeInfo();
            if (context.Session["devices"] == null)
            {
                Init();
            }
            else
            {
                myDevices = (SortedDictionary<uint, Device>)context.Session["devices"];
                UInt32.TryParse(context.Session["newDeviceID"].ToString(), out newDeviceID);
            }
        }

        // Methods
        public void AddClock(string name)
        {
            Clock clock = new Clock(name);
            AddDevice(clock);
        }

        public void AddMicrowave(string name, string fabricator)
        {
            if (!microwaveInfo.ContainsKey(fabricator))
            {
                return;
            }
            MicrowaveInfo mi = microwaveInfo[fabricator];
            Microwave microwave = new Microwave(name, mi.Volume, mi.Lamp);
            microwave.OperationDone += (sender) =>
            {
                //throw new ApplicationException("Микроволновка отработала!!!");
            };
            AddDevice(microwave);
        }

        public void AddOven(string name, string fabricator)
        {
            if (!ovenInfo.ContainsKey(fabricator))
            {
                return;
            }
            OvenInfo oi = ovenInfo[fabricator];
            Oven oven = new Oven(name, oi.Volume, oi.Lamp);
            oven.OperationDone += (sender) =>
            {
                //throw new ApplicationException("Печка отработала!!!");
            };
            AddDevice(oven);
        }

        public void AddFridge(string name, string fabricator)
        {
            if (!fridgeInfo.ContainsKey(fabricator))
            {
                return;
            }
            FridgeInfo fi = fridgeInfo[fabricator];
            Fridge fridge = new Fridge(name, fi.Coldstore, fi.Freezer);
            AddDevice(fridge);
        }
        private void AddDevice(Device device)
        {
            myDevices.Add(newDeviceID, device);
            newDeviceID++;
            context.Session["devices"] = GetDevices();
            context.Session["newDeviceID"] = newDeviceID;
        }
        public void RemoveById(uint id)
        {
            if (myDevices.ContainsKey(id))
            {
                myDevices.Remove(id);
                context.Session["devices"] = GetDevices();
            }
        }

        public void RenameById(uint id, string newName)
        {
            if (myDevices.ContainsKey(id))
            {
                myDevices[id].Name = newName;
                context.Session["devices"] = GetDevices();
            }
        }

        public Device GetDeviceById(uint id)
        {
            Device deviceFound = null;
            if (myDevices.ContainsKey(id))
            {
                deviceFound = myDevices[id];
            }
            return deviceFound;
        }

        public SortedDictionary<uint, Device> GetDevices()
        {
            return myDevices;
        }

        public string[] GetMicrowaveNames()
        {
            return microwaveInfo.Keys.ToArray();
        }

        public string[] GetOvenNames()
        {
            return ovenInfo.Keys.ToArray();
        }

        public string[] GetFridgeNames()
        {
            return fridgeInfo.Keys.ToArray();
        }


        private void Init()
        {
            myDevices = new SortedDictionary<uint, Device>();
            newDeviceID = 0;
            context.Session["devices"] = GetDevices();
            context.Session["newDeviceID"] = newDeviceID;
        }

        private void InitMicrowaveInfo()
        {
            microwaveInfo["Whirlpool"] = new MicrowaveInfo(20, new Lamp(25));
            microwaveInfo["Panasonic"] = new MicrowaveInfo(19, new Lamp(20));
            microwaveInfo["Lg"] = new MicrowaveInfo(23, new Lamp(25));
        }

        private void InitOvenInfo()
        {
            ovenInfo["Siemens"] = new OvenInfo(67, new Lamp(25));
            ovenInfo["Electrolux"] = new OvenInfo(74, new Lamp(25));
            ovenInfo["Pyramida"] = new OvenInfo(56, new Lamp(15));
        }

        private void InitFridgeInfo()
        {
            fridgeInfo["Samsung"] = new FridgeInfo(new Coldstore(254, new Lamp(15)), new Freezer(92));
            fridgeInfo["Indesit"] = new FridgeInfo(new Coldstore(233, new Lamp(15)), new Freezer(85));
            fridgeInfo["Atlant"] = new FridgeInfo(new Coldstore(202, new Lamp(15)), new Freezer(70));
        }
    }
}
