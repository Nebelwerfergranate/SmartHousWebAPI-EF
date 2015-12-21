using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartHouse;
using SmartHouseMVC.Models.DeviceManager;
using SmartHouseWF.Models.DeviceManager;

namespace SmartHouseMVC.Controllers
{
    public class HomeController : Controller
    {
        // Fields
        private DatabaseDeviceManager deviceManager = new DatabaseDeviceManager();

        // GET: Home
        public ActionResult Index()
        {
            string[] microwaveNames = deviceManager.GetMicrowaveNames();
            string[] ovenNames = deviceManager.GetOvenNames();
            string[] fridgeNames = deviceManager.GetFridgeNames();

            ViewBag.microwaveNames = microwaveNames;
            ViewBag.ovenNames = ovenNames;
            ViewBag.fridgeNames = fridgeNames;

            return View();
        }
    }
}