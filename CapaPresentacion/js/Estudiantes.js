

let tablaData;
let tablaDataHis;
let tablaDetallesHi;
let idEditar = 0;

$(document).ready(function () {

    cargarUndEduca();
});

function cargarUndEduca() {
    $("#cboUnidadEdGe").html('<option value="">Cargando Unidades Educativas...</option>');
    $("#cboUnidadEdGe").prop("disabled", true);

    $.ajax({
        type: "POST",
        url: "PageUndsEducativas.aspx/ListaUndEducativas",
        data: "{}",
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (response) {
            if (response.d.Estado) {

                const lista = response.d.Data;

                // 2. Validar que la lista no sea nula y tenga elementos
                if (lista != null && lista.length > 0) {

                    let opcionesHTML = '<option value="">-- Seleccione una Unidad Educativa --</option>';

                    $.each(lista, function (i, row) {
                        opcionesHTML += `<option value="${row.IdUnidadEducativa}">${row.Nombre}</option>`;
                    });

                    $("#cboUnidadEdGe").html(opcionesHTML);
                    $("#cboUnidadEdGe").prop("disabled", false);

                } else {
                    // Si la lista está vacía
                    $("#cboUnidadEdGe").html('<option value="">Sin Unidades Educativas disponibles</option>');
                }

            } else {
                $("#cboUnidadEdGe").html('<option value="">Ocurrio un error al cargar</option>');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
            $("#cboUnidadEdGe").html('<option value="">Error de conexión</option>');
        }
    });
}

$("#cboUnidadEdGe").on("change", function () {
    const idUnidadEducativa = $(this).val();

    // 3. LIMPIAR TABLA VISUALMENTE
    if ($.fn.DataTable.isDataTable("#tbData")) {
        $("#tbData").DataTable().clear().draw();
    }

    if (idUnidadEducativa) {
        listaEstudiantes(idUnidadEducativa);
    }
});


function listaEstudiantes(idUnidadEducativa) {
    if ($.fn.DataTable.isDataTable("#tbData")) {
        $("#tbData").DataTable().destroy();
        $('#tbData tbody').empty();
    }

    var request = {
        IdUnidadEdu: parseInt(idUnidadEducativa)
    };

    tablaData = $("#tbData").DataTable({
        responsive: true,
        searching: false,
        "ajax": {
            "url": 'PageUndsEducativas.aspx/ListarEstIdUndEd',
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
            { "data": "IdEstudiante", "visible": false, "searchable": false },
            {
                //FullName es una propiedad de solo lectura que tiene Nombres y Apellidos
                "data": "FullName",
                "orderable": false,
                "searchable": false,
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
                                <small class="text-muted">${row.Correo}</small>
                            </div>
                        </div>
                    `;
                }
            },
            //{
            //    "data": "NroCi",
            //    "className": "text-center align-middle",
            //    "render": function (data) {
            //        return `<i class="fas fa-id-card-alt mr-2 text-muted"></i>${data}`;
            //    }
            //},
            //{
            //    "data": "Estado", "className": "text-center align-middle", "render": function (data) {
            //        if (data === true)
            //            return '<span class="badge badge-primary">Activo</span>';
            //        else
            //            return '<span class="badge badge-danger">No Activo</span>';
            //    }
            //},
            {
                "defaultContent": '<button class="btn btn-primary btn-informa btn-sm mr-2"><i class="fas fa-id-badge mr-2"></i>Test</button>' +
                    '<button class="btn btn-info btn-detalle btn-sm"><i class="fas fa-address-book mr-2"></i>Info</button>',
                "orderable": false,
                "searchable": false,
                "width": "150px",
                "className": "text-center align-middle"
            }
        ],
        "order": [[0, "desc"]],
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        }
    });
}

$('#tbData tbody').on('click', '.btn-informa', function () {

    let fila = $(this).closest('tr');

    if (fila.hasClass('child')) {
        fila = fila.prev();
    }

    let data = tablaData.row(fila).data();
    const textoSms = `Historial de: ${data.FullName}.`;
    listaHistorialEst(data.IdEstudiante);
    $("#lblNombreEsr").text(textoSms);
    //swal("Mensaje", textoSms, "info")

});

function listaHistorialEst(idEstudiante) {
    if ($.fn.DataTable.isDataTable("#tbDataHist")) {
        $("#tbDataHist").DataTable().destroy();
        $('#tbDataHist tbody').empty();
    }

    var request = {
        IdEstudiante: parseInt(idEstudiante)
    };

    tablaDataHis = $("#tbDataHist").DataTable({
        responsive: true,
        searching: false,
        info: false,
        "ajax": {
            "url": 'PageEstudiantes.aspx/HistorialTestEst',
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
            { "data": "IdTest", "visible": false, "searchable": false },
            {
                "data": "ObservacionGeneralIA",
                "className": "text-wrap text-muted align-middle",
                "render": function (data) {
                    // Texto en cursiva con icono de comillas
                    return `<div><i class="fas fa-quote-left fa-fw text-secondary mr-1"></i> <em>${data}</em></div>`;
                }
            },
            //{ "data": "ObservacionGeneralIA" },
            //{
            //    "data": "ObservacionGeneralIA",
            //    "render": function (data) {
            //        return `<div style="white-space: normal;">${data}</div>`;
            //    }
            //},
            {
                "defaultContent": '<button class="btn btn-success btn-ver btn-sm"><i class="fas fa-eye"></i></button>',
                "orderable": false,
                "searchable": false,
                "width": "50px",
                "className": "text-center align-middle"
            }
        ],
        "order": [[0, "desc"]],
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        }
    });
}

$('#tbDataHist tbody').on('click', '.btn-ver', function () {

    let fila = $(this).closest('tr');

    if (fila.hasClass('child')) {
        fila = fila.prev();
    }

    let data = tablaDataHis.row(fila).data();
    const textoSms = $("#lblNombreEsr").text();
    detalleHistorialEst(data.IdTest);
    $("#myModalLabeldett").text(textoSms);
    $("#mdDetalles").modal("show");
    //swal("Mensaje", textoSms, "info")

});

function detalleHistorialEst(idTest) {
    if ($.fn.DataTable.isDataTable("#tbDetalleHistor")) {
        $("#tbDetalleHistor").DataTable().destroy();
        $('#tbDetalleHistor tbody').empty();
    }

    var request = {
        IdTest: parseInt(idTest)
    };

    tablaDetallesHi = $("#tbDetalleHistor").DataTable({
        responsive: true,
        autoWidth: false,
        // Como son máximo 3 carreras, ocultamos controles para que se vea como un "Widget" limpio
        paging: false,
        searching: false,
        info: false,
        "ajax": {
            "url": 'PageEstudiantes.aspx/DetalleHistorialTest',
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
            {
                "data": "Carrera",
                "width": "25%",
                "className": "align-middle",
                "render": function (data) {
                    // Diseño hermoso: Icono de graduación en círculo + Nombre de carrera
                    return `
                        <div class="d-flex align-items-center">
                            <div class="d-flex align-items-center justify-content-center rounded-circle bg-primary text-white mr-3" style="width: 40px; height: 40px; min-width: 40px;">
                                <i class="fas fa-user-graduate" style="font-size: 1.1rem;"></i>
                            </div>
                            <div style="line-height: 1.2;">
                                <div class="font-weight-bold text-dark" style="font-size: 13px;">${data}</div>
                                <small class="text-muted">Carrera Sugerida</small>
                            </div>
                        </div>
                    `;
                }
            },
            {
                "data": "Justificacion",
                "width": "50%",
                "className": "text-wrap text-muted align-middle",
                "render": function (data) {
                    // Texto en cursiva con icono de comillas
                    return `<div><i class="fas fa-quote-left fa-fw text-secondary mr-1"></i> <em>${data}</em></div>`;
                }
            },
            {
                "data": "Puntaje",
                "width": "25%",
                "className": "align-middle",
                "render": function (data) {
                    // Redondeamos el puntaje (ej. 95.5 -> 96)
                    let puntaje = Math.round(data);

                    // Lógica de colores adaptada a las clases de TU plantilla
                    let colorClass = "progress-bar-success"; // Verde para alta afinidad (>= 80)
                    if (puntaje < 86) colorClass = "progress-bar-warning"; // 80 origi Amarillo para media (60 a 79)
                    if (puntaje < 60) colorClass = "progress-bar-danger"; // Rojo para baja (< 60)

                    // Barra de progreso de tu template
                    return `
                        <div class="text-left mb-1 font-weight-bold" style="font-size: 12px; color: #555;">
                            Afinidad IA: <span class="float-right">${puntaje}%</span>
                        </div>
                        <div class="progress" style="height: 12px;">
                            <div class="progress-bar ${colorClass} progress-bar-striped" role="progressbar" aria-valuenow="${puntaje}" aria-valuemin="0" aria-valuemax="100" style="width: ${puntaje}%">
                            </div>
                        </div>
                    `;
                }
            }
        ],
        // Mantenemos el orden por la columna 2 (Puntaje) descendente
        "order": [[2, "desc"]],
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        }
    });
}

$('#tbData tbody').on('click', '.btn-detalle', function () {

    let fila = $(this).closest('tr');

    if (fila.hasClass('child')) {
        fila = fila.prev();
    }

    let data = tablaData.row(fila).data();
    const textoSms = `Est.: ${data.FullName}.`;
    swal("Mensaje", textoSms, "success")

});

// fin