using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SmartHouseMVC.Models.Contexts
{
    public class DeviceContextInitializer : DropCreateDatabaseIfModelChanges<DeviceContext>
    {
    }
}