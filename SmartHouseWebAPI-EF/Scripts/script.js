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

$(document).ready(function () {
    $("#clock-dialog").dialog({ autoOpen: false, title: "Add new clock" });
    $("#microwave-dialog").dialog({ autoOpen: false, title: "Add new microwave" });
    $("#oven-dialog").dialog({ autoOpen: false, title: "Add new oven" });
    $("#fridge-dialog").dialog({ autoOpen: false, title: "Add new fridge" });

    $("#rename-dialog").dialog({ autoOpen: false, title: "Rename device" });
});