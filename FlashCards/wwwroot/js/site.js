const { data } = require("jquery");

function addEntity(controllerName) {
    $.ajax({
        url: "/" + controllerName + "/Create",
        type: "Get",
        processData: false,
        contentType: false,
        data: null,
        async: true,
        error: function (jqxhr, textStatus, erroThrown) {
            console.error(Error, jqxhr);
            $('.modal').remove();
            $('.modal-backdrop').remove();
            bootbox.alert(jqxhr.responseText);
        }
    }).done((data) => {
        $('#modalX').empty();
        $('#modalX').append(data);
        $('.modal').modal('show');
    });
}