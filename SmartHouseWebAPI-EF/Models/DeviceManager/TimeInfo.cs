using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartHouseWebAPI_EF.Models.DeviceManager
{
    public class TimeInfo
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }


        public bool IsValid
        {
            get
            {
                bool isValid = false;
                if (Hours >= 0 && Hours < 24 && Minutes >= 0 && Minutes < 60 && Seconds >= 0 && Seconds < 60)
                {
                    isValid = true;
                }
                return isValid;
            }
        }
    }
}