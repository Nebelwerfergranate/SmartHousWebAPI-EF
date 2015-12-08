var images = {
    clock: "/Content/Images/clock.png",
    microwave: "/Content/Images/microwave.png",
    oven: "/Content/Images/oven.png",
    fridge: "/Content/Images/fridge.png",
    // Device-basic
    on: "/Content/Images/on.png",
    off: "/Content/Images/off.png",
    rename: "/Content/Images/rename.png",
    remove: "/Content/Images/remove.png",
    // IBacklight
    backlightOn: "/Content/Images/backlightOn.png",
    backlightOff: "/Content/Images/backlightOff.png",
    // IOpenable
    opened: "/Content/Images/opened.png",
    closed: "/Content/Images/closed.png",
    // ITimer
    start: "/Content/Images/start.png",
    stop: "/Content/Images/stop.png"
}

var types = {
    clock: "SmartHouse.Clock",
    microwave: "SmartHouse.Microwave",
    oven: "SmartHouse.Oven",
    fridge: "SmartHouse.Fridge"
}

var interfaces = {
    iBacklight: "SmartHouse.IBacklight",
    iClock: "SmartHouse.IClock",
    iOpenable: "SmartHouse.IOpenable",
    iTemperature: "SmartHouse.ITemperature",
    iTimer: "SmartHouse.ITimer",
    iVolume: "SmartHouse.IVolume"
}

var allDevicesContainer;

$(document).ready(function () {
    allDevicesContainer = document.getElementById("all-devices-container");
    var testDiv = document.createElement("div");
    $(testDiv).html("<input type='button' value='text' onclick='test()' style='background: red; height: 40px; width: 120px'/>");
    document.body.appendChild(testDiv);

    getAllDevices();
});

function getAllDevices() {
    $.ajax({
        url: "/api/SmartHouse",
        type: "GET",
        success: function(data) {
            if (data != null) {
                $(allDevicesContainer).html("");
                for (var i = 0; i < data.length; i++) {
                    var deviceContainer = document.createElement("div");
                    deviceContainer.classList.add("device-container");

                    var creator = new DeviceCreator(data[i].Id, data[i].Device, data[i].DeviceType, data[i].Interfaces);
                    var deviceBasic = creator.createDeviceBasic();
                    var deviceInterfacesDiv = creator.createDeviceInterfaces();
                    deviceContainer.appendChild(deviceBasic);
                    deviceContainer.appendChild(deviceInterfacesDiv);

                    allDevicesContainer.appendChild(deviceContainer);
                }
            }
        }
    });
}

function addDevice(event) {
    event.preventDefault();
   
    var form = $(event.target).closest("form")[0];
    var device = form.device.value;
    var name = form.name.value;
    var fabricatorList = form.fabricator;
    var fabricator = "";
    if (fabricatorList != undefined) {
        fabricator = fabricatorList.value;
    }
    
    var formInfo = { Device: device, Name: name, Fabricator: fabricator };

    $.ajax({
        url: "/api/SmartHouse",
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(formInfo),
        type: "POST",
        success: function () {

            getAllDevices();
        }
    });
    closeAllDialogs();
}

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
                var deviceInterfacesDiv = creator.createDeviceInterfaces();
                deviceContainer.appendChild(deviceBasic);
                deviceContainer.appendChild(deviceInterfacesDiv);

                allDevicesContainer.appendChild(deviceContainer);
                //alert(data);
            } else {
                alert("Data is null :(");
            }
        }
    });
}

