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
                var deviceInterfaces = creator.createDeviceInterfaces();
                deviceContainer.appendChild(deviceBasic);
                deviceContainer.appendChild(deviceInterfaces);

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
        if (device.IsOn == true) {
            imageSrc = "src='/Content/Images/on.png'";
        }
        else {
            imageSrc = "src='/Content/Images/off.png'";
        }

        var deviceImgSrc = "";
        if (type == "SmartHouse.Clock") {
            deviceImgSrc = "src='/Content/Images/clock.png'";
        }
        else if (type == "SmartHouse.Microwave") {
            deviceImgSrc = "src='/Content/Images/microwave.png'";
        }
        else if (type == "SmartHouse.Oven") {
            deviceImgSrc = "src='/Content/Images/oven.png'";
        }
        else if (type == "SmartHouse.Fridge") {
            deviceImgSrc = "src='/Content/Images/fridge.png'";
        }
        $(deviceBasic).html(
            "<div class='device-control'>" +
                "<a href='/Home/ToogleDevice?id=" + id + "'>" +
                     "<img " + imageSrc + " alt='Toggle' class='toogle-button' />" +
                "</a>" +
                "<a href='/Home/RemoveDevice?id=" + id + "'>" +
                    "<img src='/Content/Images/remove.png' alt='Remove' class='remove-button' />" +
                "</a>" +
                "<input type='image' src='/Content/Images/rename.png' alt='Rename'" +
                    "class='rename-button' title='Rename device' />" +
            "</div>" +
            "<div class='device-name'>" +
                "<span class='device-name-label'>" + device.Name + "</span>" +
            "</div>" +
            "<div class='image'>" +
                "<img " + deviceImgSrc + " alt='clock' />" +
            "<div>"
        );

        deviceBasic.getElementsByClassName("rename-button")[0].onclick = function (id, name) {
            return function () { rename(id, name); }
        }(id, device.Name);

        return deviceBasic;
    }

    this.createDeviceInterfaces = function () {
        var deviceInterfaces = document.createElement("div");
        deviceInterfaces.classList.add("device-interfaces");

        deviceInterfaces.appendChild(createSmallInterfaces());

        if (interfaces.indexOf("SmartHouse.IClock") != -1) {
            var iClockDiv = createIClock();
            deviceInterfaces.appendChild(iClockDiv);
        }

        return deviceInterfaces;
    }

    var createSmallInterfaces = function () {
        var deviceInterfacesSmall = document.createElement("div");
        deviceInterfacesSmall.classList.add("device-interfaces-small");

        var divTop = document.createElement("div");
        var smallInterfaces = "";
        if (interfaces.indexOf("SmartHouse.IOpenable") != -1) {
            smallInterfaces += createIOpenable();
        }
        if (interfaces.indexOf("SmartHouse.IBacklight") != -1) {
            smallInterfaces += createIBacklight();
        }
        $(divTop).html(smallInterfaces);
        deviceInterfacesSmall.appendChild(divTop);

        if (interfaces.indexOf("SmartHouse.IVolume") != -1) {
            var iVolumeDiv = createIVolume();
            deviceInterfacesSmall.appendChild(iVolumeDiv);
        }

        return deviceInterfacesSmall;
    }

    // IBacklight
    var createIBacklight = function () {
        var imageSrc = "";
        if (device.IsHighlighted == true) {
            imageSrc = " src='/Content/Images/backlightOn.png' alt='Backlight is on'";
        } else {
            imageSrc = " src='/Content/Images/backlightOff.png' alt='Backlight is off'";
        }
        var iBacklight = "<img " + imageSrc + " title='Lamp power: " + device.LampPower + " W' />";
        return iBacklight;
    }

    // IClock
    var createIClock = function () {
        var iClockDiv = document.createElement("div");
        iClockDiv.classList.add("iclock");

        // Такой формат Date() понимает
        // 2015-12-06T00:56:12.0666202+02:00 
        var time = new Date(device.CurrentTime);

        var dynamicClockDiv = document.createElement("div");

        if (device.IsOn == true) {
            $(dynamicClockDiv).myClock({ "dateTime": time });
            iClockDiv.appendChild(dynamicClockDiv);

            var form = document.createElement("form");
            form.action = "/Home/SetTime?id=" + id;
            form.method = "Post";

            var hours = document.createElement("input");
            hours.type = "number";
            hours.min = 0;
            hours.max = 23;
            hours.name = "hours";
            hours.required = "required";
            hours.maxLength = 2;

            var separator = document.createElement("span");
            separator.classList.add("unselectable");
            separator.appendChild(document.createTextNode(" : "));

            var minutes = document.createElement("input");
            minutes.type = "number";
            minutes.min = 0;
            minutes.max = 59;
            minutes.name = "minutes";
            minutes.required = "required";
            minutes.maxLength = 2;

            var submit = document.createElement("input");
            submit.type = "submit";
            submit.value = "Set Time";

            form.appendChild(hours);
            form.appendChild(separator);
            form.appendChild(minutes);
            form.appendChild(submit);

            iClockDiv.appendChild(form);
        } else {
            $(dynamicClockDiv).myClock({ "disabled": true });
            iClockDiv.appendChild(dynamicClockDiv);
        }

        return iClockDiv;
    }

    // IOpenable
    var createIOpenable = function () {
        var imageSrc = "";
        if (device.IsOpen == true) {
            imageSrc = "src='/Content/Images/opened.png' alt='Close'";
        } else {
            imageSrc = "src='/Content/Images/closed.png' alt='Open'";
        }
        var iOpenable = "<a href='/Home/ToogleDoor?id=" + id + "'>" +
                            "<img " + imageSrc + " />" +
                        "</a>";
        return iOpenable;
    }

    // ITemperature


    // ITimer


    // IVolume
    var createIVolume = function () {
        var iVolumeDiv = document.createElement("div");
        iVolumeDiv.classList.add("ivolume");
        $(iVolumeDiv).html(
            "<span>" +
                "Interior space: " + device.Volume + " litres" +
            "</span>"
        );
        return iVolumeDiv;
    }
}

//        <!-- device-device-interfaces -->
//        <div class="device-interfaces">
//            <div class="device-interfaces-small">

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