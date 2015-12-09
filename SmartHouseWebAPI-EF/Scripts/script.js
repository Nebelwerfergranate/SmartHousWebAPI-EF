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
    $("#" + id).dialog("open");
    $("#" + id).find("input[type='text']")[0].value = "";
}

function rename(id, oldName) {
    if (oldName == undefined) {
        oldName = "";
    }
    closeAllDialogs();
    var newNameField = document.getElementById("newName");
    newNameField.value = oldName;
    $("#rename-dialog").dialog('open');
    newNameField.select();

    var renameButton = document.getElementById("rename-button");
    renameButton.onclick = function (event) {
        event.preventDefault();
        $.ajax({
            url: "/api/SmartHouse/RenameDevice/" + id,
            data: { "": document.getElementById("newName").value },
            type: "PUT",
            success: function () {
                var device = document.getElementById("device-" + id);
                var span = device.getElementsByClassName("device-name-label")[0];
                $(span).text(newNameField.value);
                closeAllDialogs();
            }
        });
    }
}

$(document).ready(function () {
    window.scroll($.cookie("scrollLeft"), $.cookie("scrollTop"));

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