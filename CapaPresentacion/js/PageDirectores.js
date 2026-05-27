
let tablaData;
let idEditar = 0;

$(document).ready(function () {

    listaDirectores();
});

function listaDirectores() {
    if ($.fn.DataTable.isDataTable("#tbDatas")) {
        $("#tbDatas").DataTable().destroy();
        $('#tbDatas tbody').empty();
    }

    tablaData = $("#tbDatas").DataTable({
        responsive: true,
        "ajax": {
            "url": 'PageDirectores.aspx/ListaDirectores',
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
            { "data": "IdDirector", "visible": false, "searchable": false },
            {
                //FullName es una propiedad de solo lectura que tiene Nombres y Apellidos
                "data": "FullName",
                "className": "align-middle",
                "render": function (data, type, row) {
                    let colorBorde = '#fff';
                    let imgUrl = row.Photo || 'images/sinimagen.png';

                    return `
                        <div class="d-flex align-items-center">
                            <div class="position-relative mr-3">
                                <img src="${imgUrl}" 
                                     alt="Logo"
                                     class="rounded-circle"
                                     style="width: 40px; height: 40px; object-fit: cover; border: 1px solid ${colorBorde}; padding: 2px; background: #fff;"
                                     onerror="this.src='/images/sinimagen.png';"> 
                            </div>
                            <div style="line-height: 1.2;">
                                <span class="font-weight-bold text-dark" style="font-size: 1em;">
                                    ${data}
                                </span>
                                <br/>
                                <small class="text-muted">Nro CI: ${row.NroCi}</small>
                            </div>
                        </div>
                    `;
                }
            },
            { "data": "Correo", "className": "align-middle" },
            { "data": "NombreUnidadEducativa", "className": "align-middle" },
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
        "order": [[0, "desc"]],
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        }
    });
}

