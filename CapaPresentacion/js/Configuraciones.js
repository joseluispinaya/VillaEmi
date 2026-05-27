
let tablaData;
let idEditar = 0;

$(document).ready(function () {

    listaCarreras();
});

function listaCarreras() {
    if ($.fn.DataTable.isDataTable("#tbDatas")) {
        $("#tbDatas").DataTable().destroy();
        $('#tbDatas tbody').empty();
    }

    tablaData = $("#tbDatas").DataTable({
        responsive: true,
        "ajax": {
            "url": 'Configuraciones.aspx/ListaCarreras',
            "type": "POST",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": function (d) {
                return JSON.stringify(d);
            },
            "dataSrc": function (json) {
                if (json.d.Estado) {
                    return json.d.Data;
                } else {
                    return [];
                }
            }
        },
        "columns": [
            { "data": "IdCarrera", "visible": false, "searchable": false },
            { "data": "Nombre", "className": "align-middle" },
            {
                "data": "Estado", "className": "text-center align-middle", "render": function (data) {
                    if (data === true)
                        return '<span class="badge badge-primary">Activo</span>';
                    else
                        return '<span class="badge badge-danger">No Activo</span>';
                }
            },
            {
                "defaultContent": '<button class="btn btn-primary btn-editar btn-sm mr-2"><i class="fas fa-pencil-alt"></i></button>' +
                    '<button class="btn btn-info btn-detalle btn-sm"><i class="fas fa-address-book"></i></button>',
                "orderable": false,
                "searchable": false,
                "className": "text-center align-middle"
            }
        ],
        "order": [],
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        }
    });
}

$('#tbDatas tbody').on('click', '.btn-editar', function () {

    let fila = $(this).closest('tr');

    if (fila.hasClass('child')) {
        fila = fila.prev();
    }

    let data = tablaData.row(fila).data();
    idEditar = data.IdCarrera;
    $("#txtCarrera").val(data.Nombre);
    $("#cboEstado").val(data.Estado ? 1 : 0).prop("disabled", false);
    // Apuntamos al span, dejando el ícono intacto
    //$("#txtTituloModal").text("Editar Carrera");

    // Inyectas el HTML completo (ícono + texto)
    $("#myModalLabel").html('<i class="fa fa-edit mr-2"></i>Editar Carrera');
    $("#mdData").modal("show");

});

$('#tbDatas tbody').on('click', '.btn-detalle', function () {

    let fila = $(this).closest('tr');

    if (fila.hasClass('child')) {
        fila = fila.prev();
    }

    let data = tablaData.row(fila).data();
    const textoSms = `Detalle de: ${data.Nombre}.`;
    MostrarAlerta("¡Mensaje!", textoSms, "info");

});

$("#btnNuevoReg").on("click", function () {

    idEditar = 0;
    $("#txtCarrera").val("");
    $("#cboEstado").val(1).prop("disabled", true);
    // Apuntamos al span, dejando el ícono intacto
    //$("#txtTituloModal").text("Nuevo Registro");

    // Inyectas el HTML completo (ícono + texto)
    $("#myModalLabel").html('<i class="fa fa-book mr-2"></i>Nuevo Registro');

    $("#mdData").modal("show");
})

function habilitarBoton() {
    $('#btnGuardarCambios').prop('disabled', false);
}

$("#btnGuardarCambios").on("click", function () {
    // Bloqueo inmediato
    $('#btnGuardarCambios').prop('disabled', true);

    if ($("#txtCarrera").val().trim() === "") {
        MostrarToastZer("Debe Agregar un Nombre de Carrera.", "Atención", "warning");
        $("#txtCarrera").focus();
        habilitarBoton();
        return;
    }

    const objeto = {
        IdCarrera: idEditar,
        Nombre: $("#txtCarrera").val().trim(),
        Estado: ($("#cboEstado").val() === "1" ? true : false)
    }

    $("#mdData").find("div.modal-content").LoadingOverlay("show");

    $.ajax({
        type: "POST",
        url: "Configuraciones.aspx/GuardarOrEditCarreras",
        data: JSON.stringify({ objeto: objeto }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#mdData").find("div.modal-content").LoadingOverlay("hide");
            MostrarAlerta(
                response.d.Estado ? '¡Excelente!' : 'Atención', // Título dinámico
                response.d.Mensaje, // Texto del servidor
                response.d.Valor // Icono (success/error/warning)
            );

            if (response.d.Estado) {
                $("#mdData").modal("hide");
                listaCarreras();
                idEditar = 0;
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText);
            $("#mdData").find("div.modal-content").LoadingOverlay("hide");
            toastr.error("No se pudo conectar con el servidor.");
        },
        complete: function () {
            habilitarBoton();
        }
    });

})

// fin