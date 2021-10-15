$("#ButtonNuevo").click(function (eve) {
    $("#modal-content").load("/Pacientes/Create");
});

$("#EditarButton").click(function (eve) {
    $("#modal-content").load("/Pacientes/Edit/" + $(this).data("id"));
});

$("#DetallesButton").click(function (eve) {
    $("#modal-content").load("/Pacientes/Details/" + $(this).data("id"));
});

$("#EliminarButton").click(function (eve) {
    $("#modal-content").load("/Pacientes/Delete/" + $(this).data("id"));
});