// Agregamos el parámetro idSeleccionar
function cargarUndidadesEd(idUnidadEdu, idSeleccionar) {
    $("#cboUnidadEd").html('<option value="">Cargando unds educativas...</option>');
    $("#cboUnidadEd").prop("disabled", true);

    var request = {
        IdUnidadEdu: parseInt(idUnidadEdu)
    };

    $.ajax({
        url: "PageUndsEducativas.aspx/ListaUndEducativasDisponibles",
        type: "POST",
        data: JSON.stringify(request),
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (response) {
            if (response.d.Estado) {

                const lista = response.d.Data;

                if (lista != null && lista.length > 0) {

                    let opcionesHTML = '<option value="">-- Seleccione Und Educativa --</option>';

                    $.each(lista, function (i, row) {
                        opcionesHTML += `<option value="${row.IdUnidadEducativa}">${row.Nombre}</option>`;
                    });

                    $("#cboUnidadEd").html(opcionesHTML);
                    $("#cboUnidadEd").prop("disabled", false);

                    // LA MAGIA ESTÁ AQUÍ: Una vez pintado el HTML, seleccionamos el valor si existe
                    if (idSeleccionar != null && idSeleccionar != "") {
                        $("#cboUnidadEd").val(idSeleccionar);
                    } else {
                        $("#cboUnidadEd").val(""); // Deja la opción por defecto
                    }

                } else {
                    $("#cboUnidadEd").html('<option value="">Sin unds educativas disponibles</option>');
                    $("#cboUnidadEd").prop("disabled", true);
                }

            } else {
                $("#cboUnidadEd").html('<option value="">Error al cargar</option>');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
            $("#cboUnidadEd").html('<option value="">Error de conexión</option>');
        }
    });
}

$('#tbDatas tbody').on('click', '.btn-editar', function () {

    let fila = $(this).closest('tr');
    if (fila.hasClass('child')) {
        fila = fila.prev();
    }

    let data = tablaData.row(fila).data();
    idEditar = data.IdDirector;

    // Llamamos a la función: 
    // data.IdUnidadEducativa para que la incluya en la consulta SQL
    // data.IdUnidadEducativa de nuevo para que el select se posicione en ella al terminar
    cargarUndidadesEd(data.IdUnidadEducativa, data.IdUnidadEducativa);

    $("#txtNombres").val(data.Nombres);
    $("#txtApellidos").val(data.Apellidos);
    $("#txtCorreo").val(data.Correo);
    $("#txtCelular").val(data.Celular);
    $("#txtNroci").val(data.NroCi);
    $("#cboEstado").val(data.Estado ? 1 : 0).prop("disabled", false);

    $("#imgDirectReg").attr("src", data.Photo || "images/sinimagen.png");
    $("#txtFotoUr").val("");
    $(".custom-file-label-upload").text('Ningún archivo seleccionado');

    $("#myModalLabel").text("Editar Registro");
    $("#mdData").modal("show");
});

$('#tbDatas tbody').on('click', '.btn-detalle', function () {

    let fila = $(this).closest('tr');

    if (fila.hasClass('child')) {
        fila = fila.prev();
    }

    let data = tablaData.row(fila).data();
    const textoSms = `Detalles del Director: ${data.Nombres}.`;
    MostrarAlerta("¡Mensaje!", textoSms, "info");

});

$("#btnRegistro").on("click", function () {

    idEditar = 0;
    // Llamamos a la función: 0 = Unidad actual (ninguna), "" = ID a seleccionar (ninguno)
    cargarUndidadesEd(0, "");

    $("#txtNombres").val("");
    $("#txtApellidos").val("");
    $("#txtCorreo").val("");
    $("#txtCelular").val("");
    $("#txtNroci").val("");
    $("#cboEstado").val(1).prop("disabled", true);

    $('#imgDirectReg').attr('src', "images/sinimagen.png");
    $("#txtFotoUr").val("");
    $(".custom-file-label-upload").text('Ningún archivo seleccionado');

    $("#myModalLabel").text("Nuevo Registro");

    $("#mdData").modal("show");

})

function esImagen(file) {
    return file && file.type.startsWith("image/");
}

function mostrarImagenSeleccionada(input) {
    let file = input.files[0];
    let reader = new FileReader();

    // Si NO se seleccionó archivo (ej: presionaron "Cancelar")
    if (!file) {
        $('#imgDirectReg').attr('src', "images/sinimagen.png");
        $(input).next('.custom-file-label-upload').text('Ningún archivo seleccionado');
        return;
    }

    // Validación: si no es imagen, mostramos error
    if (!esImagen(file)) {
        toastr.error("El archivo seleccionado no es una imagen válida.");
        $('#imgDirectReg').attr('src', "images/sinimagen.png");
        $(input).next('.custom-file-label-upload').text('Ningún archivo seleccionado');
        input.value = ""; // Limpia el input de archivo
        return;
    }

    // Si todo es válido → mostrar vista previa
    reader.onload = (e) => $('#imgDirectReg').attr('src', e.target.result);
    reader.readAsDataURL(file);

    // Mostrar nombre del archivo
    $(input).next('.custom-file-label-upload').text(file.name);
}

$('#txtFotoUr').change(function () {
    mostrarImagenSeleccionada(this);
});

function habilitarBoton() {
    $('#btnGuardarCambios').prop('disabled', false);
}

$("#btnGuardarCambios").on("click", function () {
    // Bloqueo inmediato
    $('#btnGuardarCambios').prop('disabled', true);

    const inputs = $("#mdData input.model").serializeArray();
    const inputs_sin_valor = inputs.filter(item => item.value.trim() === "");

    if (inputs_sin_valor.length > 0) {
        const mensaje = `Debe completar el campo: "${inputs_sin_valor[0].name}"`;
        toastr.warning("", mensaje)
        $(`input[name="${inputs_sin_valor[0].name}"]`).focus();
        habilitarBoton();
        return;
    }

    if ($("#cboUnidadEd").val() === "") {
        toastr.warning("Debe seleccionar Und Educativa.");
        $("#cboUnidadEd").focus();
        habilitarBoton();
        return;
    }

    // 2. ARMAR EL OBJETO
    const objeto = {
        IdDirector: idEditar,
        IdUnidadEducativa: parseInt($("#cboUnidadEd").val()),
        Nombres: $("#txtNombres").val().trim(), // Ojo, tenías un error de tipeo aquí ("txtNombrees")
        Apellidos: $("#txtApellidos").val().trim(),
        NroCi: $("#txtNroci").val().trim(),
        Celular: $("#txtCelular").val().trim(),
        Correo: $("#txtCorreo").val().trim(),
        Estado: ($("#cboEstado").val() === "1" ? true : false),
        Photo: "" // Lo enviamos siempre vacío. Si hay foto nueva, el Base64 la reemplazará en C#.
    };

    // 3. PROCESAR EL INPUT FILE
    const fileInput = document.getElementById('txtFotoUr');
    const file = fileInput.files[0];

    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            // Extraemos solo el texto Base64, quitando la cabecera (data:image/jpeg;base64,)
            const base64String = e.target.result.split(',')[1];

            // Disparamos el AJAX enviando la imagen
            enviarAjaxDirector(objeto, base64String);
        };
        reader.readAsDataURL(file);
    } else {
        // Si no hay foto, disparamos el AJAX mandando el base64 vacío
        enviarAjaxDirector(objeto, "");
    }
});

function enviarAjaxDirector(objeto, base64String) {
    $("#mdData").find("div.modal-content").LoadingOverlay("show");

    $.ajax({
        type: "POST",
        url: "PageDirectores.aspx/GuardarOrEditDirector",
        data: JSON.stringify({ objeto: objeto, base64Image: base64String }),
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
                listaDirectores();
                idEditar = 0;
            }
        },
        error: function () {
            $("#mdData").find("div.modal-content").LoadingOverlay("hide");
            toastr.error("No se pudo conectar con el servidor.");
        },
        complete: function () {
            habilitarBoton();
        }
    });
}

// fin