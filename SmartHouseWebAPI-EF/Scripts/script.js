// Данные Css id и классы используются для работы скриптов.
// Их переименование обязательно должно быть синхронизировано с серверным кодом
//
// #clock-dialog
// #microwave-dialog
// #oven-dialog
// #fridge-dialog
// #rename-dialog
// #renameId
// #newName
//
// .js_IClockDiv
// .js_DynamicClockDiv
// .js_Timestamp

function closeAllDialogs() {
    $("#clock-dialog").dialog('close');
    $("#microwave-dialog").dialog('close');
    $("#oven-dialog").dialog('close');
    $("#fridge-dialog").dialog('close');
    $("#rename-dialog").dialog('close');
}

function openDialog(id) {
    closeAllDialogs();
    $("#" + id).dialog('open');
}

function rename(deviceId, oldName) {
    if (oldName == undefined) {
        oldName = "";
    }
    closeAllDialogs();
    document.getElementById("renameId").value = deviceId;
    document.getElementById("newName").value = oldName;
    $("#rename-dialog").dialog('open');
    document.getElementById("newName").select();
}

$(document).ready(function () {
    window.scroll($.cookie("scrollLeft"), $.cookie("scrollTop"));

    // IClock
    $(".js_IClockDiv").each(function (index, value) {
        var clockElement = $(value).find(".js_DynamicClockDiv:first");
        var timestamp = $(value).find(".js_Timestamp:first").val();
        var disabled = false;
        if (timestamp == "disabled") {
            disabled = true;
        }
        else {
            timestamp = parseInt(timestamp);
        }
        $(clockElement).myClock({ "timestamp": timestamp, "disabled": disabled });
    });

    $("#clock-dialog").dialog({ autoOpen: false, title: "Add new clock" });
    $("#microwave-dialog").dialog({ autoOpen: false, title: "Add new microwave" });
    $("#oven-dialog").dialog({ autoOpen: false, title: "Add new oven" });
    $("#fridge-dialog").dialog({ autoOpen: false, title: "Add new fridge" });

    $("#rename-dialog").dialog({ autoOpen: false, title: "Rename device" });
    
    $(document).scroll(function () {
        $.cookie("scrollTop", $(document).scrollTop());
        $.cookie("scrollLeft", $(document).scrollLeft());
    });
});