function DeviceCreator(id, device, type, deviceInterfaces) {
    //this.id = id;
    //this.device = device;
    //this.type = type;
    //this.deviceInterfaces = deviceInterfaces;

    this.createDeviceBasic = function () {
        var deviceBasic = document.createElement("div");
        deviceBasic.classList.add("device-basic");

        var deviceControlDiv = document.createElement("div");
        deviceControlDiv.classList.add("device-control");

        var toogleButton = document.createElement("a");
        toogleButton.href = "/Home/ToogleDevice?id=" + id;
        var isOnImage = document.createElement("img");
        if (device.IsOn) {
            isOnImage.src = images.on;
            isOnImage.alt = "Turn off";
        }
        else {
            isOnImage.src = images.off;
            isOnImage.alt = "Turn on";
        }
        isOnImage.classList.add("toogle-button");
        toogleButton.appendChild(isOnImage);

        var renameButton = document.createElement("input");
        renameButton.type = "image";
        renameButton.src = images.rename;
        renameButton.alt = "Rename";
        renameButton.title = "Rename device";
        renameButton.classList.add("rename-button");
        renameButton.onclick = function (id, name) {
            return function () { rename(id, name); }
        }(id, device.Name);

        var removeButton = document.createElement("a");
        removeButton.href = "/Home/RemoveDevice?id=" + id;
        var removeImage = document.createElement("img");
        removeImage.src = images.remove;
        removeImage.alt = "Remove";
        removeImage.classList.add("remove-button");
        removeButton.appendChild(removeImage);

        deviceControlDiv.appendChild(toogleButton);
        deviceControlDiv.appendChild(renameButton);
        deviceControlDiv.appendChild(removeButton);

        var deviceNameDiv = document.createElement("div");
        deviceNameDiv.classList.add('device-name');
        var name = document.createElement("span");
        name.classList.add('device-name-label');
        name.appendChild(document.createTextNode(device.Name));
        deviceNameDiv.appendChild(name);

        var imageDiv = document.createElement("div");
        imageDiv.classList.add("image");
        var deviceImage = document.createElement("img");
        if (type == types.clock) {
            deviceImage.src = images.clock;
            deviceImage.alt = "Clock";
        }
        else if (type == types.microwave) {
            deviceImage.src = images.microwave;
            deviceImage.alt = "Microwave";
        }
        else if (type == types.oven) {
            deviceImage.src = images.oven;
            deviceImage.alt = "Oven";
        }
        else if (type == types.fridge) {
            deviceImage.src = images.fridge;
            deviceImage.alt = "Fridge";
        }
        imageDiv.appendChild(deviceImage);

        deviceBasic.appendChild(deviceControlDiv);
        deviceBasic.appendChild(deviceNameDiv);
        deviceBasic.appendChild(imageDiv);

        return deviceBasic;
    }

    this.createDeviceInterfaces = function () {
        var deviceInterfacesDiv = document.createElement("div");
        deviceInterfacesDiv.classList.add("device-interfaces");

        // IBacklight, IOpenable, IVolume
        var smallInterfacesDiv = createSmallInterfaces();
        deviceInterfacesDiv.appendChild(smallInterfacesDiv);

        // IClock
        if (deviceInterfaces.indexOf(interfaces.iClock) != -1) {
            var iClockDiv = createIClock();
            deviceInterfacesDiv.appendChild(iClockDiv);
        }

        // ITemperature
        if (deviceInterfaces.indexOf(interfaces.iTemperature) != -1) {
            var iTemperatureDiv = createITemperature();
            deviceInterfacesDiv.appendChild(iTemperatureDiv);
        }

        // ITimer
        if (deviceInterfaces.indexOf(interfaces.iTimer) != -1) {
            var iTimerDiv = createITimer();
            deviceInterfacesDiv.appendChild(iTimerDiv);
        }

        // Fridge
        if (type == types.fridge) {
            var fridgeDiv = createFridge();
            deviceInterfacesDiv.appendChild(fridgeDiv);
        }

        return deviceInterfacesDiv;
    }

    var createSmallInterfaces = function () {
        var deviceInterfacesSmall = document.createElement("div");
        deviceInterfacesSmall.classList.add("device-interfaces-small");

        var divTop = document.createElement("div");
        if (deviceInterfaces.indexOf(interfaces.iOpenable) != -1) {
            var iOpenableDiv = createIOpenable();
            divTop.appendChild(iOpenableDiv);
        }
        if (deviceInterfaces.indexOf(interfaces.iBacklight) != -1) {
            var iBacklightDiv = createIBacklight();
            divTop.appendChild(iBacklightDiv);
        }
        deviceInterfacesSmall.appendChild(divTop);

        if (deviceInterfaces.indexOf(interfaces.iVolume) != -1) {
            var iVolumeDiv = createIVolume();
            deviceInterfacesSmall.appendChild(iVolumeDiv);
        }

        return deviceInterfacesSmall;
    }

    // IBacklight
    var createIBacklight = function () {
        var image = document.createElement("img");
        if (device.IsHighlighted) {
            image.src = images.backlightOn;
            image.alt = "Backlight is on";
        } else {
            image.src = images.backlightOff;
            image.alt = "Backlight is off";
        }
        image.title = "Lamp power: " + device.LampPower + " W";

        return image;
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
        var image = document.createElement("img");
        if (device.IsOpen) {
            image.src = images.opened;
            image.alt = "Close";
        } else {
            image.src = images.closed;
            image.alt = "Open";
        }

        var link = document.createElement("a");
        link.href = "/Home/ToogleDoor?id=" + id;
        link.appendChild(image);

        return link;
    }

    // ITemperature
    var createITemperature = function () {
        var iTemperatureDiv = document.createElement("div");
        iTemperatureDiv.classList.add("itemperature");

        var form = document.createElement("form");
        form.action = "/Home/SetTemperature?id=" + id;
        form.method = "POST";

        var temperature = document.createElement("input");
        temperature.type = "number";
        temperature.maxLength = 5;
        temperature.min = device.MinTemperature;
        temperature.max = device.MaxTemperature;
        temperature.value = device.Temperature;
        temperature.name = "temperature";
        temperature.required = "required";
        temperature.title = "Set value between " + device.MinTemperature + " and " + device.MaxTemperature;

        var submit = document.createElement("input");
        submit.type = "submit";
        submit.value = "Set Temperature";

        form.appendChild(temperature);
        form.appendChild(submit);

        iTemperatureDiv.appendChild(form);
        return iTemperatureDiv;
    }

    // ITimer
    var createITimer = function () {
        var iTimerDiv = document.createElement("div");

        if (device.IsOn) {
            iTimerDiv.classList.add("itimer");

            var form = document.createElement("form");
            form.action = "/Home/TimerSet?id=" + id;
            form.method = "POST";

            if (type != types.microwave) {
                var hours = document.createElement("input");
                hours.type = "number";
                hours.maxLength = 2;
                hours.min = 0;
                hours.max = 23;
                hours.name = "hours";

                var hmSeparator = document.createElement("span");
                hmSeparator.classList.add("unselectable");
                hmSeparator.appendChild(document.createTextNode(" : "));

                form.appendChild(hours);
                form.appendChild(hmSeparator);
            }
            var minutes = document.createElement("input");
            minutes.type = "number";
            minutes.min = 0;
            minutes.max = 59;
            minutes.name = "minutes";
            minutes.maxLength = 2;

            var msSeparator = document.createElement("span");
            msSeparator.classList.add("unselectable");
            msSeparator.appendChild(document.createTextNode(" : "));

            var seconds = document.createElement("input");
            seconds.type = "number";
            seconds.min = 0;
            seconds.max = 59;
            seconds.name = "seconds";
            seconds.maxLength = 2;

            var submit = document.createElement("input");
            submit.type = "submit";
            submit.value = "Set Timer";

            form.appendChild(minutes);
            form.appendChild(msSeparator);
            form.appendChild(seconds);
            form.appendChild(submit);

            iTimerDiv.appendChild(form);

            var link = document.createElement("a");
            link.href = "/Home/ToogleTimer?id=" + id;

            var image = document.createElement("img");
            if (device.IsRunning) {
                image.src = images.stop;
                image.alt = "Running";
            } else {
                image.src = images.start;
                image.alt = "Not running";
                image.title = "Don't forget to set the timer";
            }

            link.appendChild(image);

            iTimerDiv.appendChild(link);
        }

        return iTimerDiv;
    }

    // IVolume
    var createIVolume = function () {
        var iVolumeDiv = document.createElement("div");
        iVolumeDiv.classList.add("ivolume");

        var span = document.createElement("span");
        span.appendChild(document.createTextNode("Interior space: " + device.Volume + " litres"));
        iVolumeDiv.appendChild(span);

        return iVolumeDiv;
    }

    // Fridge
    var createFridge = function () {
        var fridgeDiv = document.createElement("div");
        fridgeDiv.classList.add("fridge");

        // Coldstore
        var iopenable = {};
        iopenable.command = "/Home/ToogleColdstoreDoor?id=" + id;
        if (device.ColdstoreIsOpen) {
            iopenable.imgSrc = images.opened;
            iopenable.imgAlt = "Close";
        } else {
            iopenable.imgSrc = images.closed;
            iopenable.imgAlt = "Open";
        }

        var ibacklight = {};
        if (device.ColdstoreIsHighlighted) {
            ibacklight.imgSrc = images.backlightOn;
            ibacklight.imgAlt = "Backlight is on";
        } else {
            ibacklight.imgSrc = images.backlightOff;
            ibacklight.imgAlt = "Backlight is off";
        }
        ibacklight.lampPower = device.ColdstoreLampPower;
        
        var itemperature = {};
        itemperature.formAction = "/Home/SetColdstoreTemperature?id=" + id;
        itemperature.min = device.ColdstoreMinTemperature;
        itemperature.max = device.ColdstoreMaxTemperature;
        itemperature.value = device.ColdstoreTemperature;

        var coldstore = createFridgeModule({
            "className": "coldstore",
            "iopenable": iopenable,
            "ibacklight": ibacklight,
            "ivolume": { "volume": device.ColdstoreVolume },
            "itemperature": itemperature
        });

        // Freezer
        iopenable = {};
        iopenable.command = "/Home/ToogleFreezerDoor?id=" + id;
        if (device.FreezerIsOpen) {
            iopenable.imgSrc = images.opened;
            iopenable.imgAlt = "Close";
        } else {
            iopenable.imgSrc = images.closed;
            iopenable.imgAlt = "Open";
        }

        itemperature = {};
        itemperature.formAction = "/Home/SetFreezerTemperature?id=" + id;
        itemperature.min = device.FreezerMinTemperature;
        itemperature.max = device.FreezerMaxTemperature;
        itemperature.value = device.FreezerTemperature;

        var freezer = createFridgeModule({
            "className": "freezer",
            "iopenable": iopenable,
            "ivolume": { "volume": device.FreezerVolume },
            "itemperature": itemperature
        });

        fridgeDiv.appendChild(coldstore);
        fridgeDiv.appendChild(freezer);
        return fridgeDiv;
    }
    
    var createFridgeModule = function (params) {
        var className = params.className;
        var iopenable = params.iopenable;
        var ibacklight = params.ibacklight;
        var ivolume = params.ivolume;
        var itemperature = params.itemperature;

        var fridgeModule = document.createElement("div");
        fridgeModule.classList.add(className);

        var smallInterfaces = document.createElement("div");
        smallInterfaces.classList.add("device-interfaces-small");

        var divTop = document.createElement("div");

        if (iopenable != undefined) {
            var link = document.createElement("a");
            link.href = iopenable.command;

            var iOpenableImg = document.createElement("img");
            iOpenableImg.src = iopenable.imgSrc;
            iOpenableImg.alt = iopenable.imgAlt;

            link.appendChild(iOpenableImg);
            divTop.appendChild(link);
        }
        smallInterfaces.appendChild(divTop);

        if (ibacklight != undefined) {
            var iBacklightImg = document.createElement("img");
            iBacklightImg.src = ibacklight.imgSrc;
            iBacklightImg.alt = ibacklight.imgAlt;
            iBacklightImg.title = "Lamp power: " + ibacklight.lampPower + " W";

            divTop.appendChild(iBacklightImg);
        }
        
        if (ivolume != undefined) {
            var iVolumeDiv = document.createElement("div");
            iVolumeDiv.classList.add("ivolume");

            var span = document.createElement("span");
            span.appendChild(document.createTextNode("Interior space: " + ivolume.volume + " litres"));

            iVolumeDiv.appendChild(span);
            smallInterfaces.appendChild(iVolumeDiv);
        }

        fridgeModule.appendChild(smallInterfaces);

        if (itemperature != undefined) {
            var itemperatureDiv = document.createElement("div");
            itemperatureDiv.classList.add("itemperature");

            var form = document.createElement("form");
            form.action = itemperature.formAction;
            form.method = "POST";

            var temperature = document.createElement("input");
            temperature.type = "number";
            temperature.maxLength = 5;
            temperature.min = itemperature.min;
            temperature.max = itemperature.max;
            temperature.value = itemperature.value;
            temperature.name = "temperature";
            temperature.required = "required";
            temperature.title = "Set value between " + itemperature.min + " and " + itemperature.max;

            var submit = document.createElement("input");
            submit.type = "submit";
            submit.value = "Set Temperature";

            form.appendChild(temperature);
            form.appendChild(submit);

            itemperatureDiv.appendChild(form);
            fridgeModule.appendChild(itemperatureDiv);
        }

        return fridgeModule;
    }
}
