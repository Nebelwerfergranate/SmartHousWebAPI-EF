using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartHouse;
using SmartHouseWF.Models.DeviceManager;

namespace SmartHouseMVC.Controllers
{
    public class HomeController : Controller
    {
        // Fields
        private SessionDeviceManager deviceManager = new SessionDeviceManager();

        // GET: Home
        public ActionResult Index()
        {
            string[] microwaveNames = deviceManager.GetMicrowaveNames();
            string[] ovenNames = deviceManager.GetOvenNames();
            string[] fridgeNames = deviceManager.GetFridgeNames();

            ViewBag.microwaveNames = microwaveNames;
            ViewBag.ovenNames = ovenNames;
            ViewBag.fridgeNames = fridgeNames;

            return View(deviceManager.GetDevices());
        }

        public RedirectResult AddDevice(string device = "", string name = "", string fabricator = "")
        {
            switch (device)
            {
                case "clock":
                    deviceManager.AddClock(name);
                    break;
                case "microwave":
                    deviceManager.AddMicrowave(name, fabricator);
                    break;
                case "oven":
                    deviceManager.AddOven(name, fabricator);
                    break;
                case "fridge":
                    deviceManager.AddFridge(name, fabricator);
                    break;
            }
            return Redirect("/Home/Index");
        }

        public RedirectResult RenameDevice(uint? id, string newName = "")
        {
            if (id != null)
            {
                deviceManager.RenameById((uint)id, newName);
            }
            return Redirect("/Home/Index");
        }

        public RedirectResult RemoveDevice(uint? id)
        {
            if (id != null)
            {
                deviceManager.RemoveById((uint)id);
            }
            return Redirect("/Home/Index");
        }

        // Device
        public RedirectResult ToogleDevice(uint? id)
        {
            if (id != null)
            {
                Device device = deviceManager.GetDeviceById((uint) id);
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
            return Redirect("/Home/Index");
        }
        
        //IClock
        public RedirectResult SetTime(uint? id, uint? hours, uint? minutes)
        {
            if (id != null && hours != null && minutes != null && hours < 24 && minutes < 60)
            {
                Device device = deviceManager.GetDeviceById((uint)id);
                if (device != null && device is IClock)
                {
                    ((IClock)device).CurrentTime = new DateTime(1, 1, 1, (int)hours, (int)minutes, 0);
                }
            }
            return Redirect("/Home/Index");
        }

        // IOpenable
        public RedirectResult ToogleDoor(uint? id)
        {
            if (id != null)
            {
                Device device = deviceManager.GetDeviceById((uint)id);
                if (device != null && device is IOpenable)
                {
                    IOpenable door = (IOpenable) device;
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
            return Redirect("/Home/Index");
        }
        
        // ITemperature
        public RedirectResult SetTemperature(uint? id, double? temperature)
        {
            if (id != null && temperature != null)
            {
                Device device = deviceManager.GetDeviceById((uint)id);
                if (device != null && device is ITemperature)
                {
                    ((ITemperature) device).Temperature = (double)temperature;
                }
            }
            return Redirect("/Home/Index");
        }

        // ITimer
        public RedirectResult TimerSet(uint? id, uint? hours, uint? minutes, uint? seconds)
        {
            if (hours == null)
            {
                hours = 0;
            }
            if(minutes == null)
            {
                minutes = 0;
            }
            if (seconds == null)
            {
                seconds = 0;
            }
            if (id != null && hours < 24 && minutes < 60 && seconds < 60)
            {
                Device device = deviceManager.GetDeviceById((uint)id);
                if (device != null && device is ITimer)
                {
                    ((ITimer)device).SetTimer(new TimeSpan((int)hours, (int)minutes, (int)seconds));
                }
            }
            return Redirect("/Home/Index");
        }

        public RedirectResult ToogleTimer(uint? id)
        {
            if (id != null)
            {
                Device device = deviceManager.GetDeviceById((uint)id);
                if (device != null && device is ITimer)
                {
                    ITimer iTimerObj = (ITimer) device;
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
            return Redirect("/Home/Index");
        }

        // Fridge
        public RedirectResult ToogleColdstoreDoor(uint? id)
        {
            if (id != null)
            {
                Device device = deviceManager.GetDeviceById((uint) id);
                if (device != null && device is Fridge)
                {
                    Fridge fridge = (Fridge) device;
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
            return Redirect("/Home/Index");
        }

        public RedirectResult ToogleFreezerDoor(uint? id)
        {
            if (id != null)
            {
                Device device = deviceManager.GetDeviceById((uint) id);
                if (device != null && device is Fridge)
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
            return Redirect("/Home/Index");
        }

        public RedirectResult SetColdstoreTemperature(uint? id, double? temperature)
        {
            if (id != null && temperature != null)
            {
                Device device = deviceManager.GetDeviceById((uint)id);
                if (device != null && device is Fridge)
                {
                    ((Fridge) device).ColdstoreTemperature = (double)temperature;
                }
            }
            return Redirect("/Home/Index");
        }

        public RedirectResult SetFreezerTemperature(uint? id, double? temperature)
        {
            if (id != null && temperature != null)
            {
                Device device = deviceManager.GetDeviceById((uint)id);
                if (device != null && device is Fridge)
                {
                    ((Fridge) device).FreezerTemperature = (double) temperature;
                }
            }
            return Redirect("/Home/Index");
        }
    }
}