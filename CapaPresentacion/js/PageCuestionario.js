
let tablaData;
let tablaDataPregunta;
let idEditar = 0;

$(document).ready(function () {

    listaCuestionarios();
});

function listaCuestionarios() {
    if ($.fn.DataTable.isDataTable("#tbDatas")) {
        $("#tbDatas").DataTable().destroy();
        $('#tbDatas tbody').empty();
    }

    tablaData = $("#tbDatas").DataTable({
        responsive: true,
        "ajax": {
            "url": 'PageCuestionario.aspx/ListaCuestionarios',
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
            { "data": "IdCuestionario", "visible": false, "searchable": false },
            { "data": "Titulo", "className": "align-middle" },
            { "data": "Descripcion", "className": "align-middle" },
            { "data": "CantiPreg", "className": "align-middle" },
            {
                "defaultContent": '<button class="btn btn-primary btn-editar btn-sm mr-2"><i class="fas fa-pencil-alt"></i></button>' +
                    '<button class="btn btn-info btn-detalle btn-sm"><i class="fas fa-address-book"></i></button>',
                "orderable": false,
                "searchable": false,
                "width": "90px",
                "className": "text-center align-middle"
            }
        ],
        "order": [[0, "desc"]],
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
    idEditar = data.IdCuestionario;
    $("#txtTitulo").val(data.Titulo);
    $("#txtDescripcion").val(data.Descripcion);
    $("#myModalLabel").text("Editar Cuestionario");
    $("#mdData").modal("show");

});

$('#tbDatas tbody').on('click', '.btn-detalle', function () {

    let fila = $(this).closest('tr');

    if (fila.hasClass('child')) {
        fila = fila.prev();
    }

    let data = tablaData.row(fila).data();
    const textoSms = `Detalle de: ${data.Titulo}.`;
    swal("Mensaje", textoSms, "info")

});

$("#btnRegistro").on("click", function () {

    idEditar = 0;
    $("#txtTitulo").val("");
    $("#txtDescripcion").val("");
    $("#myModalLabel").text("Nuevo Registro");

    $("#mdData").modal("show");
})

function habilitarBoton() {
    $('#btnGuardarCambios').prop('disabled', false);
}

$("#btnGuardarCambios").on("click", function () {
    // Bloqueo inmediato
    $('#btnGuardarCambios').prop('disabled', true);

    if ($("#txtTitulo").val().trim() === "") {
        MostrarToastZer("Debe Completar el Titulo.", "Atención", "warning");
        //toastr.warning("", "Debe Completar el Titulo");
        $("#txtTitulo").focus();
        habilitarBoton();
        return;
    }

    if ($("#txtDescripcion").val().trim() === "") {
        MostrarToastZer("Debe Agregar una Descripcion.", "Atención", "warning");
        //toastr.warning("", "Debe Agregar una Descripcion");
        $("#txtDescripcion").focus();
        habilitarBoton();
        return;
    }

    const objeto = {
        IdCuestionario: idEditar,
        Titulo: $("#txtTitulo").val().trim(),
        Descripcion: $("#txtDescripcion").val().trim()
    }

    $("#mdData").find("div.modal-content").LoadingOverlay("show");

    $.ajax({
        type: "POST",
        url: "PageCuestionario.aspx/GuardarOrEditCuestionario",
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
                listaCuestionarios();
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