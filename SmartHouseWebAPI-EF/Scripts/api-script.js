var allDevicesContainer;

$(document).ready(function () {
    allDevicesContainer = document.getElementById("all-devices-container");
    var testDiv = document.createElement("div");
    $(testDiv).html("<input type='button' value='text' onclick='test()' style='background: red; height: 40px; width: 120px'/>");
    document.body.appendChild(testDiv);
});


function test() {
    var id = prompt("Enter device id", "");
    if (id == null) {
        return;
    }
    $.ajax({
        url: "/api/SmartHouse/" + id,
        type: "GET",
        success: function (data) {
            if (data != null) {
                var deviceContainer = document.createElement("div");
                deviceContainer.classList.add("device-container");

                var creator = new DeviceCreator(data.Id, data.Device, data.DeviceType, data.Interfaces);
                var deviceBasic = creator.createDeviceBasic();
                var deviceInterfacec = creator.createDeviceInterfaces();
                deviceContainer.appendChild(deviceBasic);

                allDevicesContainer.appendChild(deviceContainer);
                //alert(data);
            } else {
                alert("Data is null :(");
            }
        }
    });
}

function DeviceCreator(id, device, type, interfaces) {
    this.id = id;
    this.device = device;
    this.type = type;
    this.interfaces = interfaces;
    this.createDeviceBasic = function () {
        var deviceBasic = document.createElement("div");
        deviceBasic.classList.add("device-basic");

        var imageSrc = "";
        if (this.device.IsOn == true) {
            imageSrc = "src='/Content/Images/on.png'";
        }
        else {
            imageSrc = "src='/Content/Images/off.png'";
        }

        var deviceImgSrc = "";
        if (this.type == "SmartHouse.Clock") {
            deviceImgSrc = "src='/Content/Images/clock.png'";
        }
        else if (this.type == "SmartHouse.Microwave") {
            deviceImgSrc = "src='/Content/Images/microwave.png'";
        }
        else if (this.type == "SmartHouse.Oven") {
            deviceImgSrc = "src='/Content/Images/oven.png'";
        }
        else if (this.type == "SmartHouse.Fridge") {
            deviceImgSrc = "src='/Content/Images/fridge.png'";
        }
        $(deviceBasic).html(
            "<div class='device-control'>" +
                "<a href='/Home/ToogleDevice?id=" + this.id + "'>" +
                     "<img " + imageSrc + " alt='Toggle' class='toogle-button' />" +
                "</a>" +
                "<a href='/Home/RemoveDevice?id=" + this.id + "'>" +
                    "<img src='/Content/Images/remove.png' alt='Remove' class='remove-button' />" +
                "</a>" +
                "<input type='image' src='/Content/Images/rename.png' alt='Rename'" +
                    "class='rename-button' title='Rename device' />" +
            "</div>" +
            "<div class='device-name'>" +
                "<span class='device-name-label'>" + this.device.Name + "</span>" +
            "</div>" +
            "<div class='image'>" +
                "<img " + deviceImgSrc + " alt='clock' />" +
            "<div>"
        );

        deviceBasic.getElementsByClassName("rename-button")[0].onclick = function (id, name) {
            return function () { rename(id, name); }
        }(this.id, this.device.Name);

        return deviceBasic;
    }
    this.createDeviceInterfaces = function () {
        var deviceInterfaces = document.createElement("div");
        deviceInterfaces.classList.add("device-interfaces");

        if (this.interfaces.indexOf("SmartHouse.IClock") != -1) {
            alert("device is IClock!!!");
        }
    }
}

//        <!-- device-device-interfaces -->
//        <div class="device-interfaces">
//            <div class="device-interfaces-small">
//                <div>
//                    @if (device is IOpenable)
//{
//                        IOpenable door = (IOpenable)device;
//<!-- IOpenable -->
//<a href="/Home/ToogleDoor?id=@deviceId">
//    @if (door.IsOpen)
//{
//    <img src="~/Content/Images/opened.png" alt="Close" />
//    }
//else
//{
//<img src="~/Content/Images/closed.png" alt="Open" />
//}
//</a>
//}
//                    @if (device is IBacklight)
//{
//    IBacklight ibacklightObj = (IBacklight)device;
//    <!-- IBacklight -->
//    if (ibacklightObj.IsHighlighted)
//    {
//        <img src="~/Content/Images/backlightOn.png" alt="Backlight is on"
//        title="Lamp power: @ibacklightObj.LampPower W" />
//        }
//else
//{
//    <img src="~/Content/Images/backlightOff.png" alt="Backlight is off"
//    title="Lamp power: @ibacklightObj.LampPower W" />
//    }
//}
//                </div>
//                @if (device is IVolume)
//{
//    IVolume ivolumeObj = (IVolume)device;
//    <!-- IVolume -->
//    <div class="ivolume">
//        <span>Interior space: @ivolumeObj.Volume litres</span>
//    </div>
//}
//</div>
//            @if (device is IClock)
//{
//    <!-- IClock -->
//    <div class="js_IClockDiv iclock">
//        @if (device.IsOn)
//    {
//        DateTime curTime = ((IClock)device).CurrentTime;
//        // Convert current time to miliseconds
//        string hiddenValue = (curTime.Hour * 60 * 60 * 1000 +
//                           curTime.Minute * 60 * 1000 +
//                           curTime.Second * 1000).ToString();
//        <input type="hidden" class="js_Timestamp" value="@hiddenValue" />
//        }
//else
//{
//            string hiddenValue = "disabled";
//    <input type="hidden" class="js_Timestamp" value="@hiddenValue" />
//    }

