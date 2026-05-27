
let tablaData;
let idEditar = 0;

// fin
$(document).ready(function () {

    cargarCuestionarios();
    cargarCuestionariosModal();
});

function cargarCuestionarios() {
    $("#cboCuestionarioGe").html('<option value="">Cargando Cuestionarios...</option>');
    $("#cboCuestionarioGe").prop("disabled", true);

    $.ajax({
        type: "POST",
        url: "PageCuestionario.aspx/ListaCuestionarios",
        data: "{}",
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (response) {
            if (response.d.Estado) {

                const lista = response.d.Data;

                // 2. Validar que la lista no sea nula y tenga elementos
                if (lista != null && lista.length > 0) {

                    let opcionesHTML = '<option value="">-- Seleccione un Cuestionario --</option>';

                    $.each(lista, function (i, row) {
                        opcionesHTML += `<option value="${row.IdCuestionario}">${row.Titulo}</option>`;
                    });

                    $("#cboCuestionarioGe").html(opcionesHTML);
                    $("#cboCuestionarioGe").prop("disabled", false);

                } else {
                    // Si la lista está vacía
                    $("#cboCuestionarioGe").html('<option value="">Sin Cuestionarios disponibles</option>');
                }

            } else {
                $("#cboCuestionarioGe").html('<option value="">Ocurrio un error al cargar</option>');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
            $("#cboCuestionarioGe").html('<option value="">Error de conexión</option>');
        }
    });
}

function cargarCuestionariosModal() {
    $("#cboCuestionario").html('<option value="">Cargando Cuestionarios...</option>');
    $("#cboCuestionario").prop("disabled", true);

    $.ajax({
        type: "POST",
        url: "PageCuestionario.aspx/ListaCuestionarios",
        data: "{}",
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (response) {
            if (response.d.Estado) {

                const lista = response.d.Data;

                // 2. Validar que la lista no sea nula y tenga elementos
                if (lista != null && lista.length > 0) {

                    let opcionesHTML = '<option value="">-- Seleccione un Cuestionario --</option>';

                    $.each(lista, function (i, row) {
                        opcionesHTML += `<option value="${row.IdCuestionario}">${row.Titulo}</option>`;
                    });

                    $("#cboCuestionario").html(opcionesHTML);
                    $("#cboCuestionario").prop("disabled", false);

                } else {
                    // Si la lista está vacía
                    $("#cboCuestionario").html('<option value="">Sin Cuestionarios disponibles</option>');
                }

            } else {
                $("#cboCuestionario").html('<option value="">Ocurrio un error al cargar</option>');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
            $("#cboCuestionario").html('<option value="">Error de conexión</option>');
        }
    });
}

$("#cboCuestionarioGe").on("change", function () {
    const idCuestionario = $(this).val();

    // 3. LIMPIAR TABLA VISUALMENTE
    if ($.fn.DataTable.isDataTable("#tbDatas")) {
        $("#tbDatas").DataTable().clear().draw();
    }

    if (idCuestionario) {
        listaPreguntas(idCuestionario);
    }
});

function listaPreguntas(idCuestionario) {
    if ($.fn.DataTable.isDataTable("#tbDatas")) {
        $("#tbDatas").DataTable().destroy();
        $('#tbDatas tbody').empty();
    }

    var request = {
        IdCuestionario: parseInt(idCuestionario)
    };

    tablaData = $("#tbDatas").DataTable({
        responsive: true,
        "ajax": {
            "url": 'PagePreguntas.aspx/ListaPreguntasId',
            "type": "POST",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": function () {
                return JSON.stringify(request);
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
            { "data": "IdPregunta", "visible": false, "searchable": false },
            { "data": "Texto", "className": "align-middle" },
            {
                "defaultContent": '<button class="btn btn-primary btn-editar btn-sm"><i class="fas fa-pencil-alt mr-2"></i>Editar</button>',
                "orderable": false,
                "searchable": false,
                "className": "text-center align-middle",
                "width": "100px"
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
    idEditar = data.IdPregunta;
    $("#txtPregunta").val(data.Texto);
    $("#cboCuestionario").val(data.IdCuestionario);
    $("#myModalLabel").text("Editar Pregunta");
    $("#mdData").modal("show");

});

$("#btnRegistro").on("click", function () {

    idEditar = 0;
    $("#txtPregunta").val("");
    $("#cboCuestionario").val("");
    $("#myModalLabel").text("Nuevo Registro");

    $("#mdData").modal("show");
})


//$("#btnPrueba").on("click", function () {

//    $("#cboCuestionarioGe").val(2);
//})

function habilitarBoton() {
    $('#btnGuardarCambios').prop('disabled', false);
}

$("#btnGuardarCambios").on("click", function () {
    // Bloqueo inmediato
    $('#btnGuardarCambios').prop('disabled', true);
    let idCuestionario = $("#cboCuestionario").val();

    if ($("#txtPregunta").val().trim() === "") {
        MostrarToastZer("Debe Completar la pregunta.", "Atención", "warning");
        //toastr.warning("", "Debe Completar el Titulo");
        $("#txtPregunta").focus();
        habilitarBoton();
        return;
    }

    if (idCuestionario === "") {
        MostrarToastZer("Debe seleccionar un cuestionario.", "Atención", "warning");
        //toastr.warning("", "Debe Agregar una Descripcion");
        $("#cboCuestionario").focus();
        habilitarBoton();
        return;
    }

    const objeto = {
        IdPregunta: idEditar,
        IdCuestionario: parseInt($("#cboCuestionario").val()),
        Texto: $("#txtPregunta").val().trim()
    }

    $("#mdData").find("div.modal-content").LoadingOverlay("show");

    $.ajax({
        type: "POST",
        url: "PagePreguntas.aspx/GuardarOrEditPregunta",
        data: JSON.stringify({ objeto: objeto }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#mdData").find("div.modal-content").LoadingOverlay("hide");
            //MostrarToastZer("El registro se actualizó correctamente.", "Éxito", "success");
            MostrarToastZer(
                response.d.Mensaje, // Texto del servidor
                response.d.Estado ? '¡Éxito!' : 'Atención', // Título dinámico
                response.d.Valor // Icono (success/error/warning)
            );

            if (response.d.Estado) {
                $("#mdData").modal("hide");
                $("#cboCuestionarioGe").val(idCuestionario);
                listaPreguntas(idCuestionario);
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