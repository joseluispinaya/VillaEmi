
function MostrarToastZer(mensaje, titulo, tipo) {

    toastr.options = {
        "closeButton": true,          // Muestra una "X" para cerrar manualmente
        "progressBar": true,          // Muestra una barra de tiempo agotándose
        "positionClass": "toast-top-right", // Posición en pantalla
        "preventDuplicates": false,   // Evita que el mismo mensaje se repita varias veces seguidas
        "timeOut": "3000",            // Tiempo que dura en pantalla (3 segundos)
        "extendedTimeOut": "1000"     // Tiempo extra si el usuario pasa el mouse por encima
    };

    // Si no se envía un tipo, por defecto será 'info'
    let tipoToast = tipo || "info";

    // Ejecutamos la función dinámica de toastr
    toastr[tipoToast](mensaje, titulo || "");
}

function MostrarAlerta(titulo, mensaje, tipo) {
    // Si no se envía un tipo, por defecto será 'success'
    swal(titulo, mensaje, tipo || "success");
}

// Ejemplo de uso:
// MostrarAlerta("¡Guardado!", "El registro se guardó correctamente.", "success");