//<div class="js_DynamicClockDiv"></div>

//                    @if (device.IsOn)
//                    {
//                        using (Html.BeginForm("SetTime", "Home", new { id = deviceId }, FormMethod.Post))
//                        {
//                            <input type="number" min="0" max="23"
//                            name="hours" required="required" maxlength="2" />
//                     <span class="unselectable">:</span>
//                     <input type="number" min="0" max="59"
//                            name="minutes" required="required" maxlength="2" />
//                     <input type="submit" value="Set Time" />
//                     }
//                    }
//</div>
//}
//            @if (device is ITemperature)
//{
//    <!-- ITemperature -->
//    ITemperature itemperatureObj = (ITemperature)device;
//    string title = "Set value between " + itemperatureObj.MinTemperature + " and " + itemperatureObj.MaxTemperature;

//    <div class="itemperature">
//        @using (Html.BeginForm("SetTemperature", "Home", new { id = deviceId }, FormMethod.Post))
//    {
//        <input type="number" maxlength="5" min="@itemperatureObj.MinTemperature"
//        max="@itemperatureObj.MaxTemperature"
//        value="@itemperatureObj.Temperature" name="temperature" required="required"
//        title="@title" />
// <input type="submit" value="Set Temperature" />
// }
//    </div>
//}
//            @if (device is ITimer)
//{
//    if (device.IsOn)
//    {
//        <!-- ITimer -->
//          <div class="itimer">
//              @using (Html.BeginForm("TimerSet", "Home", new { id = deviceId }, FormMethod.Post))
//        {
//            if (device is Microwave == false)
//            {
//                <input type="number" maxlength="2" min="0" max="23"
//                name="hours" />
//         <span class="unselectable">:</span>
//            }
//            <input type="number" maxlength="2" min="0" max="59"
//            name="minutes" />
//     <span class="unselectable">:</span>
//     <input type="number" maxlength="2" min="0" max="59"
//            name="seconds" />
//     <input type="submit" value="Set Timer" />
//     <a href="/Home/ToogleTimer?id=@deviceId">
//         @if (((ITimer)device).IsRunning)
//            {
//                <img src="~/Content/Images/stop.png" alt="Running" />
//                }
//        else
//        {
//            <img src="~/Content/Images/start.png" alt="Running" title="Don't forget to set the timer" />
//            }
//        </a>
//    }
//    </div>
//}
//}
//            @if (device is Fridge)
//{
//    Fridge fridge = (Fridge)device;
//    <!-- Fridge -->
//    <div class="fridge">
//        <!-- Coldstore -->
//        <div>
//            <div class="device-interfaces-small">
//                <div>
//                    <!-- Open/CLose -->
//                    <a href="/Home/ToogleColdstoreDoor?id=@deviceId">
//                        @if (fridge.ColdstoreIsOpen)
//    {
//        <img src="~/Content/Images/opened.png" alt="Close" />
//        }
//else
//{
//    <img src="~/Content/Images/closed.png" alt="Open" />
//    }
//</a>
//<!-- Backlight -->
//@if (fridge.ColdstoreIsHighlighted)
//{
//    <img src="~/Content/Images/backlightOn.png" alt="Backlight is on"
//    title="Lamp power: @fridge.ColdstoreLampPower W" />
//    }
//else
//{
//<img src="~/Content/Images/backlightOff.png" alt="Backlight is off"
//title="Lamp power: @fridge.ColdstoreLampPower W" />
//}
//</div>
//<div class="ivolume">
//    <!-- Volume -->
//    <span>Interior space: @fridge.ColdstoreVolume litres</span>
//</div>
//</div>
//<!-- Temperature -->
//<div class="itemperature">
//    @using (Html.BeginForm("SetColdstoreTemperature", "Home", new { id = deviceId }, FormMethod.Post))
//{
//    string title = "Set value between " + fridge.ColdstoreMinTemperature +
//        " and " + fridge.ColdstoreMaxTemperature;
//    <input type="number" maxlength="5" min="@fridge.ColdstoreMinTemperature"
//    max="@fridge.ColdstoreMaxTemperature"
//    value="@fridge.ColdstoreTemperature" name="temperature" required="required"
//    title="@title" />
//<input type="submit" value="Set Temperature" />
//}
//</div>
//</div>
//<!-- Freezer -->
//<div>
//<div class="device-interfaces-small">
//    <div>
//        <!-- Open/CLose -->
//        <a href="/Home/ToogleFreezerDoor/?id=@deviceId">
//            @if (fridge.FreezerIsOpen)
//{
//    <img src="~/Content/Images/opened.png" alt="Close" />
//    }
//else
//{
//<img src="~/Content/Images/closed.png" alt="Open" />
//}
//</a>
//</div>
//<div class="ivolume">
//    <!-- Volume -->
//    <span>Interior space: @fridge.FreezerVolume litres</span>
//</div>
//</div>
//<!-- Temperature -->
//<div class="itemperature">
//    @using (Html.BeginForm("SetFreezerTemperature", "Home", new { id = deviceId }, FormMethod.Post))
//{
//    string title = "Set value between " + fridge.FreezerMinTemperature +
//        " and " + fridge.FreezerMaxTemperature;
//    <input type="number" maxlength="5" min="@fridge.FreezerMinTemperature"
//    max="@fridge.FreezerMaxTemperature"
//    value="@fridge.FreezerTemperature" name="temperature" required="required"
//    title="@title" />
//<input type="submit" value="Set Temperature" />
//}
//</div>
//</div>
//</div>
//}
//        </div>
//    </div>