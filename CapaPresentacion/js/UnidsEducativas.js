
let tablaData;
let tablaDataEst;
let idEditar = 0;
let listaTemp = [];

$(document).ready(function () {

    listaUnidadesEd();
});

function listaUnidadesEd() {
    if ($.fn.DataTable.isDataTable("#tbDatas")) {
        $("#tbDatas").DataTable().destroy();
        $('#tbDatas tbody').empty();
    }

    tablaData = $("#tbDatas").DataTable({
        responsive: true,
        "ajax": {
            "url": 'PageUndsEducativas.aspx/ListaUndEducativas',
            "type": "POST",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": function (d) {
                return JSON.stringify(d);
            },
            "dataSrc": function (json) {
                if (json.d.Estado) {
                    $("#txtTotalUE").text(json.d.Data.length);
                    return json.d.Data;
                } else {
                    return [];
                }
            }
        },
        "columns": [
            {
                "data": "IdUnidadEducativa",
                "visible": false,
                "searchable": false
            },
            // UNIDAD EDUCATIVA + RESPONSABLE
            {
                "data": null,
                "className": "align-middle",
                "render": function (data, type, row) {
                    return `
                    <div class="d-flex align-items-center">
                        <div class="icon-unidad mr-3">
                            <i class="fas fa-school"></i>
                        </div>
                        <div>
                            <div class="font-weight-bold text-dark">
                                ${row.Nombre}
                            </div>
                            <small class="text-muted">
                                <i class="fas fa-user-tie mr-1"></i>
                                ${row.Responsable}
                            </small>
                        </div>
                    </div>
                    `;
                }
            },
            // CONTACTO + FECHA
            {
                "data": null,
                "className": "align-middle",
                "render": function (data, type, row) {
                    return `
                    <div>
                        <div class="mb-1">
                            <i class="fas fa-phone text-success mr-1"></i>
                            ${row.NroContacto}
                        </div>
                        <small class="text-muted">
                            <i class="far fa-calendar-alt mr-1"></i>
                            ${row.FechaCreado}
                        </small>
                    </div>
                    `;
                }
            },

            // ACCIONES
            {
                "defaultContent":
                    `
                    <div class="text-center">
                        <button class="btn btn-primary btn-sm btn-action btn-editar"
                                title="Editar">
                            <i class="fas fa-pencil-alt"></i>
                        </button>
                        <button class="btn btn-info btn-sm btn-action btn-detalle"
                                title="Detalle">
                            <i class="fas fa-address-book"></i>
                        </button>
                    </div>
                `,
                "orderable": false,
                "searchable": false,
                "width": "110px",
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
    idEditar = data.IdUnidadEducativa;
    $("#txtnombres").val(data.Nombre);
    $("#txtNroCel").val(data.NroContacto);
    $("#txtUbicacion").val(data.Ubicacion);
    $("#myModalLabel").text("Editar Registro");
    $("#mdData").modal("show");

});

$('#tbDatas tbody').on('click', '.btn-detalle', function () {

    let fila = $(this).closest('tr');

    if (fila.hasClass('child')) {
        fila = fila.prev();
    }

    let data = tablaData.row(fila).data();
    const textoSms = `U.E.: ${data.Nombre}.`;
    swal("Mensaje", textoSms, "success")

});

$("#btnRegistro").on("click", function () {

    idEditar = 0;
    $("#txtnombres").val("");
    $("#txtNroCel").val("");
    $("#txtUbicacion").val("");

    $("#myModalLabel").text("Nuevo Registro");

    $("#mdData").modal("show");

})

function habilitarBoton() {
    $('#btnGuardarCambios').prop('disabled', false);
}

$("#btnGuardarCambios").on("click", function () {
    // Bloqueo inmediato
    $('#btnGuardarCambios').prop('disabled', true);

    const inputs = $("#mdData input.model").serializeArray();
    const inputs_sin_valor = inputs.filter(item => item.value.trim() === "");

    if (inputs_sin_valor.length > 0) {
        const mensaje = `Debe completar el campo : "${inputs_sin_valor[0].name}"`;
        toastr.warning("", mensaje)
        $(`input[name="${inputs_sin_valor[0].name}"]`).focus()
        habilitarBoton();
        return;
    }

    const objeto = {
        IdUnidadEducativa: idEditar,
        Nombre: $("#txtnombres").val().trim(),
        NroContacto: $("#txtNroCel").val().trim(),
        Ubicacion: $("#txtUbicacion").val().trim()
        //Estado: ($("#cboEstado").val() == "1" ? true : false)
    }

    $("#mdData").find("div.modal-content").LoadingOverlay("show");

    $.ajax({
        type: "POST",
        url: "PageUndsEducativas.aspx/GuardarOrEditUnidadesEdu",
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
                listaUnidadesEd();
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

// fin codigo