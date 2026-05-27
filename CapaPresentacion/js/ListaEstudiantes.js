
let tablaData;

$(document).ready(function () {
    cargarUndEduca();
});

function cargarUndEduca() {
    $("#cboUnidadEducativa").html('<option value="">Cargando Unidades Educativas...</option>');
    $("#cboUnidadEducativa").prop("disabled", true);

    $.ajax({
        type: "POST",
        url: "PageUndsEducativas.aspx/ListaUndEducativas",
        data: "{}",
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (response) {
            if (response.d.Estado) {
                const lista = response.d.Data;

                if (lista != null && lista.length > 0) {
                    let opcionesHTML = '<option value="">-- Seleccione una Unidad Educativa --</option>';

                    $.each(lista, function (i, row) {
                        opcionesHTML += `<option value="${row.IdUnidadEducativa}">${row.Nombre}</option>`;
                    });

                    $("#cboUnidadEducativa").html(opcionesHTML);
                    $("#cboUnidadEducativa").prop("disabled", false);

                } else {
                    $("#cboUnidadEducativa").html('<option value="">Sin Unidades Educativas disponibles</option>');
                }
            } else {
                $("#cboUnidadEducativa").html('<option value="">Ocurrio un error al cargar</option>');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
            $("#cboUnidadEducativa").html('<option value="">Error de conexión</option>');
        }
    });
}

// ==========================================
// EVENTO CHANGE DEL SELECT
// ==========================================
$("#cboUnidadEducativa").on("change", function () {
    const idUnidadEducativa = $(this).val();

    if (idUnidadEducativa) {
        // Habilitamos el botón y cargamos la tabla
        $("#btnGenerarReporte").prop("disabled", false);
        cargarEstudiantesPorColegio(idUnidadEducativa);
    } else {
        // Deshabilitamos el botón y reiniciamos el contador
        $("#btnGenerarReporte").prop("disabled", true);
        $("#lblContadorEstudiantes").text("0 Alumnos");

        // Destruimos DataTables si existe para poder restaurar el diseño original
        if ($.fn.DataTable.isDataTable("#tbDatas")) {
            $("#tbDatas").DataTable().destroy();
        }

        // Restauramos el Placeholder visualmente
        $("#tbDatas tbody").html(`
            <tr id="rowPlaceholder">
                <td colspan="3" class="text-center text-muted pt-30 pb-30">
                    <i class="fas fa-school fa-2x mb-3 d-block text-success"></i>
                    Por favor, seleccione una Unidad Educativa en el panel izquierdo para ver los registros.
                </td>
            </tr>
        `);
    }
});

// ==========================================
// CARGA Y RENDERIZADO DE DATATABLES
// ==========================================
function cargarEstudiantesPorColegio(idUnidadEducativa) {
    if ($.fn.DataTable.isDataTable("#tbDatas")) {
        $("#tbDatas").DataTable().destroy();
        $('#tbDatas tbody').empty();
    }

    var request = {
        IdUnidadEdu: parseInt(idUnidadEducativa)
    };

    tablaData = $("#tbDatas").DataTable({
        responsive: true,
        searching: false, // Desactivado si solo muestras los de un colegio
        "ajax": {
            "url": 'PageUndsEducativas.aspx/ListarEstIdUndEd',
            "type": "POST",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": function () {
                return JSON.stringify(request);
            },
            "dataSrc": function (json) {
                // AQUÍ INTERCEPTAMOS LOS DATOS PARA EL CONTADOR
                if (json.d.Estado) {
                    let datos = json.d.Data;
                    $("#lblContadorEstudiantes").text(datos.length + " Alumnos");
                    return datos;
                } else {
                    $("#lblContadorEstudiantes").text("0 Alumnos");
                    return [];
                }
            }
        },
        "columns": [
            { "data": "IdEstudiante", "visible": false, "searchable": false },
            {
                "data": "FullName",
                "orderable": false,
                "searchable": false,
                "className": "align-middle",
                "render": function (data, type, row) {
                    let imgUrl = row.Photo || 'images/sinimagen.png';

                    // Diseño mejorado usando clases de espaciado del Webadmin
                    return `
                        <div class="media align-items-center">
                            <img src="${imgUrl}" alt="Estudiante" class="thumb-md rounded-circle mr-2 bx-shadow" style="height:40px" onerror="this.src='images/sinimagen.png';"> 
                            <div class="media-body">
                                <h5 class="m-0 font-13 text-dark">${data}</h5>
                                <p class="text-muted mb-0 font-13"><i class="fa fa-envelope mr-2"></i>${row.Correo}</p>
                            </div>
                        </div>
                    `;
                }
            },
            {
                "data": "NroCi",
                "className": "align-middle text-right", // Alineado a la derecha para equilibrar
                "render": function (data, type, row) {
                    // EVALUACIÓN DEL ESTADO BOOLEANO
                    let badgeEstado = row.Estado
                        ? '<span class="badge badge-success p-1"><i class="fa fa-check mr-2"></i>Activo</span>'
                        : '<span class="badge badge-danger p-1"><i class="fa fa-times mr-2"></i>Inactivo</span>';

                    return `
                        <div>
                            <span class="text-dark font-weight-bold d-block mb-1"><i class="fa fa-id-card text-muted mr-2"></i>CI: ${data}</span>
                            ${badgeEstado}
                        </div>
                    `;
                }
            }
        ],
        "order": [[0, "desc"]],
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json",
            "emptyTable": "No se encontraron estudiantes registrados en esta Unidad Educativa."
        }
    });
}

// Evento del botón de Reporte
$("#btnGenerarReporte").on("click", function () {
    var idColegio = $("#cboUnidadEducativa").val();
    // Aquí ejecutas tu lógica de jsPDF o redirección para descargar el reporte
    // Ejemplo: abrirReportePDF(idColegio);

    MostrarAlerta("¡Mensaje!", "En construcción", "info");
});

// fin